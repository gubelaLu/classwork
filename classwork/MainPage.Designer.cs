
namespace classwork
{
    partial class MainPage
    {
        private System.Windows.Forms.CheckBox chkPumpOverride;

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnManualSprinkler;
        private System.Windows.Forms.Button btnManualHeater;
        private System.Windows.Forms.Button btnManualFan;

        private CheckBox chkFanOverride;


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
        private void UpdateOverrideUI()
        {
            if (chkFanOverride == null ||
                chkHeaterOverride == null ||
                chkPumpOverride == null)
                return;
        }



        private void InitializeComponent()
        {
            chkFanOverride = new CheckBox();
            btnManualFan = new Button();
            chkPumpOverride = new CheckBox();
            btnManualSprinkler = new Button();
            btnManualHeater = new Button();
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
            chkHeaterOverride = new CheckBox();
            SuspendLayout();
            // 
            // chkFanOverride
            // 
            chkFanOverride.AutoSize = true;
            chkFanOverride.Location = new Point(806, 444);
            chkFanOverride.Name = "chkFanOverride";
            chkFanOverride.Size = new Size(88, 24);
            chkFanOverride.TabIndex = 102;
            chkFanOverride.Text = "Override";
            chkFanOverride.CheckedChanged += chkFanOverride_CheckedChanged;
            // 
            // btnManualFan
            // 
            btnManualFan.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnManualFan.Location = new Point(540, 430);
            btnManualFan.Name = "btnManualFan";
            btnManualFan.Size = new Size(260, 50);
            btnManualFan.TabIndex = 101;
            btnManualFan.Text = "Fan: MANUAL OFF";
            btnManualFan.UseVisualStyleBackColor = true;
            btnManualFan.Click += btnManualFan_Click;
            // 
            // chkPumpOverride
            // 
            chkPumpOverride.AutoSize = true;
            chkPumpOverride.Location = new Point(806, 330);
            chkPumpOverride.Name = "chkPumpOverride";
            chkPumpOverride.Size = new Size(88, 24);
            chkPumpOverride.TabIndex = 0;
            chkPumpOverride.Text = "Override";
            chkPumpOverride.CheckedChanged += chkPumpOverride_CheckedChanged;
            // 
            // btnManualSprinkler
            // 
            btnManualSprinkler.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnManualSprinkler.Location = new Point(540, 310);
            btnManualSprinkler.Name = "btnManualSprinkler";
            btnManualSprinkler.Size = new Size(260, 50);
            btnManualSprinkler.TabIndex = 99;
            btnManualSprinkler.Text = "Sprinkler: MANUAL OFF";
            btnManualSprinkler.UseVisualStyleBackColor = true;
            btnManualSprinkler.Click += btnManualSprinkler_Click;
            // 
            // btnManualHeater
            // 
            btnManualHeater.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnManualHeater.Location = new Point(540, 370);
            btnManualHeater.Name = "btnManualHeater";
            btnManualHeater.Size = new Size(260, 50);
            btnManualHeater.TabIndex = 100;
            btnManualHeater.Text = "Heater: MANUAL OFF";
            btnManualHeater.UseVisualStyleBackColor = true;
            btnManualHeater.Click += btnManualHeater_Click;
            // 
            // btnLogs
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
            // labelUser
            // 
            labelUser.AutoSize = true;
            labelUser.Font = new Font("Segoe UI", 16F);
            labelUser.Location = new Point(40, 30);
            labelUser.Name = "labelUser";
            labelUser.Size = new Size(0, 37);
            labelUser.TabIndex = 0;
            // 
            // labelTemperature
            // 
            labelTemperature.AutoSize = true;
            labelTemperature.Font = new Font("Segoe UI", 14F);
            labelTemperature.Location = new Point(40, 120);
            labelTemperature.Name = "labelTemperature";
            labelTemperature.Size = new Size(154, 32);
            labelTemperature.TabIndex = 1;
            labelTemperature.Text = "Temperature:";
            // 
            // labelHumidity
            // 
            labelHumidity.AutoSize = true;
            labelHumidity.Font = new Font("Segoe UI", 14F);
            labelHumidity.Location = new Point(40, 170);
            labelHumidity.Name = "labelHumidity";
            labelHumidity.Size = new Size(117, 32);
            labelHumidity.TabIndex = 2;
            labelHumidity.Text = "Humidity:";
            // 
            // labelSoil
            // 
            labelSoil.AutoSize = true;
            labelSoil.Font = new Font("Segoe UI", 14F);
            labelSoil.Location = new Point(40, 220);
            labelSoil.Name = "labelSoil";
            labelSoil.Size = new Size(160, 32);
            labelSoil.TabIndex = 3;
            labelSoil.Text = "Soil Moisture:";
            // 
            // labelSprinklerTime
            // 
            labelSprinklerTime.AutoSize = true;
            labelSprinklerTime.Font = new Font("Segoe UI", 14F);
            labelSprinklerTime.Location = new Point(40, 280);
            labelSprinklerTime.Name = "labelSprinklerTime";
            labelSprinklerTime.Size = new Size(275, 32);
            labelSprinklerTime.TabIndex = 10;
            labelSprinklerTime.Text = "Sprinkler Time (HH:mm):";
            // 
            // labelSprinklerDuration
            // 
            labelSprinklerDuration.AutoSize = true;
            labelSprinklerDuration.Font = new Font("Segoe UI", 14F);
            labelSprinklerDuration.Location = new Point(40, 330);
            labelSprinklerDuration.Name = "labelSprinklerDuration";
            labelSprinklerDuration.Size = new Size(174, 32);
            labelSprinklerDuration.TabIndex = 12;
            labelSprinklerDuration.Text = "Duration (min):";
            // 
            // lblSprinklerInfo
            // 
            lblSprinklerInfo.AutoSize = true;
            lblSprinklerInfo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSprinklerInfo.Location = new Point(40, 385);
            lblSprinklerInfo.Name = "lblSprinklerInfo";
            lblSprinklerInfo.Size = new Size(268, 28);
            lblSprinklerInfo.TabIndex = 14;
            lblSprinklerInfo.Text = "Sprinkler: 12:00 for 10 min";
            // 
            // txtTemperature
            // 
            txtTemperature.Font = new Font("Segoe UI", 14F);
            txtTemperature.Location = new Point(329, 124);
            txtTemperature.Name = "txtTemperature";
            txtTemperature.Size = new Size(180, 39);
            txtTemperature.TabIndex = 4;
            txtTemperature.TextChanged += txtTemperature_TextChanged;
            txtTemperature.Leave += txtTemperature_Leave;
            // 
            // txtHumidity
            // 
            txtHumidity.Font = new Font("Segoe UI", 14F);
            txtHumidity.Location = new Point(329, 170);
            txtHumidity.Name = "txtHumidity";
            txtHumidity.Size = new Size(180, 39);
            txtHumidity.TabIndex = 5;
            txtHumidity.Leave += txtHumidity_Leave;
            // 
            // txtSoil
            // 
            txtSoil.Font = new Font("Segoe UI", 14F);
            txtSoil.Location = new Point(329, 220);
            txtSoil.Name = "txtSoil";
            txtSoil.Size = new Size(180, 39);
            txtSoil.TabIndex = 6;
            txtSoil.Leave += txtSoil_Leave;
            // 
            // txtSprinklerTime
            // 
            txtSprinklerTime.Font = new Font("Segoe UI", 14F);
            txtSprinklerTime.Location = new Point(329, 277);
            txtSprinklerTime.Name = "txtSprinklerTime";
            txtSprinklerTime.Size = new Size(180, 39);
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
            txtSprinklerDuration.Size = new Size(180, 39);
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
            lblFanStatus.Size = new Size(90, 28);
            lblFanStatus.TabIndex = 17;
            lblFanStatus.Text = "Fan: OFF";
            // 
            // lblHeaterStatus
            // 
            lblHeaterStatus.AutoSize = true;
            lblHeaterStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblHeaterStatus.Location = new Point(180, 420);
            lblHeaterStatus.Name = "lblHeaterStatus";
            lblHeaterStatus.Size = new Size(123, 28);
            lblHeaterStatus.TabIndex = 19;
            lblHeaterStatus.Text = "Heater: OFF";
            // 
            // lblPumpStatus
            // 
            lblPumpStatus.AutoSize = true;
            lblPumpStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblPumpStatus.Location = new Point(340, 420);
            lblPumpStatus.Name = "lblPumpStatus";
            lblPumpStatus.Size = new Size(112, 28);
            lblPumpStatus.TabIndex = 21;
            lblPumpStatus.Text = "Pump: OFF";
            // 
            // lblFanTime
            // 
            lblFanTime.AutoSize = true;
            lblFanTime.Font = new Font("Segoe UI", 10F);
            lblFanTime.Location = new Point(40, 445);
            lblFanTime.Name = "lblFanTime";
            lblFanTime.Size = new Size(0, 23);
            lblFanTime.TabIndex = 18;
            // 
            // lblHeaterTime
            // 
            lblHeaterTime.AutoSize = true;
            lblHeaterTime.Font = new Font("Segoe UI", 10F);
            lblHeaterTime.Location = new Point(180, 445);
            lblHeaterTime.Name = "lblHeaterTime";
            lblHeaterTime.Size = new Size(0, 23);
            lblHeaterTime.TabIndex = 20;
            // 
            // lblPumpTime
            // 
            lblPumpTime.AutoSize = true;
            lblPumpTime.Font = new Font("Segoe UI", 10F);
            lblPumpTime.Location = new Point(340, 445);
            lblPumpTime.Name = "lblPumpTime";
            lblPumpTime.Size = new Size(0, 23);
            lblPumpTime.TabIndex = 22;
            // 
            // lblEspStatus
            // 
            lblEspStatus.AutoSize = true;
            lblEspStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEspStatus.Location = new Point(40, 470);
            lblEspStatus.Name = "lblEspStatus";
            lblEspStatus.Size = new Size(156, 23);
            lblEspStatus.TabIndex = 23;
            lblEspStatus.Text = "ESP: Disconnected";
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
            txtEspCommand.Size = new Size(420, 27);
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
            // chkHeaterOverride
            // 
            chkHeaterOverride.AutoSize = true;
            chkHeaterOverride.Location = new Point(806, 385);
            chkHeaterOverride.Name = "chkHeaterOverride";
            chkHeaterOverride.Size = new Size(88, 24);
            chkHeaterOverride.TabIndex = 150;
            chkHeaterOverride.Text = "Override";
            chkHeaterOverride.UseVisualStyleBackColor = true;
            chkHeaterOverride.CheckedChanged += chkHeaterOverride_CheckedChanged;
            // 
            // MainPage
            // 
            ClientSize = new Size(900, 700);
            Controls.Add(chkPumpOverride);
            Controls.Add(btnManualHeater);
            Controls.Add(btnManualFan);
            Controls.Add(chkFanOverride);
            Controls.Add(btnManualSprinkler);
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
            Controls.Add(chkHeaterOverride);
            Name = "MainPage";
            Text = "Main Page";
            Load += MainPage_Load;
            ResumeLayout(false);
            PerformLayout();

        }
        private CheckBox chkHeaterOverride;
    }




}