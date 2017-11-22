using HtmlAgilityPack;
using MSC;
using MSC.Brute;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Leecher.Scripts.Films
{
    class _30Nama
    {
        const string _30namaserver = "http://30nama.site/";
        public static List<Item> MovieLeech(HtmlDocument HD, RequestManage manage)
        {
            List<Item> items = new List<Item>();
            List<string> list = new List<string>()
            {
              @"<div class=""dl-box"" data-quality=""(.*?)"">",
            };
            MatchCollection coll = Regex.Matches(manage.SourcePage, list[0]);
            for (int i = 1; i < coll.Count + 1; i++)
            {
                string Info, Link;
                Info = HD.DocumentNode.SelectSingleNode(@"//*[@id=""dl-container""]/div[" + i.ToString() + "]").InnerText;
                Info = Info.Replace("\n", " ").Replace("\r", "").Replace("\t", "").Replace("  ", "").Replace("زيرنويس", "").Replace("دانلود", "");

                string pattern = @"<a href=""(.*?)""><i class=""fa fa-fw fa-download""><\/i> دانلود<\/a>";
                string pack = HD.DocumentNode.SelectSingleNode(@"//*[@id=""dl-container""]/div[" + i.ToString() + "]").InnerHtml;
                Link = Regex.Match(pack, pattern).Groups[1].Value;

                Item item = new Item { Link = Link, Info = Info };
                items.Add(item);
            }
            return items;
        }
        public static List<Item> SeriesLeech(HtmlDocument HD, RequestManage manage)
        {
            //Lazy :)
            //TODO: Series leech codes
            return null;
        }
        public static List<Item> GetLinks(string url)
        {
            PageTypes typepage = PageTypes.Unkown;
            string[] arr = url.Split('/');
            bool give = false;
            string id = "";
            foreach (string str in arr)
            {
                if (give)
                {
                    id = str;
                    break;
                }
                if (str.Split('.')[0] == "30nama")
                    give = true;
            }
            switch (id)
            {
                case "series":
                    typepage = PageTypes.Series;
                    break;
                case "movies":
                    typepage = PageTypes.Movie;
                    break;
            }

            Account ac = Accounts.GetAccount(SiteDetecter.TypeSite._30Nama);

            Config config = new Config();
            Requester Rer = new Requester();

            config.LoginURL = _30namaserver + "login";

            RequestManage login = Rer.GETData(config);
            config.Cookies = login.CookiesString;
            string pst = "log=<USER>&pwd=<PASS>&rememberme=forever&wp-submit=%D9%88%D8%B1%D9%88%D8%AF&redirect_to=http%3A%2F%2F30nama.site%2Fwp-admin%2F&instance=&action=login";
            config.DataSet = "<USER>*<PASS>";
            config.PostData = Rer.ReplaceAccount(ac, pst, config);
            config.AllowAutoRedirect = true;
            config.KeepAlive = true;
            config.ContectType = "application/x-www-form-urlencoded";
            config.Referer = config.LoginURL;
            login = Rer.POSTData(config, login);

            config.LoginURL = url;
            config.Cookies = "";
            RequestManage manage = Rer.GETData(config, login);


            HtmlDocument HD = new HtmlDocument();
            HD.LoadHtml(manage.SourcePage);

            switch (typepage)
            {
                case PageTypes.Movie:
                    return MovieLeech(HD, manage);
                case PageTypes.Series:
                    return SeriesLeech(HD, manage);
            }

            return null;
        }
    }
}
