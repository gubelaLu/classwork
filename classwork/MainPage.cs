using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text.Json;
using System.Windows.Forms;

namespace classwork
{
    public partial class MainPage : Form
    {
        private const string SettingsFilePath =
    @"C:\Users\Administrator\Source\Repos\classworkL\classwork\appSettings.json";

        private AppSettings appSettings = new();

        private string username;
        private string role;

        private System.Windows.Forms.Timer updateTimer;
        private SerialPort espSerial;

        private const string RulesFilePath =
            @"C:\Users\Administrator\Source\Repos\classworkL\classwork\controlRules.json";

        private ControlRules rules;

        // ================= SPRINKLER (MODIFIABLE) =================
        private TimeSpan SprinklerStartTime = new TimeSpan(12, 0, 0);
        private TimeSpan SprinklerDuration = TimeSpan.FromMinutes(10);

        private bool sprinklerActive;
        private DateTime? sprinklerEndTime;
        private DateTime lastSprinklerRunDate = DateTime.MinValue;

        // ================= SENSORS =================
        private class SensorInfo
        {
            public bool IsOn { get; set; }
            public DateTime? StartTime { get; set; }
            public TimeSpan TotalOnTime { get; set; }
        }

        private readonly SensorInfo TemperatureSensor = new();
        private readonly SensorInfo HumiditySensor = new();
        private readonly SensorInfo SoilSensor = new();

        // ================= ACTUATORS =================
        private class Actuator
        {
            public bool IsOn { get; set; }
            public DateTime? StartTime { get; set; }
            public TimeSpan TotalOnTime { get; set; }
        }

        private readonly Actuator Fan = new();
        private readonly Actuator Heater = new();
        private readonly Actuator Pump = new();

        // ================= RULE CLASSES =================
        private class ControlRules
        {
            public HeaterRules Heater { get; set; } = new();
            public FanRules Fan { get; set; } = new();
            public PumpRules Pump { get; set; } = new();
        }

        private class HeaterRules
        {
            public double OnBelow { get; set; }
            public double OffAbove { get; set; }
        }

        private class FanRules
        {
            public double OnAboveTemp { get; set; }
            public double OffBelowTemp { get; set; }
            public double OnAboveHumidity { get; set; }
            public double OffBelowHumidity { get; set; }
        }

        private class PumpRules
        {
            public double OnBelowSoil { get; set; }
            public double OffAboveSoil { get; set; }
        }

        public MainPage()
        {
            InitializeComponent();
            InitializeLogic();
            this.Shown += (_, __) => UpdateAllUI();
        }

        public MainPage(string username, string role)
        {
            this.username = username;
            this.role = role;
            InitializeComponent();
            InitializeLogic();
            this.Shown += (_, __) => UpdateAllUI();
        }

        private void InitializeLogic()
        {
            labelUser.Text = !string.IsNullOrEmpty(username)
                ? $"User: {username} ({role})"
                : "User: Unknown";

            rules = LoadRulesFromJson();

            // 1️⃣ Button click wiring
            btnTempToggle.Click += (_, __) =>
            {
                if (!IsAdmin())
                {
                    ShowPermissionDenied();
                    return;
                }
                ToggleSensor(TemperatureSensor);
            };

            btnHumidityToggle.Click += (_, __) =>
            {
                if (!IsAdmin())
                {
                    ShowPermissionDenied();
                    return;
                }
                ToggleSensor(HumiditySensor);
            };

            btnSoilToggle.Click += (_, __) =>
            {
                if (!IsAdmin())
                {
                    ShowPermissionDenied();
                    return;
                }
                ToggleSensor(SoilSensor);
            };
            LoadAppSettings();

            btnLogout.Click += btnLogout_Click;

            // 👉 STEP 2 GOES **HERE**
            ApplyPermissions();

            // 2️⃣ Sensors startup state
            TurnSensorOnAtStartup(TemperatureSensor);
            TurnSensorOnAtStartup(HumiditySensor);
            TurnSensorOnAtStartup(SoilSensor);

            // 3️⃣ Timer
            updateTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            updateTimer.Tick += (_, __) => UpdateAllUI();
            updateTimer.Start();

            // 4️⃣ ESP
            InitializeEspConnection();
        }


