using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingWallpaper
{
    public partial class BingWallpaperOptions : Form
    {
        private const string STARTUP_ENTRY = "BingWallpaper";
        RegistryKey startupRegistry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        bool startupRegistered;

        public BingLoader BingLoader { get; }

        public BingWallpaperOptions()
        {
            InitializeComponent();
            BingLoader = new BingLoader();
            updateStartupButton();
            BingLoader.WebClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;            
        }



        public void GetWallpaper_Click(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            try
            {
                BingLoader.GetBackground();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.Value = e.ProgressPercentage;
        }

        private void startupButton_Click(object sender, EventArgs e)
        {
            if (startupRegistered)
            {
                startupRegistry.SetValue(STARTUP_ENTRY, Application.ExecutablePath.ToString()+" "+Program.STARTUP_CMD);
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
