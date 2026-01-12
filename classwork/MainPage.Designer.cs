using System.IO.Ports;
using System.Text.Json;

namespace classwork
{
    partial class MainPage
    {
        private System.ComponentModel.IContainer components = null;

        // ===== Actuator status labels =====
        private System.Windows.Forms.Label lblFanStatus;
        private System.Windows.Forms.Label lblHeaterStatus;
        private System.Windows.Forms.Label lblPumpStatus;
        private System.Windows.Forms.Button btnLogs;

        // ===== Actuator time labels =====
        private System.Windows.Forms.Label lblFanTime;
        private System.Windows.Forms.Label lblHeaterTime;
        private System.Windows.Forms.Label lblPumpTime;

        // ===== User & sensor labels =====
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.Label labelHumidity;
        private System.Windows.Forms.Label labelSoil;

        // ===== Sprinkler labels =====
        private System.Windows.Forms.Label labelSprinklerTime;
        private System.Windows.Forms.Label labelSprinklerDuration;
        private System.Windows.Forms.Label lblSprinklerInfo;

        // ===== Buttons =====
        private System.Windows.Forms.Button btnTempToggle;
        private System.Windows.Forms.Button btnHumidityToggle;
        private System.Windows.Forms.Button btnSoilToggle;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnHelp;

        // ===== TextBoxes =====
        private System.Windows.Forms.TextBox txtTemperature;
        private System.Windows.Forms.TextBox txtHumidity;
        private System.Windows.Forms.TextBox txtSoil;

        // ===== Sprinkler TextBoxes =====
        private System.Windows.Forms.TextBox txtSprinklerTime;
        private System.Windows.Forms.TextBox txtSprinklerDuration;

