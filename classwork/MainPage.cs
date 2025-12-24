using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using RJCP.IO.Ports;
using System.IO.Ports;

namespace classwork
{
    public partial class MainPage : Form
    {
        private string role;

        // ================= TIMERS =================
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Timer espScanTimer;

        // ================= ESP =================
        private SerialPortStream espPort;

        // ================= ACTUATORS =================
        private class Actuator
        {
            public bool IsOn { get; set; }
        }

        private Actuator Fan = new();
        private Actuator Heater = new();
        private Actuator Pump = new();

        // ================= SENSORS =================
        private class SensorInfo
        {
            public bool IsOn { get; set; }
            public TimeSpan TotalOnTime { get; set; }
            public DateTime? StartTime { get; set; }
        }

        private class AlertEntry
        {
            public string Sensor { get; set; }
            public string Value { get; set; }
            public string Limit { get; set; }
            public string Time { get; set; }
        }

        private Dictionary<string, (double Min, double Max)> limits = new()
        {
            { "Temperature", (0, 50) },
            { "Humidity", (0, 100) },
            { "Soil", (0, 100) }
        };

        private static Dictionary<string, SensorInfo> sensors = new()
        {
            { "Temperature", new SensorInfo { IsOn = true, StartTime = DateTime.Now } },
            { "Humidity", new SensorInfo { IsOn = true, StartTime = DateTime.Now } },
            { "Soil", new SensorInfo { IsOn = true, StartTime = DateTime.Now } }
        };

        // ================= CONSTRUCTOR =================
        public MainPage(string username, string role)
        {
            InitializeComponent();
            this.role = role;

            labelUser.Text = $"Logged in as: {username}";
            this.FormClosed += (_, __) => Application.Exit();

            btnLogout.Click += BtnLogout_Click;

            bool isAdmin = role.ToLower() == "admin";
            txtTemperature.ReadOnly = !isAdmin;
            txtHumidity.ReadOnly = !isAdmin;
            txtSoil.ReadOnly = !isAdmin;

            btnTempToggle.Click += (_, __) => ToggleSensor("Temperature");
            btnHumidityToggle.Click += (_, __) => ToggleSensor("Humidity");
            btnSoilToggle.Click += (_, __) => ToggleSensor("Soil");

            updateTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

            InitEspConnection();
            UpdateAllToggleButtons();
            UpdateActuatorStatus();
        }
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // Stop timers cleanly
            updateTimer?.Stop();
            espScanTimer?.Stop();

            // Optional: close ESP connection
            try
            {
                espPort?.Close();
            }
            catch { }

            // Go back to login form
            var loginForm = new Form1();
            loginForm.Show();

            this.Hide();
        }


        // ================= TIMER =================
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateSensorLabel(labelTemperature, txtTemperature, "Temperature");
            UpdateSensorLabel(labelHumidity, txtHumidity, "Humidity");
            UpdateSensorLabel(labelSoil, txtSoil, "Soil");

