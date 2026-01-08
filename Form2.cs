namespace WinFormsApp2;

public class Form2:Form
{
    private NotifyIcon trayIcon;
    private ContextMenuStrip trayMenu;

    private string adapterName = "Ethernet";

    public Form2()
    {
        InitializeComponent();
        CreateTrayIcon();
        this.WindowState = FormWindowState.Minimized;
        this.ShowInTaskbar = false;
    }


}