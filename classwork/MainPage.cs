using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace classwork
{
    public partial class MainPage : Form
    {
        private string role;

        private static Dictionary<string, bool> sensorState = new Dictionary<string, bool>()
        {
            { "Temperature", true },
            { "Humidity", true },
            { "PH", true },
            { "Light", true },
            { "Soil", true }
        };

        public MainPage(string username, string role)
        {
            InitializeComponent();
            this.role = role;
            this.FormClosed += (s, e) => Application.Exit();

            labelUser.Text = "Logged in as: " + username;
            labelTemperature.Text = "Temperature: -- °C";
            labelHumidity.Text = "Air Humidity: -- %";
            labelPH.Text = "Water PH: --";
            labelLight.Text = "Light Level: -- lux";
            labelSoil.Text = "Soil Moisture: -- %";

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
        }

        private void ToggleSensor(string sensor, Button btn)
        {
            sensorState[sensor] = !sensorState[sensor];
            UpdateAllToggleButtons();
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
            if (sensorState[sensor])
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

        private void MainPage_Load(object sender, EventArgs e)
        {

        }
    }
}
