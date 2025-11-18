using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.IO;
using System.Windows.Forms;

namespace classwork
{
    public partial class MainPage : Form
    {
        private string role;
        private System.Windows.Forms.Timer updateTimer;

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

        private Dictionary<string, (double Min, double Max)> limits =
            new Dictionary<string, (double Min, double Max)>()
        {
            { "Temperature", (0, 50) },
            { "Humidity", (0, 100) },
            { "PH", (0, 14) },
            { "Light", (0, 200000) },
            { "Soil", (0, 100) }
        };

        private static Dictionary<string, SensorInfo> sensors = new Dictionary<string, SensorInfo>()
        {
            { "Temperature", new SensorInfo { IsOn = true, TotalOnTime = TimeSpan.Zero, StartTime = DateTime.Now } },
            { "Humidity", new SensorInfo { IsOn = true, TotalOnTime = TimeSpan.Zero, StartTime = DateTime.Now } },
            { "PH", new SensorInfo { IsOn = true, TotalOnTime = TimeSpan.Zero, StartTime = DateTime.Now } },
            { "Light", new SensorInfo { IsOn = true, TotalOnTime = TimeSpan.Zero, StartTime = DateTime.Now } },
            { "Soil", new SensorInfo { IsOn = true, TotalOnTime = TimeSpan.Zero, StartTime = DateTime.Now } }
        };

        public MainPage(string username, string role)
        {
            InitializeComponent();
            this.role = role;
            this.FormClosed += (s, e) => Application.Exit();

            labelUser.Text = "Logged in as: " + username;

            labelTemperature.Text = "Temperature:";
            labelHumidity.Text = "Humidity:";
            labelPH.Text = "PH:";
            labelLight.Text = "Light:";
            labelSoil.Text = "Soil:";

            bool isAdmin = role.ToLower() == "admin";

            txtTemperature.ReadOnly = !isAdmin;
            txtHumidity.ReadOnly = !isAdmin;
            txtPH.ReadOnly = !isAdmin;
            txtLight.ReadOnly = !isAdmin;
            txtSoil.ReadOnly = !isAdmin;

            UpdateAllToggleButtons();

            btnTempToggle.Click += (s, e) => ToggleSensor("Temperature", btnTempToggle);
            btnHumidityToggle.Click += (s, e) => ToggleSensor("Humidity", btnHumidityToggle);
            btnPHToggle.Click += (s, e) => ToggleSensor("PH", btnPHToggle);
            btnLightToggle.Click += (s, e) => ToggleSensor("Light", btnLightToggle);
            btnSoilToggle.Click += (s, e) => ToggleSensor("Soil", btnSoilToggle);

            btnLogout.Click += (s, e) =>
            {
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Hide();
            };

            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 1000;
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void ToggleSensor(string sensor, Button btn)
        {
            var info = sensors[sensor];

            if (info.IsOn)
            {
                info.TotalOnTime += DateTime.Now - info.StartTime.Value;
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

        // DYNAMIC PATH — saves inside project\alerts\
        private void SaveAlert(string sensor, double value, string limit)
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Directory.GetParent(exeDir).Parent.Parent.Parent.FullName;

            string alertsDir = Path.Combine(projectRoot, "alerts");
            if (!Directory.Exists(alertsDir))
                Directory.CreateDirectory(alertsDir);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");

            string filePath = Path.Combine(alertsDir, $"alert_{sensor}_{timestamp}.json");

            var alert = new AlertEntry
            {
                Sensor = sensor,
                Value = value.ToString(),
                Limit = limit,
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string json = JsonSerializer.Serialize(alert, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateSensorLabel(labelTemperature, txtTemperature, "Temperature");
            UpdateSensorLabel(labelHumidity, txtHumidity, "Humidity");
            UpdateSensorLabel(labelPH, txtPH, "PH");
            UpdateSensorLabel(labelLight, txtLight, "Light");
            UpdateSensorLabel(labelSoil, txtSoil, "Soil");
        }

        private void UpdateSensorLabel(Label lbl, TextBox txt, string sensor)
        {
            var info = sensors[sensor];

            TimeSpan total = info.TotalOnTime;

            if (info.IsOn && info.StartTime.HasValue)
                total += DateTime.Now - info.StartTime.Value;

            string formatted = total.ToString(@"hh\:mm\:ss");

            if (info.IsOn)
                lbl.Text = $"{sensor}: {txt.Text} (ON for {formatted})";
            else
                lbl.Text = $"{sensor}: {txt.Text} (OFF, Total: {formatted})";

            if (double.TryParse(txt.Text, out double value))
            {
                var (min, max) = limits[sensor];

                if (value < min)
                    SaveAlert(sensor, value, $"Min {min}");

                if (value > max)
                    SaveAlert(sensor, value, $"Max {max}");
            }
        }

        private void UpdateAllToggleButtons()
        {
            UpdateToggleButton(btnTempToggle, "Temperature");
            UpdateToggleButton(btnHumidityToggle, "Humidity");
            UpdateToggleButton(btnPHToggle, "PH");
            UpdateToggleButton(btnLightToggle, "Light");
            UpdateToggleButton(btnSoilToggle, "Soil");
        }

        private void UpdateToggleButton(Button btn, string sensor)
        {
            var info = sensors[sensor];

            if (info.IsOn)
            {
                btn.Text = sensor + " ON";
                btn.BackColor = Color.LimeGreen;
            }
            else
            {
                btn.Text = sensor + " OFF";
                btn.BackColor = Color.Red;
            }

            btn.Enabled = role.ToLower() == "admin";
        }
    }
}