        // ===== ESP UI =====
        private System.Windows.Forms.Label lblEspStatus;
        private System.Windows.Forms.TextBox txtEspLog;
        private System.Windows.Forms.TextBox txtEspCommand;
        private System.Windows.Forms.Button btnEspSend;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnLogs = new Button();
            labelUser = new Label();
            labelTemperature = new Label();
            labelHumidity = new Label();
            labelSoil = new Label();
            labelSprinklerTime = new Label();
            labelSprinklerDuration = new Label();
            lblSprinklerInfo = new Label();
            txtTemperature = new TextBox();
            txtHumidity = new TextBox();
            txtSoil = new TextBox();
            txtSprinklerTime = new TextBox();
            txtSprinklerDuration = new TextBox();
            btnTempToggle = new Button();
            btnHumidityToggle = new Button();
            btnSoilToggle = new Button();
            btnLogout = new Button();
            btnHelp = new Button();
            lblFanStatus = new Label();
            lblHeaterStatus = new Label();
            lblPumpStatus = new Label();
            lblFanTime = new Label();
            lblHeaterTime = new Label();
            lblPumpTime = new Label();
            lblEspStatus = new Label();
            txtEspLog = new TextBox();
            txtEspCommand = new TextBox();
            btnEspSend = new Button();
            SuspendLayout();
            // 
            // labelUser
            // 
            labelUser.AutoSize = true;
            labelUser.Font = new Font("Segoe UI", 16F);
            labelUser.Location = new Point(40, 30);
            labelUser.Name = "labelUser";
            labelUser.Size = new Size(0, 30);
            labelUser.TabIndex = 0;
            // 
            // labelTemperature
            // 
            labelTemperature.AutoSize = true;
            labelTemperature.Font = new Font("Segoe UI", 14F);
            labelTemperature.Location = new Point(40, 120);
            labelTemperature.Name = "labelTemperature";
            labelTemperature.Size = new Size(122, 25);
            labelTemperature.TabIndex = 1;
            labelTemperature.Text = "Temperature:";
            // 
            // labelHumidity
            // 
            labelHumidity.AutoSize = true;
            labelHumidity.Font = new Font("Segoe UI", 14F);
            labelHumidity.Location = new Point(40, 170);
            labelHumidity.Name = "labelHumidity";
            labelHumidity.Size = new Size(92, 25);
            labelHumidity.TabIndex = 2;
            labelHumidity.Text = "Humidity:";
            // 
            // labelSoil
            // 
            labelSoil.AutoSize = true;
            labelSoil.Font = new Font("Segoe UI", 14F);
            labelSoil.Location = new Point(40, 220);
            labelSoil.Name = "labelSoil";
            labelSoil.Size = new Size(127, 25);
            labelSoil.TabIndex = 3;
            labelSoil.Text = "Soil Moisture:";
            // 
            // labelSprinklerTime
            // 
            labelSprinklerTime.AutoSize = true;
            labelSprinklerTime.Font = new Font("Segoe UI", 14F);
            labelSprinklerTime.Location = new Point(40, 280);
            labelSprinklerTime.Name = "labelSprinklerTime";
            labelSprinklerTime.Size = new Size(216, 25);
            labelSprinklerTime.TabIndex = 10;
            labelSprinklerTime.Text = "Sprinkler Time (HH:mm):";
            // 
            // labelSprinklerDuration
            // 
            labelSprinklerDuration.AutoSize = true;
            labelSprinklerDuration.Font = new Font("Segoe UI", 14F);
            labelSprinklerDuration.Location = new Point(40, 330);
            labelSprinklerDuration.Name = "labelSprinklerDuration";
            labelSprinklerDuration.Size = new Size(139, 25);
            labelSprinklerDuration.TabIndex = 12;
            labelSprinklerDuration.Text = "Duration (min):";
            // 
            // lblSprinklerInfo
            // 
            lblSprinklerInfo.AutoSize = true;
            lblSprinklerInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSprinklerInfo.Location = new Point(40, 385);
            lblSprinklerInfo.Name = "lblSprinklerInfo";
            lblSprinklerInfo.Size = new Size(210, 21);
            lblSprinklerInfo.TabIndex = 14;
            lblSprinklerInfo.Text = "Sprinkler: 12:00 for 10 min";
            // 
            // txtTemperature
            // 
            txtTemperature.Font = new Font("Segoe UI", 14F);
            txtTemperature.Location = new Point(329, 124);
            txtTemperature.Name = "txtTemperature";
            txtTemperature.Size = new Size(180, 32);
            txtTemperature.TabIndex = 4;
            txtTemperature.TextChanged += txtTemperature_TextChanged;
            txtTemperature.Leave += txtTemperature_Leave;
            // 
            // txtHumidity
            // 
            txtHumidity.Font = new Font("Segoe UI", 14F);
            txtHumidity.Location = new Point(329, 170);
            txtHumidity.Name = "txtHumidity";
            txtHumidity.Size = new Size(180, 32);
            txtHumidity.TabIndex = 5;
            txtHumidity.Leave += txtHumidity_Leave;
            // 
            // txtSoil
            // 
            txtSoil.Font = new Font("Segoe UI", 14F);
            txtSoil.Location = new Point(329, 220);
            txtSoil.Name = "txtSoil";
            txtSoil.Size = new Size(180, 32);
            txtSoil.TabIndex = 6;
            txtSoil.Leave += txtSoil_Leave;
            // 
            // txtSprinklerTime
            // 
            txtSprinklerTime.Font = new Font("Segoe UI", 14F);
            txtSprinklerTime.Location = new Point(329, 277);
            txtSprinklerTime.Name = "txtSprinklerTime";
            txtSprinklerTime.Size = new Size(180, 32);
            txtSprinklerTime.TabIndex = 11;
            txtSprinklerTime.Text = "12:00";
            txtSprinklerTime.TextChanged += txtSprinklerTime_TextChanged;
            txtSprinklerTime.KeyPress += txtSprinklerTime_KeyPress;
            txtSprinklerTime.Leave += txtSprinklerTime_Leave;
            // 
            // txtSprinklerDuration
            // 
            txtSprinklerDuration.Font = new Font("Segoe UI", 14F);
            txtSprinklerDuration.Location = new Point(329, 330);
            txtSprinklerDuration.Name = "txtSprinklerDuration";
            txtSprinklerDuration.Size = new Size(180, 32);
            txtSprinklerDuration.TabIndex = 13;
            txtSprinklerDuration.Text = "10";
            txtSprinklerDuration.KeyPress += txtSprinklerDuration_KeyPress_1;
            // 
            // btnTempToggle
            // 
            btnTempToggle.Location = new Point(620, 120);
            btnTempToggle.Name = "btnTempToggle";
            btnTempToggle.Size = new Size(180, 45);
            btnTempToggle.TabIndex = 7;
            // 
            // btnHumidityToggle
            // 
            btnHumidityToggle.Location = new Point(620, 170);
            btnHumidityToggle.Name = "btnHumidityToggle";
            btnHumidityToggle.Size = new Size(180, 45);
            btnHumidityToggle.TabIndex = 8;
            // 
            // btnSoilToggle
            // 
            btnSoilToggle.Location = new Point(620, 220);
            btnSoilToggle.Name = "btnSoilToggle";
            btnSoilToggle.Size = new Size(180, 45);
            btnSoilToggle.TabIndex = 9;
            // 
            // btnLogout
            // 
            btnLogout.Font = new Font("Segoe UI", 12F);
            btnLogout.Location = new Point(750, 20);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(120, 45);
            btnLogout.TabIndex = 16;
            btnLogout.Text = "Log Out";
            // 
            // btnHelp
            // 
            btnHelp.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnHelp.Location = new Point(700, 20);
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new Size(40, 45);
            btnHelp.TabIndex = 15;
            btnHelp.Text = "?";
            btnHelp.UseVisualStyleBackColor = true;
            btnHelp.Click += btnHelp_Click;
            // 
            // lblFanStatus
            // 
            lblFanStatus.AutoSize = true;
            lblFanStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFanStatus.Location = new Point(40, 420);
            lblFanStatus.Name = "lblFanStatus";
            lblFanStatus.Size = new Size(73, 21);
            lblFanStatus.TabIndex = 17;
            lblFanStatus.Text = "Fan: OFF";
            // 
            // lblHeaterStatus
            // 
            lblHeaterStatus.AutoSize = true;
            lblHeaterStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblHeaterStatus.Location = new Point(180, 420);
            lblHeaterStatus.Name = "lblHeaterStatus";
            lblHeaterStatus.Size = new Size(98, 21);
            lblHeaterStatus.TabIndex = 19;
            lblHeaterStatus.Text = "Heater: OFF";
            // 
            // lblPumpStatus
            // 
            lblPumpStatus.AutoSize = true;
            lblPumpStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblPumpStatus.Location = new Point(340, 420);
            lblPumpStatus.Name = "lblPumpStatus";
            lblPumpStatus.Size = new Size(91, 21);
            lblPumpStatus.TabIndex = 21;
            lblPumpStatus.Text = "Pump: OFF";
            // 
            // lblFanTime
            // 
            lblFanTime.AutoSize = true;
            lblFanTime.Font = new Font("Segoe UI", 10F);
            lblFanTime.Location = new Point(40, 445);
            lblFanTime.Name = "lblFanTime";
            lblFanTime.Size = new Size(0, 19);
            lblFanTime.TabIndex = 18;
            // 
            // lblHeaterTime
            // 
            lblHeaterTime.AutoSize = true;
            lblHeaterTime.Font = new Font("Segoe UI", 10F);
            lblHeaterTime.Location = new Point(180, 445);
            lblHeaterTime.Name = "lblHeaterTime";
            lblHeaterTime.Size = new Size(0, 19);
            lblHeaterTime.TabIndex = 20;
            // 
            // lblPumpTime
            // 
            lblPumpTime.AutoSize = true;
            lblPumpTime.Font = new Font("Segoe UI", 10F);
            lblPumpTime.Location = new Point(340, 445);
            lblPumpTime.Name = "lblPumpTime";
            lblPumpTime.Size = new Size(0, 19);
            lblPumpTime.TabIndex = 22;
            // 
            // lblEspStatus
            // 
            lblEspStatus.AutoSize = true;
            lblEspStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEspStatus.Location = new Point(40, 470);
            lblEspStatus.Name = "lblEspStatus";
            lblEspStatus.Size = new Size(130, 19);
            lblEspStatus.TabIndex = 23;
            lblEspStatus.Text = "ESP: Disconnected";
            //
            // LOGS
            //
            btnLogs.Font = new Font("Segoe UI", 12F);
            btnLogs.Location = new Point(580, 20);
            btnLogs.Name = "btnLogs";
            btnLogs.Size = new Size(110, 45);
            btnLogs.TabIndex = 27;
            btnLogs.Text = "Logs";
            btnLogs.UseVisualStyleBackColor = true;
            btnLogs.Click += btnLogs_Click;

