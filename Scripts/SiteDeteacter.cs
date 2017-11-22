using System;
using System.Linq;

namespace Leecher.Scripts
{
    class SiteDetecter
    {
        public static TypeSite GetTypeSite(string url)
        {
            TypeSite ReturnDef = TypeSite.Unknown;
            try
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                    url = "http://" + url;
                Uri myUri = new Uri(url);
                string[] AllDomain = myUri.Host.Split('.');
                foreach (var item in AllDomain)
                {
                    var LeechersList = Enum.GetValues(typeof(TypeSite)).Cast<TypeSite>().ToList();
                    foreach (TypeSite LeechItem in LeechersList)
                        if (item.ToLower().Contains(LeechItem.ToString().ToLower()) || LeechItem.ToString().ToLower().Contains(item.ToLower()))
                        {
                            ReturnDef = LeechItem;
                            break;
                        }
                }
            }
            catch { }
            return ReturnDef;
        }

        public enum TypeSite
        {
            //NightMovie,
            //TinyMoviez,
            //IranFilm,
            //BaranMovie,
            //Youtube,
            //Vimeo,
            //Imovie_dl,
            Instagram,
            PicoFile,
            //ZippyShare,
            //UploadBoy,
            //UptoBox,
            //SendSpace,
            //Aparat,
            //Brazzers,
            Xnxx,
            //Tinyz,          // TinyMoviez  
            //srvdl2,         // Imovie_dl
            RGhost,
            //Opizo,
            //Tamasha,
            //Varzesh3,
            //Cafebazaar,
            //Namava,
            //MediaFire,
            Dropbox,
            //Filimo,
            Unknown
        }
    }
}