            // 🔥 SYSTEM-LEVEL DECISION (ONCE PER CYCLE)
            EvaluateSystemLogic();
        }

        // ================= SENSOR UI =================
        private void ToggleSensor(string sensor)
        {
            var info = sensors[sensor];

            if (info.IsOn)
            {
                info.TotalOnTime += DateTime.Now - info.StartTime!.Value;
                info.StartTime = null;
                info.IsOn = false;
            }
            else
            {
                info.StartTime = DateTime.Now;
                info.IsOn = true;
            }

            UpdateAllToggleButtons();
        }

        private void UpdateSensorLabel(Label lbl, TextBox txt, string sensor)
        {
            var info = sensors[sensor];

            TimeSpan total = info.TotalOnTime;
            if (info.IsOn && info.StartTime.HasValue)
                total += DateTime.Now - info.StartTime.Value;

            string formatted = total.ToString(@"hh\:mm\:ss");

            lbl.Text = info.IsOn
                ? $"{sensor}: (ON for {formatted})"
                : $"{sensor}: (OFF, Total {formatted})";

            if (info.IsOn && double.TryParse(txt.Text.Trim(), out double value))
            {
                var (min, max) = limits[sensor];
                if (value < min) SaveAlert(sensor, value, $"Min {min}");
                if (value > max) SaveAlert(sensor, value, $"Max {max}");
            }
        }

        private void UpdateAllToggleButtons()
        {
            UpdateToggleButton(btnTempToggle, "Temperature");
            UpdateToggleButton(btnHumidityToggle, "Humidity");
            UpdateToggleButton(btnSoilToggle, "Soil");
        }

        private void UpdateToggleButton(Button btn, string sensor)
        {
            btn.Text = sensors[sensor].IsOn ? $"{sensor} ON" : $"{sensor} OFF";
            btn.BackColor = sensors[sensor].IsOn ? Color.LimeGreen : Color.Red;
            btn.Enabled = role.ToLower() == "admin";
        }

        // ================= SYSTEM LOGIC =================
        private bool TryGetSensorValue(string sensor, TextBox txt, out double value)
        {
            value = 0;
            return sensors[sensor].IsOn &&
                   double.TryParse(txt.Text.Trim(), out value);
        }

        private void EvaluateSystemLogic()
        {
            bool fanNeeded = false;

            // Temperature
            if (TryGetSensorValue("Temperature", txtTemperature, out double temp))
            {
                if (temp > 28) fanNeeded = true;

                if (temp < 10) TurnHeater(true);
                else if (temp > 15) TurnHeater(false);
            }

            // Humidity
            if (TryGetSensorValue("Humidity", txtHumidity, out double hum))
            {
                if (hum > 70) fanNeeded = true;
            }

            // Soil
            if (TryGetSensorValue("Soil", txtSoil, out double soil))
            {
                if (soil < 30) TurnPump(true);
                else if (soil > 50) TurnPump(false);
            }

            // Fan decided ONCE
            TurnFan(fanNeeded);
        }

        // ================= ACTUATORS =================
        private void TurnFan(bool on)
        {
            if (Fan.IsOn == on) return;
            Fan.IsOn = on;
            SendEspCommand(on ? "FAN_ON" : "FAN_OFF");
            UpdateActuatorStatus();
        }

        private void TurnHeater(bool on)
        {
            if (Heater.IsOn == on) return;
            Heater.IsOn = on;
            SendEspCommand(on ? "HEATER_ON" : "HEATER_OFF");
            UpdateActuatorStatus();
        }

        private void TurnPump(bool on)
        {
            if (Pump.IsOn == on) return;
            Pump.IsOn = on;
            SendEspCommand(on ? "PUMP_ON" : "PUMP_OFF");
            UpdateActuatorStatus();
        }

        private void UpdateActuatorStatus()
        {
            lblFanStatus.Text = Fan.IsOn ? "Fan: ON" : "Fan: OFF";
            lblFanStatus.ForeColor = Fan.IsOn ? Color.LimeGreen : Color.Red;

            lblHeaterStatus.Text = Heater.IsOn ? "Heater: ON" : "Heater: OFF";
            lblHeaterStatus.ForeColor = Heater.IsOn ? Color.LimeGreen : Color.Red;

            lblPumpStatus.Text = Pump.IsOn ? "Pump: ON" : "Pump: OFF";
            lblPumpStatus.ForeColor = Pump.IsOn ? Color.LimeGreen : Color.Red;
        }

        // ================= ALERT LOGGING =================
        private void SaveAlert(string sensor, double value, string limit)
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alerts");
            Directory.CreateDirectory(dir);

            string file = Path.Combine(dir, $"{sensor}.json");
            List<AlertEntry> logs = File.Exists(file)
                ? JsonSerializer.Deserialize<List<AlertEntry>>(File.ReadAllText(file)) ?? new()
                : new();

            logs.Add(new AlertEntry
            {
                Sensor = sensor,
                Value = value.ToString(),
                Limit = limit,
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            });

            File.WriteAllText(
                file,
                JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true })
            );
        }

        // ================= ESP =================
        private void InitEspConnection()
        {
            espScanTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            espScanTimer.Tick += (_, __) => TryConnectToEsp();
            espScanTimer.Start();

            TryConnectToEsp();
        }

        private void TryConnectToEsp()
        {
            if (espPort != null && espPort.IsOpen) return;

            foreach (string port in SerialPort.GetPortNames())
            {
                try
                {
                    espPort = new SerialPortStream(port, 115200);
                    espPort.Open();
                    lblEspStatus.Text = $"ESP: Connected ({port})";
                    return;
                }
                catch { }
            }

            lblEspStatus.Text = "ESP: Disconnected";
        }

        private void SendEspCommand(string cmd)
        {
            if (espPort == null || !espPort.IsOpen) return;
            espPort.WriteLine(cmd);
            AppendEspLog($"> {cmd}");
        }

        private void AppendEspLog(string text)
        {
            if (txtEspLog.InvokeRequired)
            {
                txtEspLog.Invoke(new Action<string>(AppendEspLog), text);
                return;
            }

            txtEspLog.AppendText(text + Environment.NewLine);
        }

        private void btnEspSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEspCommand.Text))
                SendEspCommand(txtEspCommand.Text.Trim());
        }
    }
}
