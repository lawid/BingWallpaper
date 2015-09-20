using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

public sealed class Wallpaper
{
    Wallpaper() { }

    const int SPI_SETDESKWALLPAPER = 20;
    const int SPIF_UPDATEINIFILE = 0x01;
    const int SPIF_SENDWININICHANGE = 0x02;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    public enum Style : int
    {
        Tiled,
        Centered,
        Stretched
    }

    public static void Set(string path, Style style)
    {
        //System.IO.Stream s = new System.Net.WebClient().OpenRead(uri.ToString());
        //Stream s2 = new FileStream()

        //Image img = Image.FromStream(s);

        //using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        //{
        //    using (Image original = Image.FromStream(fs))
        //    {
        //        original.open

        //        //Image img = Image.FromFile(fileName);
        //        //string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");
        //        //img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        string path = Path.GetFullPath(fileName);

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                if (style == Style.Stretched)
                {
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                if (style == Style.Centered)
                {
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }

                if (style == Style.Tiled)
                {
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                }

                SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0,
                    path,
                    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        //    }
        //}
    }
}