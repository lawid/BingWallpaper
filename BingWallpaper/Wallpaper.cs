using Microsoft.Win32;
using System.Runtime.InteropServices;

public sealed class Wallpaper
{
    private Wallpaper()
    { }

    private const int SPI_SETDESKWALLPAPER = 20;
    private const int SPIF_UPDATEINIFILE = 0x01;
    private const int SPIF_SENDWININICHANGE = 0x02;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

    public enum Style : int
    {
        Tiled,
        Centered,
        Stretched
    }

    public static void Set(string path, Style style)
    {
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
    }
}