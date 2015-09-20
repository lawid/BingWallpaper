using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingWallpaper
{
    public class BingLoader
    {
        public const string BING_URL = "http://www.bing.com";
        public const string WALLPAPER_VARIABLE_DECLARATION = "g_img={url:'";

        public WebClient WebClient { get; }

        public delegate void WallpaperSetHandler();
        public event WallpaperSetHandler OnWallpaperSet;

        // Field to hold fileName while downloading
        private string path;

        public BingLoader()
        {
            WebClient = new WebClient();
            WebClient.Proxy = WebProxy.GetDefaultProxy();
            WebClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

        }

        public void GetBackground()
        {

            Console.WriteLine("Requesting site");
            string html = WebClient.DownloadString(BING_URL);

            Console.WriteLine("parsing site");
            string imgUrl = ParseImageUrl(html);
            Uri imgUri = new Uri(BING_URL + imgUrl);
            Console.WriteLine(imgUrl);

            int fileExtensionPoint = imgUrl.LastIndexOf('.') + 1;
            string fileExtension = imgUrl.Substring(fileExtensionPoint);
            string fileName = "bing_background." + fileExtension;
            
            path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), fileName);

            Console.WriteLine(path);

            WebClient.DownloadFileAsync(imgUri, fileName);
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            bool exists = File.Exists(path);
            Console.WriteLine("File exists " + exists);
            if (exists) Wallpaper.Set(path, Wallpaper.Style.Stretched);
            if(OnWallpaperSet != null) OnWallpaperSet();
        }

        public string ParseImageUrl(string html)
        {
            // Parse html
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection scriptTags = doc.DocumentNode.SelectNodes("//script");

            foreach (HtmlNode scriptNode in scriptTags)
            {
                string script = scriptNode.InnerText;
                try
                {
                    int gImgStart = script.IndexOf(WALLPAPER_VARIABLE_DECLARATION);
                    int imgStart = script.IndexOf('\'', gImgStart) + 1;
                    int imgEnd = script.IndexOf('\'', imgStart);

                    string url = script.Substring(imgStart, imgEnd - imgStart);
                    return url;
                }
                catch (Exception)
                {
                    Console.WriteLine("g_img not found in this script");
                }
            }

            return null;
        }
    }
}
