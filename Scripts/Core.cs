using Leecher.Scripts;
using Leecher.Scripts.Films;
using Leecher.Scripts.Uploaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSC_Leecher.Scripts
{
    class Core
    {
        public static List<Item> GetList(string url, string pass, SiteDetecter.TypeSite typesite)
        {
            List<Item> items = new List<Item>();
            switch (typesite)
            {     
                case SiteDetecter.TypeSite.Instagram:
                    items = InstaPhotos.GetLinks(url);
                    break;
                case SiteDetecter.TypeSite.PicoFile:
                    items = PicoFile.GetLinks(url, pass);
                    break;
                case SiteDetecter.TypeSite.Xnxx:
                    items = Xnxx.GetLinks(url);
                    break;
                case SiteDetecter.TypeSite.Dropbox:
                    items = Dropbox.GetLinks(url);
                    break;
                case SiteDetecter.TypeSite.RGhost:
                    items = RGhost.GetLinks(url);
                    break;
            }
            return items;
        }
    }
}