        // ================= UI UPDATE =================
        private void UpdateAllUI()
        {
            UpdateSprinklerFromUI();

            UpdateSensorUI(TemperatureSensor, btnTempToggle, "Temperature", txtTemperature);
            UpdateSensorUI(HumiditySensor, btnHumidityToggle, "Humidity", txtHumidity);
            UpdateSensorUI(SoilSensor, btnSoilToggle, "Soil", txtSoil);

            ApplyAutomationRules();

            UpdateActuatorUI(Fan, lblFanStatus, lblFanTime, "Fan");
            UpdateActuatorUI(Heater, lblHeaterStatus, lblHeaterTime, "Heater");
            UpdateActuatorUI(Pump, lblPumpStatus, lblPumpTime, "Pump");

            lblSprinklerInfo.Text =
                $"Sprinkler: {SprinklerStartTime:hh\\:mm} for {SprinklerDuration.TotalMinutes} min";
        }

        // ================= SPRINKLER UI PARSING =================
        private void UpdateSprinklerFromUI()
        {
            if (TimeSpan.TryParseExact(txtSprinklerTime.Text, @"hh\:mm", null, out var time))
                SprinklerStartTime = time;

            if (int.TryParse(txtSprinklerDuration.Text, out var minutes) && minutes > 0)
                SprinklerDuration = TimeSpan.FromMinutes(minutes);
        }

        private bool ApplyTimedSprinkler()
        {
            var now = DateTime.Now;

            if (sprinklerActive && sprinklerEndTime.HasValue && now >= sprinklerEndTime.Value)
            {
                sprinklerActive = false;
                sprinklerEndTime = null;
            }

            if (!sprinklerActive &&
                lastSprinklerRunDate.Date != now.Date &&
                now.TimeOfDay >= SprinklerStartTime &&
                now.TimeOfDay < SprinklerStartTime.Add(TimeSpan.FromMinutes(1)))
            {
                sprinklerActive = true;
                sprinklerEndTime = now.Add(SprinklerDuration);
                lastSprinklerRunDate = now.Date;
            }

            return sprinklerActive;
        }

        // ================= AUTOMATION =================
        private void ApplyAutomationRules()
        {
            double? temp = TryParseDouble(txtTemperature.Text);
            double? hum = TryParseDouble(txtHumidity.Text);
            double? soil = TryParseDouble(txtSoil.Text);

            bool sprinkler = ApplyTimedSprinkler();

            SetActuatorState(Heater,
                temp.HasValue &&
                (!Heater.IsOn ? temp <= rules.Heater.OnBelow : temp < rules.Heater.OffAbove));

            SetActuatorState(Fan,
                !Fan.IsOn
                    ? (temp >= rules.Fan.OnAboveTemp || hum >= rules.Fan.OnAboveHumidity)
                    : (temp > rules.Fan.OffBelowTemp || hum > rules.Fan.OffBelowHumidity));

            SetActuatorState(Pump,
                sprinkler ||
                (soil.HasValue &&
                 (!Pump.IsOn ? soil <= rules.Pump.OnBelowSoil : soil < rules.Pump.OffAboveSoil)));
        }

        // ================= HELP =================
        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                $"Sprinkler:\n" +
                $"- Start: {SprinklerStartTime:hh\\:mm}\n" +
                $"- Duration: {SprinklerDuration.TotalMinutes} min\n\n" +
                "Sensors:\nDHT11 (Temp/Humidity)\nAR0182 (Soil)",
                "Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ================= UTILS =================
        private static double? TryParseDouble(string t)
        {
            if (string.IsNullOrWhiteSpace(t)) return null;
            t = t.Replace(',', '.');
            return double.TryParse(t, NumberStyles.Float, CultureInfo.InvariantCulture, out var v) ? v : null;
        }

