using MSC;
using MSC.Brute;
using System.Collections.Generic;

namespace Leecher.Scripts.Uploaders
{
    class Unknown
    {
        public static Item GetLinks(string url)
        {
            Config config = new Config();
            Requester Rr = new Requester();
            config.LoginURL = url;
            config.Method = Method.GET;
            config.AllowAutoRedirect = false;
            config.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:51.0) Gecko/20100101 Firefox/51.0";
            RequestManage get = Rr.GETData(config);
            Item item = new Item();
            item.Link = get.Location;
            return item;
        }
    }
}
