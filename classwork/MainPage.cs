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
        // Path to JSON file that stores UI/app settings (sprinkler, sensor values)
        private const string SettingsFilePath =
            @"C:\Users\Administrator\Source\Repos\classworkL\classwork\appSettings.json";
        // Path to login activity log file
        private const string LoginLogFilePath =
            @"C:\Users\Administrator\Source\Repos\classworkL\classwork\login_log.json";

        // Object holding deserialized application settings
        private AppSettings appSettings = new();

        // Logged-in user data (passed from login form)
        private string username;
        private string role;

        // Timer used to refresh UI and automation logic every second
        private System.Windows.Forms.Timer updateTimer;

        // Serial connection to ESP device (ESP32 / ESP8266)
        private SerialPort espSerial;

        // Path to JSON file that contains automation rules
        private const string RulesFilePath =
            @"C:\Users\Administrator\Source\Repos\classworkL\classwork\controlRules.json";

        // Object holding deserialized automation rules
        private ControlRules rules;

        // ================= SPRINKLER (MODIFIABLE) =================

        // Time of day when sprinkler should start
        private TimeSpan SprinklerStartTime = new TimeSpan(12, 0, 0);

        // How long the sprinkler should run
        private TimeSpan SprinklerDuration = TimeSpan.FromMinutes(10);

        // Current sprinkler runtime state
        private bool sprinklerActive;
        private DateTime? sprinklerEndTime;

        // Used to ensure sprinkler runs only once per day
        private DateTime lastSprinklerRunDate = DateTime.MinValue;

        // ================= SENSORS =================

        // Generic sensor state container
        private class SensorInfo
        {
            public bool IsOn { get; set; }              // Whether sensor is enabled
            public DateTime? StartTime { get; set; }    // When sensor was turned on
            public TimeSpan TotalOnTime { get; set; }   // Accumulated uptime
        }

        // Individual sensors
        private readonly SensorInfo TemperatureSensor = new();
        private readonly SensorInfo HumiditySensor = new();
        private readonly SensorInfo SoilSensor = new();

        // ================= ACTUATORS =================

        // Generic actuator state container
        private class Actuator
        {
            public bool IsOn { get; set; }              // Current state
            public DateTime? StartTime { get; set; }    // When actuator was enabled
            public TimeSpan TotalOnTime { get; set; }   // Accumulated runtime
        }

        // Physical actuators controlled by automation rules
        private readonly Actuator Fan = new();
        private readonly Actuator Heater = new();
        private readonly Actuator Pump = new();

        // ================= RULE CLASSES =================

        // Root automation rules object
        private class ControlRules
        {
            public HeaterRules Heater { get; set; } = new();
            public FanRules Fan { get; set; } = new();
            public PumpRules Pump { get; set; } = new();
        }

        // Heater threshold rules (hysteresis)
        private class HeaterRules
        {
            public double OnBelow { get; set; }     // Turn heater ON below this temp
            public double OffAbove { get; set; }    // Turn heater OFF above this temp
        }

        // Fan threshold rules (temperature + humidity)
        private class FanRules
        {
            public double OnAboveTemp { get; set; }
            public double OffBelowTemp { get; set; }
            public double OnAboveHumidity { get; set; }
            public double OffBelowHumidity { get; set; }
        }

        // Pump threshold rules (soil moisture)
        private class PumpRules
        {
            public double OnBelowSoil { get; set; }
            public double OffAboveSoil { get; set; }
        }

        // Default constructor (no user info)
        public MainPage()
        {
            InitializeComponent();
            InitializeLogic();
            // Force UI refresh once form is visible
            this.Shown += (_, __) => UpdateAllUI();
        }

        // Constructor with authenticated user
        public MainPage(string username, string role)
        {
            this.username = username;
            this.role = role;
            InitializeComponent();
            InitializeLogic();
            this.Shown += (_, __) => UpdateAllUI();
        }

        // Initializes core application logic
        private void InitializeLogic()
        {
            // Display logged-in user information
            labelUser.Text = !string.IsNullOrEmpty(username)
                ? $"User: {username} ({role})"
                : "User: Unknown";

            // Load automation thresholds from JSON file
            rules = LoadRulesFromJson();

            // 1️⃣ Button click wiring (admin-only sensor toggles)
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

            // Load persisted UI settings
            LoadAppSettings();

            // Logout button handler
            btnLogout.Click += btnLogout_Click;

            // 👉 Apply role-based UI permissions
            ApplyPermissions();

            // 2️⃣ Enable all sensors at startup
            TurnSensorOnAtStartup(TemperatureSensor);
            TurnSensorOnAtStartup(HumiditySensor);
            TurnSensorOnAtStartup(SoilSensor);

            // 3️⃣ Start UI refresh timer (1 second interval)
            updateTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            updateTimer.Tick += (_, __) => UpdateAllUI();
            updateTimer.Start();

            // 4️⃣ Initialize ESP serial communication
            InitializeEspConnection();
        }
        private void InitializeEspConnection()
        {
            lblEspStatus.Text = "ESP: Connecting...";
            lblEspStatus.ForeColor = Color.Orange;

            foreach (string portName in SerialPort.GetPortNames())
            {
                SerialPort port = null;

                try
                {
                    port = new SerialPort(portName, 115200)
                    {
                        NewLine = "\n",
                        ReadTimeout = 500,
                        WriteTimeout = 500
                    };

                    port.Open();

                    // SUCCESS → lock this port
                    espSerial = port;
                    espSerial.DataReceived += EspSerial_DataReceived;

                    lblEspStatus.Text = $"ESP: Connected ({portName})";
                    lblEspStatus.ForeColor = Color.Green;
                    return;
                }
                catch
                {
                    port?.Dispose(); // prevent port leaks
                }
            }

            lblEspStatus.Text = "ESP: Disconnected";
            lblEspStatus.ForeColor = Color.Red;
        }
        // ================= UI UPDATE =================

        // Central update loop: UI + automation logic
        private void UpdateAllUI()
        {
            // Read sprinkler settings from UI
            UpdateSprinklerFromUI();

            // Update sensor UI elements
            UpdateSensorUI(TemperatureSensor, btnTempToggle, "Temperature", txtTemperature);
            UpdateSensorUI(HumiditySensor, btnHumidityToggle, "Humidity", txtHumidity);
            UpdateSensorUI(SoilSensor, btnSoilToggle, "Soil", txtSoil);

            // Apply automation rules to actuators
            ApplyAutomationRules();

            // Update actuator status UI
            UpdateActuatorUI(Fan, lblFanStatus, lblFanTime, "Fan");
            UpdateActuatorUI(Heater, lblHeaterStatus, lblHeaterTime, "Heater");
            UpdateActuatorUI(Pump, lblPumpStatus, lblPumpTime, "Pump");

            // Display sprinkler summary
            lblSprinklerInfo.Text =
                $"Sprinkler: {SprinklerStartTime:hh\\:mm} for {SprinklerDuration.TotalMinutes} min";
        }

        // ================= SPRINKLER UI PARSING =================

        // Reads sprinkler time and duration from textboxes
        private void UpdateSprinklerFromUI()
        {
            if (TimeSpan.TryParseExact(txtSprinklerTime.Text, @"hh\:mm", null, out var time))
                SprinklerStartTime = time;

            if (int.TryParse(txtSprinklerDuration.Text, out var minutes) && minutes > 0)
                SprinklerDuration = TimeSpan.FromMinutes(minutes);
        }

        // Handles timed sprinkler execution logic
        private bool ApplyTimedSprinkler()
        {
            var now = DateTime.Now;

            // Stop sprinkler when duration ends
            if (sprinklerActive && sprinklerEndTime.HasValue && now >= sprinklerEndTime.Value)
            {
                sprinklerActive = false;
                sprinklerEndTime = null;
            }

            // Start sprinkler once per day at configured time
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
        // Applies automation logic based on sensor values and configured rules
        private void ApplyAutomationRules()
        {
            // Safely parse sensor input values from textboxes
            double? temp = TryParseDouble(txtTemperature.Text);
            double? hum = TryParseDouble(txtHumidity.Text);
            double? soil = TryParseDouble(txtSoil.Text);

            // Check whether timed sprinkler should currently be active
            bool sprinkler = ApplyTimedSprinkler();

            // Heater control logic with hysteresis:
            // - Turn ON if temperature goes below OnBelow
            // - Turn OFF only after it rises above OffAbove
            SetActuatorState(Heater,
                temp.HasValue &&
                (!Heater.IsOn ? temp <= rules.Heater.OnBelow : temp < rules.Heater.OffAbove));

            // Fan control logic:
            // - Turns ON if temperature OR humidity exceeds upper thresholds
            // - Turns OFF only when both drop below lower thresholds
            SetActuatorState(Fan,
                !Fan.IsOn
                    ? (temp >= rules.Fan.OnAboveTemp || hum >= rules.Fan.OnAboveHumidity)
                    : (temp > rules.Fan.OffBelowTemp || hum > rules.Fan.OffBelowHumidity));

            // Pump control logic:
            // - Runs if sprinkler schedule is active
            // - OR if soil moisture is below threshold (with hysteresis)
            SetActuatorState(Pump,
                sprinkler ||
                (soil.HasValue &&
                 (!Pump.IsOn ? soil <= rules.Pump.OnBelowSoil : soil < rules.Pump.OffAboveSoil)));
        }

        // ================= HELP =================
        // Displays basic information about system behavior and sensors
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
        // Tries to parse a double value regardless of comma/dot decimal separator
        private static double? TryParseDouble(string t)
        {
            if (string.IsNullOrWhiteSpace(t)) return null;
            t = t.Replace(',', '.');
            return double.TryParse(t, NumberStyles.Float, CultureInfo.InvariantCulture, out var v) ? v : null;
        }

        // Forces a sensor to start in ON state and initializes its start time
        private static void TurnSensorOnAtStartup(SensorInfo s)
        {
            s.IsOn = true;
            s.StartTime = DateTime.Now;
        }

        // Checks whether the current user has administrator privileges
        private bool IsAdmin()
        {
            return string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        // Toggles sensor ON/OFF state and updates runtime statistics
        private void ToggleSensor(SensorInfo s)
        {
            if (s.IsOn)
            {
                // Turning sensor OFF: accumulate runtime
                s.IsOn = false;
                s.TotalOnTime += DateTime.Now - s.StartTime.Value;
                s.StartTime = null;
            }
            else
            {
                // Turning sensor ON: record start time
                s.IsOn = true;
                s.StartTime = DateTime.Now;
            }
        }

        // Updates actuator state and tracks how long it has been running
        private void SetActuatorState(Actuator a, bool on)
        {
            if (on && !a.IsOn)
            {
                // Actuator just turned ON
                a.IsOn = true;
                a.StartTime = DateTime.Now;
            }
            else if (!on && a.IsOn)
            {
                // Actuator just turned OFF
                a.IsOn = false;
                a.TotalOnTime += DateTime.Now - a.StartTime.Value;
                a.StartTime = null;
            }
        }

        // Shows warning message when non-admin tries to modify restricted controls
        private void ShowPermissionDenied()
        {
            MessageBox.Show(
                "You do not have permission to change sensor states.\n\n" +
                "Only administrators can enable or disable sensors.",
                "Permission Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        // Updates sensor button text, color, and textbox state
        private void UpdateSensorUI(SensorInfo s, Button b, string name, TextBox box)
        {
            // Calculate total elapsed ON time
            var elapsed = s.TotalOnTime +
                (s.IsOn && s.StartTime.HasValue ? DateTime.Now - s.StartTime.Value : TimeSpan.Zero);

            // Display sensor value or N/A if empty
            var value = string.IsNullOrWhiteSpace(box.Text) ? "N/A" : box.Text;

            // Update button text and color
            b.Text = $"{name}: {(s.IsOn ? "ON" : "OFF")} | {value} | {elapsed:hh\\:mm\\:ss}";
            b.ForeColor = s.IsOn ? Color.Green : Color.Red;

            // Disable textbox when sensor is OFF
            box.Enabled = s.IsOn;
        }
        // ================= LOGIN LOGS =================
        // Displays contents of login_log.json in a scrollable dialog
        private void btnLogs_Click(object sender, EventArgs e)
        {
            if (!File.Exists(LoginLogFilePath))
            {
                MessageBox.Show(
                    "Login log file not found.",
                    "Logs",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var json = File.ReadAllText(LoginLogFilePath);

                // Simple log viewer using a read-only multiline textbox
                Form logForm = new Form
                {
                    Text = "Login Logs",
                    Width = 700,
                    Height = 500,
                    StartPosition = FormStartPosition.CenterParent
                };

                TextBox txtLogs = new TextBox
                {
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 10),
                    Text = json
                };

                logForm.Controls.Add(txtLogs);
                logForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load logs:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // Updates actuator status labels and runtime display
        private void UpdateActuatorUI(Actuator a, Label s, Label t, string name)
        {
            // Calculate total actuator runtime
            var elapsed = a.TotalOnTime +
                (a.IsOn && a.StartTime.HasValue ? DateTime.Now - a.StartTime.Value : TimeSpan.Zero);

            // Update status label
            s.Text = $"{name}: {(a.IsOn ? "ON" : "OFF")}";
            s.ForeColor = a.IsOn ? Color.Green : Color.Red;

            // Update elapsed time label
            t.Text = elapsed.ToString(@"hh\:mm\:ss");
        }

        // Handles logout action and returns user to start/login form
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


        // ================= ROLE-BASED PERMISSIONS =================
        // Enables or disables UI controls depending on user role
        private void ApplyPermissions()
        {
            // Determine whether current user is an administrator
            bool isAdmin = IsAdmin();

            // Only admins can toggle sensors
            btnTempToggle.Enabled = isAdmin;
            btnHumidityToggle.Enabled = isAdmin;
            btnSoilToggle.Enabled = isAdmin;

            // Visual hint for non-admin users
            if (!isAdmin)
            {
                btnTempToggle.Text += " (Admin only)";
                btnHumidityToggle.Text += " (Admin only)";
                btnSoilToggle.Text += " (Admin only)";
            }
        }
        private ControlRules LoadRulesFromJson()
        {
            var fallback = new ControlRules
            {
                Heater = new HeaterRules { OnBelow = 15, OffAbove = 30 },
                Fan = new FanRules
                {
                    OnAboveTemp = 28,
                    OffBelowTemp = 25,
                    OnAboveHumidity = 75,
                    OffBelowHumidity = 65
                },
                Pump = new PumpRules { OnBelowSoil = 30, OffAboveSoil = 45 }
            };

            try
            {
                if (!File.Exists(RulesFilePath))
                    return fallback;

                var json = File.ReadAllText(RulesFilePath);
                return JsonSerializer.Deserialize<ControlRules>(json) ?? fallback;
            }
            catch
            {
                return fallback;
            }
        }
        // ================= ESP COMMAND SENDER =================
        // Sends a raw command string to the connected ESP device
        private void btnEspSend_Click(object sender, EventArgs e)
        {
            // Read and trim command input
            var cmd = txtEspCommand.Text?.Trim();

            // Do nothing if input is empty
            if (string.IsNullOrWhiteSpace(cmd))
                return;

            // Check if ESP serial connection is available
            if (espSerial == null || !espSerial.IsOpen)
            {
                txtEspLog.AppendText("[ESP] Not connected\n");
                return;
            }

            // Send command to ESP and log it in UI
            espSerial.WriteLine(cmd);
            txtEspLog.AppendText($"> {cmd}{Environment.NewLine}");
            txtEspCommand.Clear();
        }
        private void EspSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string line = espSerial.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                // Example expected format:
                // TEMP:23.6;HUM:45.1;SOIL:612

                double? temp = null;
                double? hum = null;
                double? soil = null;

                foreach (var part in line.Split(';'))
                {
                    var kv = part.Split(':');
                    if (kv.Length != 2)
                        continue;

                    var key = kv[0].Trim().ToUpperInvariant();
                    var value = kv[1].Trim().Replace(',', '.');

                    if (!double.TryParse(value, NumberStyles.Float,
                        CultureInfo.InvariantCulture, out var v))
                        continue;

                    switch (key)
                    {
                        case "TEMP":
                            temp = v;
                            break;
                        case "HUM":
                            hum = v;
                            break;
                        case "SOIL":
                            soil = v;
                            break;
                    }
                }

                // Switch to UI thread
                BeginInvoke(new Action(() =>
                {
                    if (temp.HasValue && TemperatureSensor.IsOn)
                        txtTemperature.Text = temp.Value.ToString("F1");

                    if (hum.HasValue && HumiditySensor.IsOn)
                        txtHumidity.Text = hum.Value.ToString("F1");

                    if (soil.HasValue && SoilSensor.IsOn)
                        txtSoil.Text = soil.Value.ToString("F0");
                }));
            }
            catch
            {
                // Ignore malformed serial data
            }
        }
        // ================= INPUT VALIDATION =================
        // Allows only numeric input for sprinkler duration
        private void txtSprinklerDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits and control keys (e.g., Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // Duplicate handler (same validation logic, designer-linked)
        private void txtSprinklerDuration_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // Allows only digits and ':' character for HH:mm time format
        private void txtSprinklerTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != ':')
            {
                e.Handled = true;
            }
        }

        // Validates sprinkler time when user leaves the textbox
        private void txtSprinklerTime_Leave(object sender, EventArgs e)
        {
            var text = txtSprinklerTime.Text;

            // Expect exactly HH:mm format
            var parts = text.Split(':');
            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out int hour) ||
                !int.TryParse(parts[1], out int minute))
            {
                ShowInvalidTime();
                return;
            }

            // Validate time ranges
            if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
            {
                ShowInvalidTime();
                return;
            }

            // Normalize format (e.g., 9:5 -> 09:05)
            txtSprinklerTime.Text = $"{hour:D2}:{minute:D2}";

            // Persist valid settings
            SaveAppSettings();
        }

        // Displays warning message and restores last valid sprinkler time
        private void ShowInvalidTime()
        {
            MessageBox.Show(
                "Please enter a valid time in HH:mm format.\n\n" +
                "- Hours: 0 to 23\n" +
                "- Minutes: 0 to 59",
                "Invalid Sprinkler Time",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            // Restore previously valid value
            txtSprinklerTime.Text = SprinklerStartTime.ToString(@"hh\:mm");
            txtSprinklerTime.Focus();
        }

        // ================= APPLICATION SETTINGS MODEL =================
        // Represents persistent UI state stored in JSON
        private class AppSettings
        {
            public string SprinklerTime { get; set; } = "12:00";          // Stored sprinkler start time
            public int SprinklerDurationMinutes { get; set; } = 10;      // Stored sprinkler duration

            public string TemperatureValue { get; set; } = "";           // Last temperature input
            public string HumidityValue { get; set; } = "";              // Last humidity input
            public string SoilValue { get; set; } = "";                  // Last soil moisture input
        }


        // ================= SETTINGS LOADING =================
        // Loads persisted application settings from JSON file
        private void LoadAppSettings()
        {
            try
            {
                // If settings file does not exist, keep defaults
                if (!File.Exists(SettingsFilePath))
                    return;

                // Read JSON and deserialize into AppSettings object
                var json = File.ReadAllText(SettingsFilePath);
                appSettings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch
            {
                // In case of corrupted file or read error, reset to defaults
                appSettings = new AppSettings();
            }

            // Apply loaded values to UI and internal logic
            txtSprinklerTime.Text = appSettings.SprinklerTime;
            txtSprinklerDuration.Text = appSettings.SprinklerDurationMinutes.ToString();

            // Parse sprinkler start time for logic usage
            if (TimeSpan.TryParse(appSettings.SprinklerTime, out var t))
                SprinklerStartTime = t;

            // Apply sprinkler duration
            SprinklerDuration = TimeSpan.FromMinutes(appSettings.SprinklerDurationMinutes);

            // Restore last entered sensor values
            txtTemperature.Text = appSettings.TemperatureValue;
            txtHumidity.Text = appSettings.HumidityValue;
            txtSoil.Text = appSettings.SoilValue;
        }

        // ================= SETTINGS SAVING =================
        // Saves current UI state into JSON file (admin-only)
        private void SaveAppSettings()
        {
            // Prevent non-admin users from saving settings
            if (!IsAdmin())
                return;

            // Store current UI values into settings object
            appSettings.SprinklerTime = txtSprinklerTime.Text;
            appSettings.SprinklerDurationMinutes =
                int.TryParse(txtSprinklerDuration.Text, out var m) ? m : 10;

            // Save sensor textbox values
            appSettings.TemperatureValue = txtTemperature.Text;
            appSettings.HumidityValue = txtHumidity.Text;
            appSettings.SoilValue = txtSoil.Text;

            try
            {
                // Serialize settings with readable formatting
                var json = JsonSerializer.Serialize(appSettings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(SettingsFilePath, json);
            }
            catch
            {
                // Silent failure to avoid crashing the application
            }
        }

        // ================= TEXTBOX EVENT HANDLERS =================
        // Required for WinForms designer event binding

        private void txtSprinklerTime_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtTemperature_TextChanged(object sender, EventArgs e)
        {
        }

        // Save settings when temperature field loses focus
        private void txtTemperature_Leave(object sender, EventArgs e)
        {
            SaveAppSettings();
        }

        // Save settings when humidity field loses focus
        private void txtHumidity_Leave(object sender, EventArgs e)
        {
            SaveAppSettings();
        }

        // Save settings when soil moisture field loses focus
        private void txtSoil_Leave(object sender, EventArgs e)
        {
            SaveAppSettings();
        }

        // ================= FORM LIFECYCLE =================
        // Handles closing the form via the X button
        private void MainPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If user manually closes the window
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Try to bring back the start/login form
                foreach (Form form in Application.OpenForms)
                {
                    if (form is Form1)
                    {
                        form.Show();
                        form.BringToFront();
                        return;
                    }
                }

                // Safety fallback if Form1 is not open
                new Form1().Show();
            }
        }

        private void MainPage_Load(object sender, EventArgs e)
        {
            // Reserved for future initialization logic
        }

        // Duplicate FormClosing handler (designer-linked)
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