        private static void TurnSensorOnAtStartup(SensorInfo s)
        {
            s.IsOn = true;
            s.StartTime = DateTime.Now;
        }
        private bool IsAdmin()
        {
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        private void ToggleSensor(SensorInfo s)
        {
            if (s.IsOn)
            {
                s.IsOn = false;
                s.TotalOnTime += DateTime.Now - s.StartTime.Value;
                s.StartTime = null;
            }
            else
            {
                s.IsOn = true;
                s.StartTime = DateTime.Now;
            }
        }

        private void SetActuatorState(Actuator a, bool on)
        {
            if (on && !a.IsOn)
            {
                a.IsOn = true;
                a.StartTime = DateTime.Now;
            }
            else if (!on && a.IsOn)
            {
                a.IsOn = false;
                a.TotalOnTime += DateTime.Now - a.StartTime.Value;
                a.StartTime = null;
            }
        }
        private void ShowPermissionDenied()
        {
            MessageBox.Show(
                "You do not have permission to change sensor states.\n\n" +
                "Only administrators can enable or disable sensors.",
                "Permission Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void UpdateSensorUI(SensorInfo s, Button b, string name, TextBox box)
        {
            var elapsed = s.TotalOnTime +
                (s.IsOn && s.StartTime.HasValue ? DateTime.Now - s.StartTime.Value : TimeSpan.Zero);

            var value = string.IsNullOrWhiteSpace(box.Text) ? "N/A" : box.Text;

            b.Text = $"{name}: {(s.IsOn ? "ON" : "OFF")} | {value} | {elapsed:hh\\:mm\\:ss}";
            b.ForeColor = s.IsOn ? Color.Green : Color.Red;
            box.Enabled = s.IsOn;
        }

        private void UpdateActuatorUI(Actuator a, Label s, Label t, string name)
        {
            var elapsed = a.TotalOnTime +
                (a.IsOn && a.StartTime.HasValue ? DateTime.Now - a.StartTime.Value : TimeSpan.Zero);

            s.Text = $"{name}: {(a.IsOn ? "ON" : "OFF")}";
            s.ForeColor = a.IsOn ? Color.Green : Color.Red;
            t.Text = elapsed.ToString(@"hh\:mm\:ss");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Try to find the already opened Form1 (start screen)
            foreach (Form form in Application.OpenForms)
            {
                if (form is Form1)
                {
                    form.Show();
                    form.BringToFront();
                    this.Close();
                    return;
                }
            }

            // If Form1 is not found (edge case), create it
            var startForm = new Form1();
            startForm.Show();

            this.Close();
        }

        private void ApplyPermissions()
        {
            bool isAdmin = IsAdmin();

            btnTempToggle.Enabled = isAdmin;
            btnHumidityToggle.Enabled = isAdmin;
            btnSoilToggle.Enabled = isAdmin;

            if (!isAdmin)
            {
                btnTempToggle.Text += " (Admin only)";
                btnHumidityToggle.Text += " (Admin only)";
                btnSoilToggle.Text += " (Admin only)";
            }
        }

        private void btnEspSend_Click(object sender, EventArgs e)
        {
            var cmd = txtEspCommand.Text?.Trim();

            if (string.IsNullOrWhiteSpace(cmd))
                return;

            if (espSerial == null || !espSerial.IsOpen)
            {
                txtEspLog.AppendText("[ESP] Not connected\n");
                return;
            }

            espSerial.WriteLine(cmd);
            txtEspLog.AppendText($"> {cmd}{Environment.NewLine}");
            txtEspCommand.Clear();
        }
        private void txtSprinklerDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits and control keys (Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtSprinklerDuration_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtSprinklerTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
        !char.IsDigit(e.KeyChar) &&
        e.KeyChar != ':')
            {
                e.Handled = true;
            }
        }

        private void txtSprinklerTime_Leave(object sender, EventArgs e)
        {
            var text = txtSprinklerTime.Text;

            // Must be exactly HH:mm
            var parts = text.Split(':');
            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out int hour) ||
                !int.TryParse(parts[1], out int minute))
            {
                ShowInvalidTime();
                return;
            }

            // Standard time ranges
            if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
            {
                ShowInvalidTime();
                return;
            }

            // Normalize format (e.g. 9:5 -> 09:05)
            txtSprinklerTime.Text = $"{hour:D2}:{minute:D2}";
            SaveAppSettings();

        }

