using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BingWallpaper
{
    public partial class BingWallpaperOptions : Form
    {
        private const string STARTUP_ENTRY = "BingWallpaper";
        private RegistryKey startupRegistry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private bool startupRegistered;

        public BingLoader BingLoader { get; }

        public BingWallpaperOptions()
        {
            InitializeComponent();
            BingLoader = new BingLoader();
            updateStartupButton();
        }

        public void GetWallpaper_Click(object sender, EventArgs e)
        {
            try
            {
                BingLoader.GetBackground();
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.ToString());
                MessageBox.Show(exception.ToString(), exception.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void startupButton_Click(object sender, EventArgs e)
        {
            if (startupRegistered)
            {
                startupRegistry.SetValue(STARTUP_ENTRY, Application.ExecutablePath.ToString() + " " + Program.STARTUP_CMD);
            }
            else
            {
                startupRegistry.DeleteValue(STARTUP_ENTRY, false);
            }
            updateStartupButton();
        }

        private void updateStartupButton()
        {
            if (startupRegistered = startupRegistry.GetValue(STARTUP_ENTRY) == null)
            {
                button1.Text = "Register startup";
            }
            else
            {
                button1.Text = "Deregister startup";
            }
        }
    }
}