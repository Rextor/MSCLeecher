//                                                      Writed By RezaHNA in 4/27/2017
//                                                           don't need update
//                                                        Download link from page

using System.Collections.Generic;

namespace Leecher.Scripts.Uploaders
{
    class Dropbox
    {
        public static List<Item> GetLinks(string url)
        {
            Item item = new Item();
            string[] StrSplit = url.Split('/');
            string FileName = StrSplit[StrSplit.Length - 1].Replace("?dl=0", "");
            item.Info = FileName;
            item.Link = url.Replace("https://www.dropbox.com/", "https://dl.dropboxusercontent.com/");
            return new List<Item> { item };
        }
    }
}