            // 
            // txtEspLog
            // 
            txtEspLog.Location = new Point(40, 500);
            txtEspLog.Multiline = true;
            txtEspLog.Name = "txtEspLog";
            txtEspLog.ReadOnly = true;
            txtEspLog.ScrollBars = ScrollBars.Vertical;
            txtEspLog.Size = new Size(540, 120);
            txtEspLog.TabIndex = 24;
            // 
            // txtEspCommand
            // 
            txtEspCommand.Location = new Point(40, 635);
            txtEspCommand.Name = "txtEspCommand";
            txtEspCommand.Size = new Size(420, 23);
            txtEspCommand.TabIndex = 25;
            // 
            // btnEspSend
            // 
            btnEspSend.Location = new Point(470, 633);
            btnEspSend.Name = "btnEspSend";
            btnEspSend.Size = new Size(110, 34);
            btnEspSend.TabIndex = 26;
            btnEspSend.Text = "Send to ESP";
            btnEspSend.UseVisualStyleBackColor = true;
            btnEspSend.Click += btnEspSend_Click;
            // 
            // MainPage
            // 
            ClientSize = new Size(900, 700);
            Controls.Add(labelUser);
            Controls.Add(labelTemperature);
            Controls.Add(labelHumidity);
            Controls.Add(labelSoil);
            Controls.Add(txtTemperature);
            Controls.Add(txtHumidity);
            Controls.Add(txtSoil);
            Controls.Add(btnTempToggle);
            Controls.Add(btnHumidityToggle);
            Controls.Add(btnSoilToggle);
            Controls.Add(labelSprinklerTime);
            Controls.Add(txtSprinklerTime);
            Controls.Add(labelSprinklerDuration);
            Controls.Add(txtSprinklerDuration);
            Controls.Add(lblSprinklerInfo);
            Controls.Add(btnHelp);
            Controls.Add(btnLogout);
            Controls.Add(lblFanStatus);
            Controls.Add(lblFanTime);
            Controls.Add(lblHeaterStatus);
            Controls.Add(lblHeaterTime);
            Controls.Add(lblPumpStatus);
            Controls.Add(lblPumpTime);
            Controls.Add(lblEspStatus);
            Controls.Add(txtEspLog);
            Controls.Add(txtEspCommand);
            Controls.Add(btnEspSend);
            Controls.Add(btnLogs);
            Name = "MainPage";
            Text = "Main Page";
            FormClosing += MainPage_FormClosing_1;
            Load += MainPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }
        private void InitializeEspConnection()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                try
                {
                    espSerial = new SerialPort(portName, 115200);
                    espSerial.Open();

                    lblEspStatus.Text = $"ESP: Connected ({portName})";
                    lblEspStatus.ForeColor = Color.Green;
                    return;
                }
                catch
                {
                }
            }

            lblEspStatus.Text = "ESP: Disconnected";
            lblEspStatus.ForeColor = Color.Red;
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


    }
}
