using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace classwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int x = 30, y = 30;
        System.Windows.Forms.Timer timeoutTimer = new System.Windows.Forms.Timer();

        public class UserData
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        public class LoginLogEntry
        {
            public string Username { get; set; }
            public DateTime LoginTime { get; set; }
            public bool Success { get; set; }
        }

        private string GetLogFilePath()
        {
            string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
            string logFilePath = Path.Combine(projectRoot, @"..\..\..\login_log.json");
            return Path.GetFullPath(logFilePath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string logFilePath = GetLogFilePath();

            GroupBox groupBox = new GroupBox { Text = "Login Panel", Width = 500, Height = 320 };
            this.Controls.Add(groupBox);

            Label User = new Label { Text = "Username:", Location = new Point(x, y) };
            groupBox.Controls.Add(User);

            Label Password = new Label { Text = "Password:", Location = new Point(x, y + 30) };
            groupBox.Controls.Add(Password);

            TextBox UserName = new TextBox { Size = new Size(150, 20), Location = new Point(User.Right + 10, y - 3) };
            groupBox.Controls.Add(UserName);

            TextBox PasswordType = new TextBox { Size = new Size(150, 20), PasswordChar = '*', Location = new Point(Password.Right + 10, y + 27) };
            groupBox.Controls.Add(PasswordType);

            CheckBox showPassCheckBox = new CheckBox { Text = "Show Password", Size = new Size(110, 20), Location = new Point(PasswordType.Right + 10, PasswordType.Top) };
            groupBox.Controls.Add(showPassCheckBox);

            showPassCheckBox.CheckedChanged += (s, ev) =>
            {
                PasswordType.PasswordChar = showPassCheckBox.Checked ? '\0' : '*';
            };

            Button LogInBTN = new Button { Text = "Log in", Size = new Size(70, 30), Location = new Point(UserName.Left, PasswordType.Bottom + 15) };
            groupBox.Controls.Add(LogInBTN);

            Button SignUp = new Button { Text = "Sign up", Size = new Size(95, 30), Location = new Point(LogInBTN.Right + 15, LogInBTN.Top) };
            groupBox.Controls.Add(SignUp);

            Button btnShowLog = new Button
            {
                Text = "Show Log",
                Size = new Size(95, 30),
                Location = new Point(SignUp.Right + 15, LogInBTN.Top)
            };
            groupBox.Controls.Add(btnShowLog);

            btnShowLog.Click += (s, ev) =>
            {
                string logFile = GetLogFilePath();
                if (!File.Exists(logFile))
                {
                    MessageBox.Show("No login_log.json found.", "Log");
                    return;
                }

                string logJson = File.ReadAllText(logFile);
                if (string.IsNullOrWhiteSpace(logJson))
                {
                    MessageBox.Show("Log file is empty.", "Log");
                    return;
                }

                List<LoginLogEntry> logs = new List<LoginLogEntry>();
                try
                {
                    logs = JsonSerializer.Deserialize<List<LoginLogEntry>>(logJson);
                }
                catch
                {
                    MessageBox.Show("Corrupted log file.", "Error");
                    return;
                }

                if (logs == null || logs.Count == 0)
                {
                    MessageBox.Show("Log is empty.", "Log");
                    return;
                }

                string display = string.Join(Environment.NewLine, logs.Select(l =>
                    $"{l.Username} logged in at {l.LoginTime:yyyy-MM-dd HH:mm:ss} | Success: {l.Success}"));
                MessageBox.Show(display, "Login Log");
            };

            timeoutTimer.Interval = 10000;
            timeoutTimer.Tick += timeoutTimer_tick;
            timeoutTimer.Start();

            Label Error = new Label { ForeColor = Color.Red, Location = new Point(x, SignUp.Bottom + 10), Size = new Size(450, 25), Text = "" };
            groupBox.Controls.Add(Error);

            UserName.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Enter) { LogInBTN.PerformClick(); ev.SuppressKeyPress = true; } };
            PasswordType.KeyDown += (s, ev) => { if (ev.KeyCode == Keys.Enter) { LogInBTN.PerformClick(); ev.SuppressKeyPress = true; } };

            LogInBTN.Click += (s, ev) =>
            {
                string enteredUser = UserName.Text;
                string enteredPassword = PasswordType.Text;

                try
                {
                    if (!File.Exists("info.json"))
                    {
                        Error.ForeColor = Color.Red;
                        Error.Text = "info.json not found!";
                        return;
                    }

                    string json = File.ReadAllText("info.json");
                    List<UserData> users = JsonSerializer.Deserialize<List<UserData>>(json);

                    var loggedUser = users.FirstOrDefault(u => u.Username == enteredUser && u.Password == enteredPassword);
                    bool success = loggedUser != null;

                    LogLoginAttempt(enteredUser, success, logFilePath);

                    if (success)
                    {
                        timeoutTimer.Stop();
                        MainPage page = new MainPage(loggedUser.Username, loggedUser.Role);
                        page.Show();
                        this.Hide();
                    }
                    else
                    {
                        Error.ForeColor = Color.Red;
                        Error.Text = "Incorrect username or password.";
                        RestartTimer();
                    }
                }
                catch (Exception ex)
                {
                    Error.ForeColor = Color.Red;
                    Error.Text = $"Error: {ex.Message}";
                }
            };

            UserName.TextChanged += (s, ev) => RestartTimer();
            PasswordType.TextChanged += (s, ev) => RestartTimer();
            LogInBTN.MouseMove += (s, ev) => RestartTimer();
            SignUp.MouseMove += (s, ev) => RestartTimer();
            groupBox.MouseMove += (s, ev) => RestartTimer();
        }

        private void LogLoginAttempt(string username, bool success, string logFilePath)
        {
            List<LoginLogEntry> logs = new List<LoginLogEntry>();
            if (File.Exists(logFilePath))
            {
                string existingJson = File.ReadAllText(logFilePath);
                if (!string.IsNullOrWhiteSpace(existingJson))
                {
                    try
                    {
                        logs = JsonSerializer.Deserialize<List<LoginLogEntry>>(existingJson) ?? new List<LoginLogEntry>();
                    }
                    catch
                    {
                        logs = new List<LoginLogEntry>();
                    }
                }
            }

            logs.Add(new LoginLogEntry { Username = username, LoginTime = DateTime.Now, Success = success });
            string updatedJson = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(logFilePath, updatedJson);
        }

        private void RestartTimer() { timeoutTimer.Stop(); timeoutTimer.Start(); }

        private void timeoutTimer_tick(object sender, EventArgs e)
        {
            timeoutTimer.Stop();
            MessageBox.Show("10 seconds passed. Timeout!", "Timeout");
            this.Close();
        }
    }
}
