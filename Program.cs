using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace IPTray
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

                private string adapterName = "Ethernet";

        public Form1()
        {
            InitializeComponent();
            CreateTrayIcon();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        private void CreateTrayIcon()
        {
            // kontextové menu
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Nastavit IP 192.168.0.51", null, SetStaticIP_Click);
            trayMenu.Items.Add("Přepnout na DHCP", null, SetDHCP_Click);
            trayMenu.Items.Add(new ToolStripSeparator());
            trayMenu.Items.Add("Ukončit", null, Exit_Click);

            // ikona v oznamovací oblasti
            trayIcon = new NotifyIcon();
            trayIcon.Text = "IP Switcher";
            trayIcon.Icon = SystemIcons.Application;
            trayIcon.Visible = true;
            trayIcon.ContextMenuStrip = trayMenu;
        }

        // statická IP 192.168.0.51
        private void SetStaticIP_Click(object sender, EventArgs e)
        {
            string cmd = 
                $"netsh interface ip set address \"{adapterName}\" static 192.168.0.51 255.255.255.0 192.168.0.1";

            RunCmd(cmd);
            trayIcon.ShowBalloonTip(2000, "IP Switcher", "IP nastavena na 192.168.0.51", ToolTipIcon.Info);
        }

        // DHCP
        private void SetDHCP_Click(object sender, EventArgs e)
        {
            string cmd1 = $"netsh interface ip set address \"{adapterName}\" dhcp";
            string cmd2 = $"netsh interface ip set dns \"{adapterName}\" dhcp";

            RunCmd(cmd1);
            RunCmd(cmd2);

            trayIcon.ShowBalloonTip(2000, "IP Switcher", "Adaptér přepnut na DHCP", ToolTipIcon.Info);
        }

        // netsh
        private void RunCmd(string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/C " + command,
                Verb = "runas",             // Spustí jako administrátor
                UseShellExecute = true,
                CreateNoWindow = true
            };

            Process.Start(psi);
        }

        // uzavření
        private void Exit_Click(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            trayIcon.Visible = false;
            base.OnFormClosing(e);
        }
    }
}
