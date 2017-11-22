using System;
using System.Collections.Generic;
using System.Linq;
using MSC.Brute;
using Leecher.Scripts;
using System.IO;

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

        public static void LoadAccounts(string patch)
        {
            if (!File.Exists(patch))
                return;

            List<string> lines = File.ReadAllLines(patch).ToList();
            SiteDetecter.TypeSite[] list = Enum.GetValues(typeof(SiteDetecter.TypeSite)).Cast<SiteDetecter.TypeSite>().ToArray();
            foreach(string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    break;

                string[] arr = line.Split(':');

                if (arr.Length != 3)
                    break;

                foreach(SiteDetecter.TypeSite tup in list)
                {
                    string m = arr[2];
                    if (m.ToLower() == tup.ToString().ToLower() || tup.ToString().ToLower().Contains(m.ToLower()))
                    {
                        AccountList.Add(new UserAccount { account = new Account { Username = arr[0], Password = arr[1] }, TypeSite = tup });
                        break;
                    }
                }
            }
        }

        public static List<UserAccount> AccountList = new List<UserAccount>()
        {

        };

        public static Account GetAccount(object type)
        {
            List<Account> lis = new List<Account>();
            foreach (UserAccount sit in AccountList)
            {
                if ((SiteDetecter.TypeSite)sit.TypeSite == (SiteDetecter.TypeSite)type)
                    lis.Add(sit.account);
            }
            Random ra = new Random();
            return lis[ra.Next(lis.Count)];
        }
    }

}
