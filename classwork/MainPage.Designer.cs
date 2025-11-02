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
            labelUser = new Label();
            labelTemperature = new Label();
            labelHumidity = new Label();
            labelPH = new Label();
            labelLight = new Label();
            labelSoil = new Label();
            btnTempToggle = new Button();
            btnHumidityToggle = new Button();
            btnPHToggle = new Button();
            btnLightToggle = new Button();
            btnSoilToggle = new Button();
            btnLogout = new Button();
            SuspendLayout();
            // 
            // labelUser
            // 
            labelUser.AutoSize = true;
            labelUser.Font = new Font("Segoe UI", 11F);
            labelUser.Location = new Point(20, 20);
            labelUser.Name = "labelUser";
            labelUser.Size = new Size(0, 20);
            labelUser.TabIndex = 0;
      
            labelTemperature.AutoSize = true;
            labelTemperature.Location = new Point(40, 70);
            labelTemperature.Name = "labelTemperature";
            labelTemperature.Size = new Size(0, 15);
            labelTemperature.TabIndex = 1;

            labelHumidity.AutoSize = true;
            labelHumidity.Location = new Point(40, 110);
            labelHumidity.Name = "labelHumidity";
            labelHumidity.Size = new Size(0, 15);
            labelHumidity.TabIndex = 2;
        
            labelPH.AutoSize = true;
            labelPH.Location = new Point(40, 150);
            labelPH.Name = "labelPH";
            labelPH.Size = new Size(0, 15);
            labelPH.TabIndex = 3;
            // 
            // labelLight
            // 
            labelLight.AutoSize = true;
            labelLight.Location = new Point(40, 190);
            labelLight.Name = "labelLight";
            labelLight.Size = new Size(0, 15);
            labelLight.TabIndex = 4;
            // 
            // labelSoil
            // 
            labelSoil.AutoSize = true;
            labelSoil.Location = new Point(40, 230);
            labelSoil.Name = "labelSoil";
            labelSoil.Size = new Size(0, 15);
            labelSoil.TabIndex = 5;
            // 
            // btnTempToggle
            // 
            btnTempToggle.Location = new Point(300, 70);
            btnTempToggle.Name = "btnTempToggle";
            btnTempToggle.Size = new Size(100, 30);
            btnTempToggle.TabIndex = 6;
            // 
            // btnHumidityToggle
            // 
            btnHumidityToggle.Location = new Point(300, 110);
            btnHumidityToggle.Name = "btnHumidityToggle";
            btnHumidityToggle.Size = new Size(100, 30);
            btnHumidityToggle.TabIndex = 7;
            // 
            // btnPHToggle
            // 
            btnPHToggle.Location = new Point(300, 150);
            btnPHToggle.Name = "btnPHToggle";
            btnPHToggle.Size = new Size(100, 30);
            btnPHToggle.TabIndex = 8;
            // 
            // btnLightToggle
            // 
            btnLightToggle.Location = new Point(300, 190);
            btnLightToggle.Name = "btnLightToggle";
            btnLightToggle.Size = new Size(100, 30);
            btnLightToggle.TabIndex = 9;
            // 
            // btnSoilToggle
            // 
            btnSoilToggle.Location = new Point(300, 230);
            btnSoilToggle.Name = "btnSoilToggle";
            btnSoilToggle.Size = new Size(100, 30);
            btnSoilToggle.TabIndex = 10;
            // 
            // btnLogout
            // 
            btnLogout.Location = new Point(350, 20);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(80, 30);
            btnLogout.TabIndex = 11;
            btnLogout.Text = "Log Out";
            // 
            // MainPage
            // 
            ClientSize = new Size(450, 300);
            Controls.Add(labelUser);
            Controls.Add(labelTemperature);
            Controls.Add(labelHumidity);
            Controls.Add(labelPH);
            Controls.Add(labelLight);
            Controls.Add(labelSoil);
            Controls.Add(btnTempToggle);
            Controls.Add(btnHumidityToggle);
            Controls.Add(btnPHToggle);
            Controls.Add(btnLightToggle);
            Controls.Add(btnSoilToggle);
            Controls.Add(btnLogout);
            Name = "MainPage";
            Text = "Main Page";
            Load += MainPage_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}