        private void ShowInvalidTime()
        {
            MessageBox.Show(
                "Please enter a valid time in HH:mm format.\n\n" +
                "- Hours: 0 to 23\n" +
                "- Minutes: 0 to 59",
                "Invalid Sprinkler Time",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtSprinklerTime.Text = SprinklerStartTime.ToString(@"hh\:mm");
            txtSprinklerTime.Focus();
        }

        private class AppSettings
        {
            public string SprinklerTime { get; set; } = "12:00";
            public int SprinklerDurationMinutes { get; set; } = 10;

            public string TemperatureValue { get; set; } = "";
            public string HumidityValue { get; set; } = "";
            public string SoilValue { get; set; } = "";
        }

        private void LoadAppSettings()
        {
            try
            {
                if (!File.Exists(SettingsFilePath))
                    return;

                var json = File.ReadAllText(SettingsFilePath);
                appSettings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch
            {
                appSettings = new AppSettings();
            }

            // Apply to UI + logic
            txtSprinklerTime.Text = appSettings.SprinklerTime;
            txtSprinklerDuration.Text = appSettings.SprinklerDurationMinutes.ToString();

            if (TimeSpan.TryParse(appSettings.SprinklerTime, out var t))
                SprinklerStartTime = t;

            SprinklerDuration = TimeSpan.FromMinutes(appSettings.SprinklerDurationMinutes);
            // Apply sensor values
            txtTemperature.Text = appSettings.TemperatureValue;
            txtHumidity.Text = appSettings.HumidityValue;
            txtSoil.Text = appSettings.SoilValue;

        }
        private void SaveAppSettings()
        {
            if (!IsAdmin())
                return;

            appSettings.SprinklerTime = txtSprinklerTime.Text;
            appSettings.SprinklerDurationMinutes =
                int.TryParse(txtSprinklerDuration.Text, out var m) ? m : 10;

            // SAVE SENSOR VALUES
            appSettings.TemperatureValue = txtTemperature.Text;
            appSettings.HumidityValue = txtHumidity.Text;
            appSettings.SoilValue = txtSoil.Text;

            try
            {
                var json = JsonSerializer.Serialize(appSettings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(SettingsFilePath, json);
            }
            catch { }
        }


        private void txtSprinklerTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTemperature_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTemperature_Leave(object sender, EventArgs e)
        {
            SaveAppSettings();
        }

        private void txtHumidity_Leave(object sender, EventArgs e)
        {
            SaveAppSettings();

        }

        private void txtSoil_Leave(object sender, EventArgs e)
        {
            SaveAppSettings();

        }
        private void MainPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If user closes window using X
            if (e.CloseReason == CloseReason.UserClosing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Form1)
                    {
                        form.Show();
                        form.BringToFront();
                        return;
                    }
                }

                // Safety fallback
                new Form1().Show();
            }
        }

        private void MainPage_Load(object sender, EventArgs e)
        {

        }

        private void MainPage_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            // If user closes MainPage using the X button
            if (e.CloseReason == CloseReason.UserClosing)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Form1)
                    {
                        form.Show();
                        form.BringToFront();
                        return;
                    }
                }

                // Safety fallback: recreate start screen
                new Form1().Show();
            }
        }

    }
}
