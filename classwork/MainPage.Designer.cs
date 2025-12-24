namespace classwork
{
    partial class MainPage
    {
        private System.ComponentModel.IContainer components = null;

        // ===== Actuator status labels =====
        private System.Windows.Forms.Label lblFanStatus;
        private System.Windows.Forms.Label lblHeaterStatus;
        private System.Windows.Forms.Label lblPumpStatus;

        // ===== Actuator time labels =====
        private System.Windows.Forms.Label lblFanTime;
        private System.Windows.Forms.Label lblHeaterTime;
        private System.Windows.Forms.Label lblPumpTime;

        // ===== User & sensor labels =====
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.Label labelHumidity;
        private System.Windows.Forms.Label labelSoil;

        // ===== Buttons =====
        private System.Windows.Forms.Button btnTempToggle;
        private System.Windows.Forms.Button btnHumidityToggle;
        private System.Windows.Forms.Button btnSoilToggle;
        private System.Windows.Forms.Button btnLogout;

        // ===== Help button (NEW) =====
        private System.Windows.Forms.Button btnHelp;

        // ===== TextBoxes =====
        private System.Windows.Forms.TextBox txtTemperature;
        private System.Windows.Forms.TextBox txtHumidity;
        private System.Windows.Forms.TextBox txtSoil;

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
            labelUser = new System.Windows.Forms.Label();
            labelTemperature = new System.Windows.Forms.Label();
            labelHumidity = new System.Windows.Forms.Label();
            labelSoil = new System.Windows.Forms.Label();

            txtTemperature = new System.Windows.Forms.TextBox();
            txtHumidity = new System.Windows.Forms.TextBox();
            txtSoil = new System.Windows.Forms.TextBox();

            btnTempToggle = new System.Windows.Forms.Button();
            btnHumidityToggle = new System.Windows.Forms.Button();
            btnSoilToggle = new System.Windows.Forms.Button();
            btnLogout = new System.Windows.Forms.Button();

            // NEW
            btnHelp = new System.Windows.Forms.Button();

            lblFanStatus = new System.Windows.Forms.Label();
            lblHeaterStatus = new System.Windows.Forms.Label();
            lblPumpStatus = new System.Windows.Forms.Label();

            lblFanTime = new System.Windows.Forms.Label();
            lblHeaterTime = new System.Windows.Forms.Label();
            lblPumpTime = new System.Windows.Forms.Label();

            lblEspStatus = new System.Windows.Forms.Label();
            txtEspLog = new System.Windows.Forms.TextBox();
            txtEspCommand = new System.Windows.Forms.TextBox();
            btnEspSend = new System.Windows.Forms.Button();

            SuspendLayout();

            ClientSize = new System.Drawing.Size(900, 650);

            // ===== User label =====
            labelUser.AutoSize = true;
            labelUser.Font = new System.Drawing.Font("Segoe UI", 16F);
            labelUser.Location = new System.Drawing.Point(40, 30);

            System.Drawing.Font labelFont = new System.Drawing.Font("Segoe UI", 14F);

            // ===== Sensor labels =====
            labelTemperature.AutoSize = true;
            labelTemperature.Font = labelFont;
            labelTemperature.Location = new System.Drawing.Point(40, 120);
            labelTemperature.Text = "Temperature:";

            labelHumidity.AutoSize = true;
            labelHumidity.Font = labelFont;
            labelHumidity.Location = new System.Drawing.Point(40, 170);
            labelHumidity.Text = "Humidity:";

            labelSoil.AutoSize = true;
            labelSoil.Font = labelFont;
            labelSoil.Location = new System.Drawing.Point(40, 220);
            labelSoil.Text = "Soil Moisture:";

            // ===== TextBoxes =====
            txtTemperature.Location = new System.Drawing.Point(400, 120);
            txtTemperature.Size = new System.Drawing.Size(180, 35);
            txtTemperature.Font = new System.Drawing.Font("Segoe UI", 14F);

            txtHumidity.Location = new System.Drawing.Point(400, 170);
            txtHumidity.Size = new System.Drawing.Size(180, 35);
            txtHumidity.Font = new System.Drawing.Font("Segoe UI", 14F);

            txtSoil.Location = new System.Drawing.Point(400, 220);
            txtSoil.Size = new System.Drawing.Size(180, 35);
            txtSoil.Font = new System.Drawing.Font("Segoe UI", 14F);

            // ===== Toggle buttons =====
            System.Drawing.Size btnSize = new System.Drawing.Size(180, 45);

            btnTempToggle.Location = new System.Drawing.Point(620, 120);
            btnTempToggle.Size = btnSize;

            btnHumidityToggle.Location = new System.Drawing.Point(620, 170);
            btnHumidityToggle.Size = btnSize;

            btnSoilToggle.Location = new System.Drawing.Point(620, 220);
            btnSoilToggle.Size = btnSize;

            // ===== Logout =====
            btnLogout.Location = new System.Drawing.Point(750, 20);
            btnLogout.Size = new System.Drawing.Size(120, 45);
            btnLogout.Font = new System.Drawing.Font("Segoe UI", 12F);
            btnLogout.Text = "Log Out";

            // ===== Help button (NEW) =====
            btnHelp.Location = new System.Drawing.Point(700, 20);
            btnHelp.Size = new System.Drawing.Size(40, 45);
            btnHelp.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            btnHelp.Text = "?";
            btnHelp.UseVisualStyleBackColor = true;
            btnHelp.Click += new System.EventHandler(this.btnHelp_Click);

            // =====================
            // ACTUATOR STATUS
            // =====================
            System.Drawing.Font statusFont =
                new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            System.Drawing.Font timeFont =
                new System.Drawing.Font("Segoe UI", 10F);

            lblFanStatus.AutoSize = true;
            lblFanStatus.Font = statusFont;
            lblFanStatus.Location = new System.Drawing.Point(40, 270);
            lblFanStatus.Text = "Fan: OFF";
            lblFanStatus.ForeColor = System.Drawing.Color.Red;

            lblFanTime.AutoSize = true;
            lblFanTime.Font = timeFont;
            lblFanTime.Location = new System.Drawing.Point(40, 295);
            lblFanTime.Text = "00:00:00";

            lblHeaterStatus.AutoSize = true;
            lblHeaterStatus.Font = statusFont;
            lblHeaterStatus.Location = new System.Drawing.Point(180, 270);
            lblHeaterStatus.Text = "Heater: OFF";
            lblHeaterStatus.ForeColor = System.Drawing.Color.Red;

            lblHeaterTime.AutoSize = true;
            lblHeaterTime.Font = timeFont;
            lblHeaterTime.Location = new System.Drawing.Point(180, 295);
            lblHeaterTime.Text = "00:00:00";

            lblPumpStatus.AutoSize = true;
            lblPumpStatus.Font = statusFont;
            lblPumpStatus.Location = new System.Drawing.Point(340, 270);
            lblPumpStatus.Text = "Pump: OFF";
            lblPumpStatus.ForeColor = System.Drawing.Color.Red;

            lblPumpTime.AutoSize = true;
            lblPumpTime.Font = timeFont;
            lblPumpTime.Location = new System.Drawing.Point(340, 295);
            lblPumpTime.Text = "00:00:00";

            // =====================
            // ESP UI
            // =====================
            lblEspStatus.AutoSize = true;
            lblEspStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblEspStatus.Location = new System.Drawing.Point(40, 320);
            lblEspStatus.Text = "ESP: Disconnected";

            txtEspLog.Location = new System.Drawing.Point(40, 350);
            txtEspLog.Size = new System.Drawing.Size(540, 140);
            txtEspLog.Multiline = true;
            txtEspLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtEspLog.ReadOnly = true;

            txtEspCommand.Location = new System.Drawing.Point(40, 510);
            txtEspCommand.Size = new System.Drawing.Size(420, 30);

            btnEspSend.Location = new System.Drawing.Point(470, 508);
            btnEspSend.Size = new System.Drawing.Size(110, 34);
            btnEspSend.Text = "Send to ESP";
            btnEspSend.UseVisualStyleBackColor = true;
            btnEspSend.Click += new System.EventHandler(this.btnEspSend_Click);

            // =====================
            // ADD CONTROLS
            // =====================
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

            Controls.Add(btnHelp);   // NEW
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

            Name = "MainPage";
            Text = "Main Page";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
