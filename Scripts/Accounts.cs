using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSC.Brute;
using Leecher.Scripts;

namespace Leecher
{
    public class Accounts
    {
        public class UserAccount
        {
            public Account account { set; get; }
            public object TypeSite { set; get; }
            public override string ToString()
            {
                return TypeSite.ToString() + "=" + account.ToString();
            }
        }

        public static List<UserAccount> list = new List<UserAccount>()
        {
            new UserAccount {account=new Account {Username= "Email" ,Password= "Password" }, TypeSite= SiteDetecter.TypeSite.Xnxx },
        };
        public static Account GetAccount(object type)
        {
            List<Account> lis = new List<Account>();
            foreach (UserAccount sit in list)
            {
                if ((SiteDetecter.TypeSite)sit.TypeSite == (SiteDetecter.TypeSite)type)
                    lis.Add(sit.account);
            }
            Random ra = new Random();
            return lis[ra.Next(lis.Count)];
        }
    }

}
