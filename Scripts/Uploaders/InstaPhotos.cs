//                                                      Writed By Rextor in 15/10/2016
//                                                             Don't Need Update

using System.Text.RegularExpressions;

using MSC;
using MSC.Brute;
using System.Collections.Generic;

namespace Leecher.Scripts.Uploaders
{
    class InstaPhotos
    {
        public static List<Item> GetLinks(string url)
        {
            Config config = new Config();
            Requester rer = new Requester();

            config.LoginURL = url;
            config.KeepAlive = true;
            config.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";
            config.Method = Method.GET;
            RequestManage manage = rer.GETData(config);

            string pattern = @"<meta property=""og:image"" content=""(.*?)""";
            Match m = Regex.Match(manage.SourcePage, pattern);
            string link = m.Groups[1].ToString();
            return new List<Item> { new Item { Link = link } };
        }
    }
}
