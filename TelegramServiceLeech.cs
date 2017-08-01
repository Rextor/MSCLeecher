using Leecher.Scripts;
using Leecher.Scripts.Films;
using Leecher.Scripts.Uploaders;
using MSC;
using MSC_Leecher.Scripts;
using NetTelegramBotApi;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leecher
{
    public class TelegramServiceLeech
    {
        public string Token;
        public MSC.Logger log = new MSC.Logger();

        public bool stat;

        public async void Run()
        {
            var MainMenu = new ReplyKeyboardMarkup();
            MainMenu.Keyboard = new KeyboardButton[][]
      {
        new KeyboardButton[]
        {
            new KeyboardButton("About Us👍"),

        },

        new KeyboardButton[]
        {
            new KeyboardButton("Supported Site for leeching?")
        }
    };
            TelegramBot Bot = null;
            try {
                Bot = new TelegramBot(Token);
                await Bot.MakeRequestAsync(new GetMe());
                log.AddMessage(string.Format("Bot connected."), Log.Type.OutPut);
            }
            catch { log.AddMessage("Can't connect the bot!", Log.Type.Error); return; }
            long Offset = 0;
            while (stat)
            {
                var Update = await Bot.MakeRequestAsync(new GetUpdates() { Offset = Offset });
                foreach (var Up in Update)
                {
                    try
                    {
                        List<Item> items = new List<Item>();
                        Offset = Up.UpdateId + 1;
                        var Message = Up.Message.Text;
                        string neh = "";
                        if (Message != "")
                        {

                            log.AddMessage(string.Format("Telegram Service: Text: {0}  Username: {1} ChatID: {2}", Message, Up.Message.From.Username, Up.Message.From.Id), MSC.Log.Type.Infomation);
                            if (Message == "About Us👍")
                            {
                                neh += @"
💢Bot             By   : Rextor
💢MSC           By   : Rextor
💢Scryipts      By  :  Rextor && Reza-HNA
💢Account    By   : SkyFall

➖➖➖➖➖➖➖➖➖➖
Powered By MSC | @VIPLeechBot";
                                var msg = new SendMessage(Up.Message.Chat.Id, neh) { ReplyMarkup = MainMenu };
                                await Bot.MakeRequestAsync(msg);
                            }
                            if (Message == "/start")
                            {
                                neh = @"♨️با سلام خدمت کاربر گرامی 
به ربات وی ای پی لیچر (VIP Leecher) خوش امدید 
با استفاده از این ربات میتوانید از سایت های زیر بدون محدودیت دانلود کنید با سرعت زیاد 😃";

                                neh += @"

➖➖➖➖➖➖➖➖➖➖
Powered By MSC | @VIPLeechBot";
                                var msg = new SendMessage(Up.Message.Chat.Id, neh) { ReplyMarkup = MainMenu };
                                await Bot.MakeRequestAsync(msg);
                            }
                            if (Message == "Supported Site for leeching?")
                            {
                                SiteDetecter.TypeSite[] list = Enum.GetValues(typeof(SiteDetecter.TypeSite)).Cast<SiteDetecter.TypeSite>().ToArray();
                                Array.Sort<SiteDetecter.TypeSite>(list);
                                List<string> lfw = new List<string>();
                                foreach (SiteDetecter.TypeSite item in list)
                                    lfw.Add(item.ToString());
                                ArrayList q = new ArrayList();
                                foreach (object o in lfw)
                                    q.Add(o);
                                q.Sort();
                                lfw.Clear();
                                foreach (object o in q)
                                    lfw.Add(o.ToString());
                                neh += @"💢Supported Site for leeching :
";
                                foreach (string gw in lfw)
                                    neh += @"
💢" + gw;


                                neh += @"

➖➖➖➖➖➖➖➖➖➖
Powered By MSC | @VIPLeechBot";
                                var msg = new SendMessage(Up.Message.Chat.Id, neh) { ReplyMarkup = MainMenu };
                                await Bot.MakeRequestAsync(msg);
                            }

                            else
                            {
                                Leecher.Scripts.SiteDetecter.TypeSite typesite = Scripts.SiteDetecter.GetTypeSite(Message);
                                neh = typesite.ToString() + @" Detected!
Please wait...";
                                var msg = new SendMessage(Up.Message.Chat.Id, neh) { ReplyMarkup = MainMenu };
                                await Bot.MakeRequestAsync(msg);
                                string url = Message;

                                items = Core.GetList(url, "", typesite);

                                neh = @"🚀Links:

";
                                foreach (Item i in items)
                                {
                                    neh += @"
➖➖➖➖➖➖➖➖➖➖
📌" + i.Info + @"
📂" + i.Link;
                                }


                                neh += @"

➖➖➖➖➖➖➖➖➖➖
Powered By MSC | @VIPLeechBot";
                                if (items.Count != 0)
                                {
                                    var msg1 = new SendMessage(Up.Message.Chat.Id, neh) { ReplyMarkup = MainMenu };
                                    await Bot.MakeRequestAsync(msg1);
                                }
                            }

                        }
                    }
                    catch(Exception ex) { log.AddMessage(ex.Message, Log.Type.Error); }
                }
            }
        }
    }
}
