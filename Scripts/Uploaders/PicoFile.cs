//                                                      Writed By Rextor in 5/10/2016
//                                                            Don't need update


using MSC;
using MSC.Brute;
using System.Collections.Generic;

namespace Leecher.Scripts.Uploaders
{
    class PicoFile
    {
        public static List<Item> GetLinks(string url, string password)
        {
            Config config = new Config();
            Requester rr = new Requester();

            config.KeepAlive = true;
            config.Cookies = "_ga=GA1.2.2039718586.1465731600; _gat=1";
            config.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0";
            string[] arr = url.Split('/');
            bool give = false;
            string id = "";
            string server = "";
            foreach (string str in arr)
            {
                if (str.Contains("picofile.com"))
                {
                    server = str.Substring(0, 2);
                }
                if (give)
                {
                    id = str;
                    break;
                }
                if (str == "file")
                    give = true;
            }
            config.LoginURL = "http://" + server + ".picofile.com/file/GenerateDownloadLink?fileId=" + id;
            config.PostData = "";
            config.Referer = url;
            config.Method = Method.POST;
            if (!string.IsNullOrEmpty(password))
            {
                config.PostData = "password=" + password;
                config.ContectType = "application/x-www-form-urlencoded; charset=UTF-8";
                config.AllowAutoRedirect = true;
            }
            RequestManage link = rr.POSTData(config);
            string Return = link.SourcePage;
            if (link.ErrorAst)
            {
                string[] error = Return.Split('|');
                if (error[1] == "NotFound")
                    Return = "File Not Found";
                if (error[1] == "Forbidden")
                    Return = "Invalid password or somting error";
            }
            return new List<Item> { new Item { Link = Return } };
        }
    }
}
