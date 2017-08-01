//                                                      Writed By Rextor in 4/3/2017
//                                             For update just login to site and past all cookies


using System.Collections.Generic;
using MSC;
using MSC.Brute;

namespace Leecher.Scripts.Films
{
    class Xnxx
    {
        public static List<Item> GetLinks(string url)
        {
            List<Item> items = new List<Item>();
            Config config = new Config();
            Requester Rer = new Requester();

            Account ac = Accounts.GetAccount(SiteDetecter.TypeSite.Xnxx);

            config.LoginURL = url;
            config.DataSet = "<USER>*<PASS>";
            config.Method = Method.GET;
            config.AllowAutoRedirect = true;
            config.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:52.0) Gecko/20100101 Firefox/52.0";
            config.KeepAlive = true;
            config.ContectType = "application/x-www-form-urlencoded; charset=UTF-8";

            RequestManage so = Rer.GETData(config);
            config.LoginURL = "http://www.xnxx.com/account/login/rpc";
            
            config.Referer = url;
            
            config.Method = Method.POST;

            string pst = "login=<USER>&password=<PASS>";
            config.PostData = Rer.ReplaceAccount(ac, pst, config);

            RequestManage manage = Rer.POSTData(config,so);

            config.Method = Method.GET;
            config.LoginURL = url;
            config.Referer = "http://www.xnxx.com/";

            Token tok = new Token();
            tok.RegexPattern = @"flashvars=""id_video=(.*?)&amp;";
            tok = Rer.GetToken(tok, config, manage);

            config.Referer = url;
            config.LoginURL = "http://www.xnxx.com/video-download/" + tok.GrpValue[0];
            config.Cookies = manage.CookiesString;

            Token lib = new Token();
            lib.RegexPattern= @"{""LOGGED"":true,""URL"":""(.*?)"",""URL_LOW"":""(.*?)""}";
            lib = Rer.GetToken(lib, config);

            Item high = new Item();
            high.Info = "HIGH QUALITY";
            high.Link = lib.GrpValue[0].Replace("\\/","/");

            Item low = new Item();
            low.Info = "LOW QUALITY";
            low.Link = lib.GrpValue[1].Replace("\\/", "/"); ;

            items.Add(low);
            items.Add(high);

            return items;
        }
    }
}
//http://www.xnxx.com/video-cz6ln73/we_wont_judge_you_for_being_a_sissy