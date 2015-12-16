using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace BingWallpaper
{
    public class BingLoader
    {
        public const string BING_URL = "http://www.bing.com";
        public const string STREAM_URL = "http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US";

        public WebClient WebClient { get; }
        
        public BingLoader()
        {
            WebClient = new WebClient();
            WebClient.Proxy = WebProxy.GetDefaultProxy();
        }

        public void GetBackground()
        {
            Trace.TraceInformation("Requesting site");
            string jsonStream = WebClient.DownloadString(STREAM_URL);

            Trace.TraceInformation("Parsing stream");
            JObject stream = JObject.Parse(jsonStream);
            JArray images = JArray.Parse(stream["images"].ToString());

            string imgUrl = null;
            foreach (JObject image in images)
            {
                imgUrl = image["url"].ToString();
                break; // only one image for now
            }

            Uri imgUri = new Uri(BING_URL + imgUrl);

            int lastSlash = imgUrl.LastIndexOf('/') + 1;
            string fileName = imgUrl.Substring(lastSlash);

            var path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), fileName);
            
            WebClient.DownloadFile(imgUri, path);
        
            bool exists = File.Exists(path);
            Trace.TraceInformation("File: "+path+" exists? " + exists);

            if (exists) Wallpaper.Set(path, Wallpaper.Style.Stretched);
        }
    }
}