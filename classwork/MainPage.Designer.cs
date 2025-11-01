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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.labelUser = new System.Windows.Forms.Label();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.labelHumidity = new System.Windows.Forms.Label();
            this.labelPH = new System.Windows.Forms.Label();
            this.labelLight = new System.Windows.Forms.Label();
            this.labelSoil = new System.Windows.Forms.Label();

            this.btnTempToggle = new System.Windows.Forms.Button();
            this.btnHumidityToggle = new System.Windows.Forms.Button();
            this.btnPHToggle = new System.Windows.Forms.Button();
            this.btnLightToggle = new System.Windows.Forms.Button();
            this.btnSoilToggle = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();

            this.SuspendLayout();

            this.labelUser.Location = new System.Drawing.Point(20, 20);
            this.labelUser.AutoSize = true;
            this.labelUser.Font = new System.Drawing.Font("Segoe UI", 11F);

            this.labelTemperature.Location = new System.Drawing.Point(40, 70);
            this.labelTemperature.AutoSize = true;

            this.labelHumidity.Location = new System.Drawing.Point(40, 110);
            this.labelHumidity.AutoSize = true;

            this.labelPH.Location = new System.Drawing.Point(40, 150);
            this.labelPH.AutoSize = true;

            this.labelLight.Location = new System.Drawing.Point(40, 190);
            this.labelLight.AutoSize = true;

            this.labelSoil.Location = new System.Drawing.Point(40, 230);
            this.labelSoil.AutoSize = true;

            this.btnTempToggle.Location = new System.Drawing.Point(300, 70);
            this.btnTempToggle.Size = new System.Drawing.Size(100, 30);

            this.btnHumidityToggle.Location = new System.Drawing.Point(300, 110);
            this.btnHumidityToggle.Size = new System.Drawing.Size(100, 30);

            this.btnPHToggle.Location = new System.Drawing.Point(300, 150);
            this.btnPHToggle.Size = new System.Drawing.Size(100, 30);

            this.btnLightToggle.Location = new System.Drawing.Point(300, 190);
            this.btnLightToggle.Size = new System.Drawing.Size(100, 30);

            this.btnSoilToggle.Location = new System.Drawing.Point(300, 230);
            this.btnSoilToggle.Size = new System.Drawing.Size(100, 30);

            this.btnLogout.Location = new System.Drawing.Point(350, 20);
            this.btnLogout.Size = new System.Drawing.Size(80, 30);
            this.btnLogout.Text = "Log Out";

            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.labelTemperature);
            this.Controls.Add(this.labelHumidity);
            this.Controls.Add(this.labelPH);
            this.Controls.Add(this.labelLight);
            this.Controls.Add(this.labelSoil);

            this.Controls.Add(this.btnTempToggle);
            this.Controls.Add(this.btnHumidityToggle);
            this.Controls.Add(this.btnPHToggle);
            this.Controls.Add(this.btnLightToggle);
            this.Controls.Add(this.btnSoilToggle);
            this.Controls.Add(this.btnLogout);

            this.ClientSize = new System.Drawing.Size(450, 300);
            this.Text = "Main Page";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}