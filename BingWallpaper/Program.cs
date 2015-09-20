using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingWallpaper
{
    static class Program
    {
        public const string STARTUP_CMD = "/startup";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new BingWallpaperOptions();            
            
            if (args.Length > 0 && args[0] == STARTUP_CMD)
            {
                form.BingLoader.OnWallpaperSet += StartupExitListener;
                form.GetWallpaper_Click(null, null);
            }
            Application.Run(form);
        }

        private static void StartupExitListener()
        {
            Application.Exit();
        }
    }
}
