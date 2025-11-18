namespace classwork
{
    partial class MainPage
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.Label labelHumidity;
        private System.Windows.Forms.Label labelPH;
        private System.Windows.Forms.Label labelLight;
        private System.Windows.Forms.Label labelSoil;

        private System.Windows.Forms.Button btnTempToggle;
        private System.Windows.Forms.Button btnHumidityToggle;
        private System.Windows.Forms.Button btnPHToggle;
        private System.Windows.Forms.Button btnLightToggle;
        private System.Windows.Forms.Button btnSoilToggle;
        private System.Windows.Forms.Button btnLogout;

        private System.Windows.Forms.TextBox txtTemperature;
        private System.Windows.Forms.TextBox txtHumidity;
        private System.Windows.Forms.TextBox txtPH;
        private System.Windows.Forms.TextBox txtLight;
        private System.Windows.Forms.TextBox txtSoil;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            labelUser = new Label();
            labelTemperature = new Label();
            labelHumidity = new Label();
            labelPH = new Label();
            labelLight = new Label();
            labelSoil = new Label();

            txtTemperature = new TextBox();
            txtHumidity = new TextBox();
            txtPH = new TextBox();
            txtLight = new TextBox();
            txtSoil = new TextBox();

            btnTempToggle = new Button();
            btnHumidityToggle = new Button();
            btnPHToggle = new Button();
            btnLightToggle = new Button();
            btnSoilToggle = new Button();
            btnLogout = new Button();

            SuspendLayout();

            ClientSize = new Size(900, 500);

            labelUser.AutoSize = true;
            labelUser.Font = new Font("Segoe UI", 16F);
            labelUser.Location = new Point(40, 30);

            Font labelFont = new Font("Segoe UI", 14F);

            labelTemperature.AutoSize = true;
            labelTemperature.Font = labelFont;
            labelTemperature.Location = new Point(40, 120);

            labelHumidity.AutoSize = true;
            labelHumidity.Font = labelFont;
            labelHumidity.Location = new Point(40, 170);

            labelPH.AutoSize = true;
            labelPH.Font = labelFont;
            labelPH.Location = new Point(40, 220);

            labelLight.AutoSize = true;
            labelLight.Font = labelFont;
            labelLight.Location = new Point(40, 270);

            labelSoil.AutoSize = true;
            labelSoil.Font = labelFont;
            labelSoil.Location = new Point(40, 320);

            txtTemperature.Location = new Point(400, 120);
            txtTemperature.Size = new Size(180, 35);
            txtTemperature.Font = new Font("Segoe UI", 14F);

            txtHumidity.Location = new Point(400, 170);
            txtHumidity.Size = new Size(180, 35);
            txtHumidity.Font = new Font("Segoe UI", 14F);

            txtPH.Location = new Point(400, 220);
            txtPH.Size = new Size(180, 35);
            txtPH.Font = new Font("Segoe UI", 14F);

            txtLight.Location = new Point(400, 270);
            txtLight.Size = new Size(180, 35);
            txtLight.Font = new Font("Segoe UI", 14F);

            txtSoil.Location = new Point(400, 320);
            txtSoil.Size = new Size(180, 35);
            txtSoil.Font = new Font("Segoe UI", 14F);

            Size btnSize = new Size(180, 45);

            btnTempToggle.Location = new Point(620, 120);
            btnTempToggle.Size = btnSize;

            btnHumidityToggle.Location = new Point(620, 170);
            btnHumidityToggle.Size = btnSize;

            btnPHToggle.Location = new Point(620, 220);
            btnPHToggle.Size = btnSize;

            btnLightToggle.Location = new Point(620, 270);
            btnLightToggle.Size = btnSize;

            btnSoilToggle.Location = new Point(620, 320);
            btnSoilToggle.Size = btnSize;

            btnLogout.Location = new Point(750, 20);
            btnLogout.Size = new Size(120, 45);
            btnLogout.Font = new Font("Segoe UI", 12F);
            btnLogout.Text = "Log Out";

            Controls.Add(labelUser);
            Controls.Add(labelTemperature);
            Controls.Add(labelHumidity);
            Controls.Add(labelPH);
            Controls.Add(labelLight);
            Controls.Add(labelSoil);

            Controls.Add(txtTemperature);
            Controls.Add(txtHumidity);
            Controls.Add(txtPH);
            Controls.Add(txtLight);
            Controls.Add(txtSoil);

            Controls.Add(btnTempToggle);
            Controls.Add(btnHumidityToggle);
            Controls.Add(btnPHToggle);
            Controls.Add(btnLightToggle);
            Controls.Add(btnSoilToggle);
            Controls.Add(btnLogout);

            Name = "MainPage";
            Text = "Main Page";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
