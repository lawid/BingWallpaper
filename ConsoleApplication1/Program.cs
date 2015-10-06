using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleApplication1
{
    class Program
    {
        static WebClient web { get; set; }

        static void Main(string[] args)
        {
            web = new WebClient();
            web.Proxy = WebProxy.GetDefaultProxy();

            string contents = web.DownloadString("http://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US");

            JObject stream = JObject.Parse(contents);
            JArray images = JArray.Parse(stream["images"].ToString());

            
            
            foreach(JObject image in images)
            {
                Console.WriteLine(image["url"].ToString());
                
                break; // just one img for now
            }

            //Console.WriteLine(stream["images"]);
            Console.ReadKey();
        }
    }
}
