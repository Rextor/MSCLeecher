//                                                     Writen By Reza-HNA in 2017/06/03
//                                                             Don't Need Update


using MSC;
using MSC.Brute;
using System.Collections.Generic;
using HtmlAgilityPack;


namespace Leecher.Scripts.Uploaders
{
    class RGhost
    {
        public static List<Item> GetLinks(string url)
        {
            Config config = new Config();
            config.LoginURL = url;
            config.Method = Method.GET;
            config.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:52.0) Gecko/20100101 Firefox/54.0";
            config.AllowAutoRedirect = true;
            Requester Rer = new Requester();
            RequestManage link = Rer.GETData(config);

            HtmlDocument DocumentHTML = new HtmlDocument();
            DocumentHTML.LoadHtml(link.SourcePage);
            var nodes = DocumentHTML.DocumentNode.SelectSingleNode("//div[@class='file-info-btns-block m-b-10 m-t-15']");
            string msmsdskj = nodes.InnerHtml.ToString().Replace(@"""","");
            int ind1 = msmsdskj.IndexOf("href=") + "href=".Length;
            int ind2 = msmsdskj.IndexOf(@"class=btn") - @"class=btn".Length;
            string dlLink = msmsdskj.Substring(ind1, ind2);
            nodes = DocumentHTML.DocumentNode.SelectSingleNode("//div[@class='filename']");
            Item itemFin = new Item();
            itemFin.Link = dlLink;
            itemFin.Info = nodes.InnerText.Replace("Б", "B").Replace("\n\n","").Replace(@"("," ").Replace(@")", " ");
            List<Item> Items = new List<Item>();
            Items.Add(itemFin);
            return Items;

        }
    }
}
