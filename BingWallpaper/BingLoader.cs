using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
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
        public const string STREAM_URL = "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US";
        

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
            string jsonStream = WebClient.DownloadString(STREAM_URL);

            Console.WriteLine("Parsing stream");
            JObject stream = JObject.Parse(jsonStream);
            JArray images = JArray.Parse(stream["images"].ToString());

            string imgUrl = null;
            foreach (JObject image in images)
            {
                imgUrl = image["url"].ToString();
                break; // only one image for now
            }
            
            Uri imgUri = new Uri(BING_URL + imgUrl);
            Console.WriteLine(imgUrl);

            // get extension
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
            // Propagate wallpaper set
            if(OnWallpaperSet != null) OnWallpaperSet();
        }                
    }
}
