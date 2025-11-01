using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace classwork
{
    public partial class MainPage : Form
    {
        private string role;
        private Dictionary<string, bool> sensorState = new Dictionary<string, bool>()
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

            UpdateToggleButton(btnTempToggle, "Temperature");
            UpdateToggleButton(btnHumidityToggle, "Humidity");
            UpdateToggleButton(btnPHToggle, "PH");
            UpdateToggleButton(btnLightToggle, "Light");
            UpdateToggleButton(btnSoilToggle, "Soil");

            SetupAdminControls();

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
            UpdateToggleButton(btn, sensor);
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
        }

        private void SetupAdminControls()
        {
            bool isAdmin = role.ToLower() == "admin";

            btnTempToggle.Visible = isAdmin;
            btnHumidityToggle.Visible = isAdmin;
            btnPHToggle.Visible = isAdmin;
            btnLightToggle.Visible = isAdmin;
            btnSoilToggle.Visible = isAdmin;
        }
    }
}
