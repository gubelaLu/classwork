using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace classwork
{
    public partial class MainPage : Form
    {
        private string username;
        private string role;

        // ================= TIMER =================
        private System.Windows.Forms.Timer updateTimer;

        // ================= RULES FILE =================
        private const string RulesFilePath =
            @"C:\Users\Administrator\Source\Repos\classworkL\classwork\controlRules.json";

        private ControlRules rules;

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

        // ================= RULE MODELS (match your JSON) =================
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

        // ================= CONSTRUCTORS =================
        public MainPage()
        {
            InitializeComponent();
            InitializeLogic();
        }

        public MainPage(string username, string role)
        {
            this.username = username;
            this.role = role;

            InitializeComponent();
            InitializeLogic();
        }

        // ================= INIT =================
        private void InitializeLogic()
        {
            labelUser.Text = !string.IsNullOrEmpty(username)
                ? $"User: {username} ({role})"
                : "User: Unknown";

            // Load rules from JSON (this is what you asked for)
            rules = LoadRulesFromJson();

            // Sensor toggles
            btnTempToggle.Click += (s, e) => ToggleSensor(TemperatureSensor);
            btnHumidityToggle.Click += (s, e) => ToggleSensor(HumiditySensor);
            btnSoilToggle.Click += (s, e) => ToggleSensor(SoilSensor);

            // Logout wiring (Designer does not wire it)
            btnLogout.Click += btnLogout_Click;

            // Sensors ON from start (as you requested previously)
            TurnSensorOnAtStartup(TemperatureSensor);
            TurnSensorOnAtStartup(HumiditySensor);
            TurnSensorOnAtStartup(SoilSensor);

            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 1000;
            updateTimer.Tick += UpdateUi;
            updateTimer.Start();

            // Initial UI render
            UpdateSensorUI(TemperatureSensor, btnTempToggle, "Temperature", txtTemperature);
            UpdateSensorUI(HumiditySensor, btnHumidityToggle, "Humidity", txtHumidity);
            UpdateSensorUI(SoilSensor, btnSoilToggle, "Soil", txtSoil);

            UpdateActuatorUI(Fan, lblFanStatus, lblFanTime, "Fan");
            UpdateActuatorUI(Heater, lblHeaterStatus, lblHeaterTime, "Heater");
            UpdateActuatorUI(Pump, lblPumpStatus, lblPumpTime, "Pump");
        }

        private ControlRules LoadRulesFromJson()
        {
            // Defaults used only if JSON missing/unreadable
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
                string path = RulesFilePath;

                // Optional convenience: if absolute path missing, try local file
                if (!File.Exists(path))
                {
                    var local = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "controlRules.json");
                    if (File.Exists(local))
                        path = local;
                }

                if (!File.Exists(path))
                {
                    txtEspLog?.AppendText($"[Rules] File not found. Using defaults. Expected: {RulesFilePath}{Environment.NewLine}");
                    return fallback;
                }

                var json = File.ReadAllText(path);
                var loaded = JsonSerializer.Deserialize<ControlRules>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (loaded == null)
                {
                    txtEspLog?.AppendText($"[Rules] Failed to parse JSON. Using defaults.{Environment.NewLine}");
                    return fallback;
                }

                txtEspLog?.AppendText($"[Rules] Loaded from: {path}{Environment.NewLine}");
                return loaded;
            }
            catch (Exception ex)
            {
                txtEspLog?.AppendText($"[Rules] Error loading JSON. Using defaults. {ex.Message}{Environment.NewLine}");
                return fallback;
            }
        }

        private static void TurnSensorOnAtStartup(SensorInfo sensor)
        {
            sensor.IsOn = true;
            sensor.StartTime = DateTime.Now;
        }

        // ================= SENSOR LOGIC =================
        private void ToggleSensor(SensorInfo sensor)
        {
            if (!sensor.IsOn)
            {
                sensor.IsOn = true;
                sensor.StartTime = DateTime.Now;
            }
            else
            {
                sensor.IsOn = false;
                if (sensor.StartTime.HasValue)
                    sensor.TotalOnTime += DateTime.Now - sensor.StartTime.Value;

                sensor.StartTime = null;
            }
        }

        private void UpdateUi(object sender, EventArgs e)
        {
            UpdateSensorUI(TemperatureSensor, btnTempToggle, "Temperature", txtTemperature);
            UpdateSensorUI(HumiditySensor, btnHumidityToggle, "Humidity", txtHumidity);
            UpdateSensorUI(SoilSensor, btnSoilToggle, "Soil", txtSoil);

            // Actuator automation from JSON rules
            ApplyAutomationRulesFromJson();

            UpdateActuatorUI(Fan, lblFanStatus, lblFanTime, "Fan");
            UpdateActuatorUI(Heater, lblHeaterStatus, lblHeaterTime, "Heater");
            UpdateActuatorUI(Pump, lblPumpStatus, lblPumpTime, "Pump");
        }

        private void UpdateSensorUI(SensorInfo sensor, Button button, string name, TextBox valueBox)
        {
            var elapsed = sensor.TotalOnTime;

            if (sensor.IsOn && sensor.StartTime.HasValue)
                elapsed += DateTime.Now - sensor.StartTime.Value;

            if (sensor.IsOn)
            {
                button.Text = $"{name}: ON ({elapsed:hh\\:mm\\:ss})";
                button.ForeColor = Color.Green;
                valueBox.Enabled = true;
            }
            else
            {
                button.Text = $"{name}: OFF ({elapsed:hh\\:mm\\:ss})";
                button.ForeColor = Color.Red;
                valueBox.Enabled = false;
            }
        }

        // ================= AUTOMATION (JSON RULES + HYSTERESIS) =================
        private void ApplyAutomationRulesFromJson()
        {
            // Only use a value if that sensor is ON and numeric.
            double? temp = TemperatureSensor.IsOn ? TryParseDouble(txtTemperature.Text) : null;
            double? hum = HumiditySensor.IsOn ? TryParseDouble(txtHumidity.Text) : null;
            double? soil = SoilSensor.IsOn ? TryParseDouble(txtSoil.Text) : null;

            // HEATER (depends on temperature)
            bool heaterDesired = false;
            if (temp.HasValue)
            {
                if (!Heater.IsOn)
                {
                    heaterDesired = temp.Value <= rules.Heater.OnBelow;
                }
                else
                {
                    heaterDesired = temp.Value < rules.Heater.OffAbove; // stays ON until reaching OffAbove
                }
            }
            // if temp missing -> heaterDesired stays false

            // PUMP (depends on soil)
            bool pumpDesired = false;
            if (soil.HasValue)
            {
                if (!Pump.IsOn)
                {
                    pumpDesired = soil.Value <= rules.Pump.OnBelowSoil;
                }
                else
                {
                    pumpDesired = soil.Value < rules.Pump.OffAboveSoil; // stays ON until reaching OffAboveSoil
                }
            }

            // FAN (depends on temp and/or humidity)
            bool fanDesired = false;

            // If both missing, fan stays OFF
            if (temp.HasValue || hum.HasValue)
            {
                if (!Fan.IsOn)
                {
                    // Turn ON if either exceeds its "OnAbove" threshold
                    fanDesired =
                        (temp.HasValue && temp.Value >= rules.Fan.OnAboveTemp) ||
                        (hum.HasValue && hum.Value >= rules.Fan.OnAboveHumidity);
                }
                else
                {
                    // Keep ON if either is still above its "OffBelow" threshold
                    bool keepByTemp = temp.HasValue && temp.Value > rules.Fan.OffBelowTemp;
                    bool keepByHum = hum.HasValue && hum.Value > rules.Fan.OffBelowHumidity;

                    fanDesired = keepByTemp || keepByHum;
                }
            }

            SetActuatorState(Heater, heaterDesired);
            SetActuatorState(Pump, pumpDesired);
            SetActuatorState(Fan, fanDesired);
        }

        private static double? TryParseDouble(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            text = text.Trim().Replace(',', '.');

            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var v))
                return v;

            return null;
        }

        private void SetActuatorState(Actuator actuator, bool on)
        {
            if (on && !actuator.IsOn)
            {
                actuator.IsOn = true;
                actuator.StartTime = DateTime.Now;
                return;
            }

            if (!on && actuator.IsOn)
            {
                actuator.IsOn = false;
                if (actuator.StartTime.HasValue)
                    actuator.TotalOnTime += DateTime.Now - actuator.StartTime.Value;
                actuator.StartTime = null;
            }
        }

        private void UpdateActuatorUI(Actuator actuator, Label statusLabel, Label timeLabel, string name)
        {
            var elapsed = actuator.TotalOnTime;

            if (actuator.IsOn && actuator.StartTime.HasValue)
                elapsed += DateTime.Now - actuator.StartTime.Value;

            statusLabel.Text = actuator.IsOn ? $"{name}: ON" : $"{name}: OFF";
            statusLabel.ForeColor = actuator.IsOn ? Color.Green : Color.Red;

            timeLabel.Text = elapsed.ToString(@"hh\:mm\:ss");
        }

        // ================= HELP (if your Designer has the ? button wired) =================
        private void btnHelp_Click(object sender, EventArgs e)
        {
            // Uses current loaded rules
            string msg =
                $"Rules source:\n{RulesFilePath}\n\n" +
                "Heater:\n" +
                $"  ON if Temp <= {rules.Heater.OnBelow}\n" +
                $"  OFF if Temp >= {rules.Heater.OffAbove}\n\n" +
                "Fan:\n" +
                $"  ON if Temp >= {rules.Fan.OnAboveTemp} OR Humidity >= {rules.Fan.OnAboveHumidity}\n" +
                $"  OFF only when Temp <= {rules.Fan.OffBelowTemp} AND Humidity <= {rules.Fan.OffBelowHumidity}\n\n" +
                "Pump:\n" +
                $"  ON if Soil <= {rules.Pump.OnBelowSoil}\n" +
                $"  OFF if Soil >= {rules.Pump.OffAboveSoil}\n\n" +
                "Notes:\n" +
                "• A rule is applied only if that sensor is ON and its value is numeric.\n" +
                "• Hysteresis is used to avoid constant ON/OFF toggling.";

            MessageBox.Show(msg, "Help: Control Rules", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ================= LOGOUT =================
        private void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f.GetType().Name == "LoginPage")
                    {
                        f.Show();
                        f.BringToFront();
                        Close();
                        return;
                    }
                }

                var loginType = Type.GetType("classwork.LoginPage");
                if (loginType != null && typeof(Form).IsAssignableFrom(loginType))
                {
                    var login = (Form)Activator.CreateInstance(loginType);
                    login.Show();
                    Close();
                    return;
                }
            }
            catch
            {
                // fallback below
            }

            Close();
        }

        // ===== REQUIRED because Designer wires this event =====
        private void btnEspSend_Click(object sender, EventArgs e)
        {
            var cmd = txtEspCommand.Text?.Trim();
            if (string.IsNullOrWhiteSpace(cmd))
                return;

            txtEspLog.AppendText($"> {cmd}{Environment.NewLine}");
            txtEspCommand.Clear();
        }
    }
}
