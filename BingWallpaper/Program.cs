using System;
using System.Windows.Forms;

namespace BingWallpaper
{
    internal static class Program
    {
        public const string STARTUP_CMD = "/startup";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == STARTUP_CMD)
            {
                try
                {
                    var loader = new BingLoader();
                    loader.GetBackground();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString(), exception.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var form = new BingWallpaperOptions();
                Application.Run(form);
            }
        }
    }
}