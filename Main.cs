using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Leecher.Scripts;
using Leecher.Scripts.Films;
using Leecher.Scripts.Uploaders;
using MSC;
using System.Drawing;
using System.Collections;
using MSC.Brute;
using Leecher;
using System.Text;
using System.Threading.Tasks;

namespace MSC_Leecher
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Accounts.LoadAccounts(Application.StartupPath + @"\Accounts.txt");
            SiteDetecter.TypeSite[] list = Enum.GetValues(typeof(SiteDetecter.TypeSite)).Cast<SiteDetecter.TypeSite>().ToArray();
            Array.Sort<SiteDetecter.TypeSite>(list);
            foreach (SiteDetecter.TypeSite item in list)
                listBox1.Items.Add(item.ToString());
            ArrayList q = new ArrayList();
            foreach (object o in listBox1.Items)
                q.Add(o);
            q.Sort();
            listBox1.Items.Clear();
            foreach (object o in q)
                listBox1.Items.Add(o);

            foreach (Accounts.UserAccount o in Accounts.AccountList)
                richTextBox2.Text += o.ToString() + "\n";
            tsl.log = new Logger();
            tsl.log.OnMessageReceived += Logger_OnMessageReceived;
            logger = new Logger();
            logger.OnMessageReceived += Logger_OnMessageReceived;
        }

        private void AppendText( string text, Color color)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;

            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(text + "\n");
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
        }

        private void Logger_OnMessageReceived(object sender, MessageReceivedArge e)
        {
            Color col = Color.Black;
            switch (e.log.typeT)
            {
                case Log.Type.Error:
                    col = Color.Red;
                    break;
                case Log.Type.Infomation:
                    col = Color.Blue;
                    break;
                case Log.Type.OutPut:
                    col = Color.Green;
                    break;
            }
            AppendText(e.log.GetMessage(checkBox2.Checked), col);
        }

        SiteDetecter.TypeSite typesite;
        Logger logger;

        public void Doing()
        {
            try {
                logger.Clean();
                string url = textBox1.Text;
                logger.AddMessage("Leeching " + url, Log.Type.Infomation);
                textBox1.Text = "";
                string pass = textBox2.Text;
                typesite = SiteDetecter.GetTypeSite(url);
                logger.AddMessage("Detected Site : " + typesite.ToString(), Log.Type.Infomation);
                Leech(url, pass);
            }
            catch(Exception ex)
            {
                logger.AddMessage("ERROR: " + ex.Message, Log.Type.Error);
            }
            logger.AddMessage("End of Request...", Log.Type.Infomation);
            button1.Enabled = true;
        }
        List<Item> items = new List<Item>();

        public string Yon(string url)
        {
            Config config = new Config();
            MSC.Brute.Requester Rer = new MSC.Brute.Requester();
            config.LoginURL = "http://yon.ir";
            MSC.Brute.RequestManage login = Rer.GETData(config);
            config.Cookies = login.CookiesString;
            config.LoginURL = "http://yon.ir/app/shorten.php";
            config.KeepAlive = true;
            config.Method = Method.POST;
            config.Referer = "http://yon.ir/";
            config.ContectType = "application/x-www-form-urlencoded";
            config.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:51.0) Gecko/20100101 Firefox/51.0";
            config.PostData = "url=" + url + "&wish=";
            MSC.Brute.RequestManage manage = Rer.POSTData(config, login); ;
            string pattern = @"output"":""(.*?)""";
            return "http://yon.ir/" + System.Text.RegularExpressions.Regex.Match(manage.SourcePage, pattern).Groups[1].Value.ToString();
        }
        public void Leech(string url, string pass)
        {
            items.Clear();

            if (typesite != SiteDetecter.TypeSite.Unknown)
            {
                ProsCase(url, pass);
            }
            else
            {
                string reirected = tryLocation(url);
                DialogResult Dr = MessageBox.Show("Can't detecte site!\nUse Unknown script to leech?\nElse press No button to RedirectLink\nLink: " + reirected, "Error!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (Dr == DialogResult.No)
                {
                    url = reirected;
                    typesite = SiteDetecter.GetTypeSite(url);
                    logger.AddMessage("Leeching " + url, Log.Type.Infomation);
                    typesite = SiteDetecter.GetTypeSite(url);
                    logger.AddMessage("Detected Site : " + typesite.ToString(), Log.Type.Infomation);
                    Leech(url, pass);
                }
                else if (Dr == DialogResult.Yes)
                {
                    items.Add(Unknown.GetLinks(url));
                    PrintItems(items);
                }
                else
                    logger.AddMessage("Can't detecte site!", Log.Type.Error);
            }
        }

        public void ProsCase(string url, string pass)
        {
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
                case SiteDetecter.TypeSite._30Nama:
                    items = _30Nama.GetLinks(url);
                    break;
            }
            
            PrintItems(items);
        }

        private static string tryLocation(string url)
        {
            return Unknown.GetLinks(url).Link;
        }

        public void PrintItems(List<Item> item)
        {
            foreach(Item i in item)
            {
                ListViewItem II;
                II = listView1.Items.Add(i.Info);
                if (checkBox1.Checked)
                {
                        try
                        {
                            if (i.Link.StartsWith("http://") || i.Link.StartsWith("https://"))
                                i.Link = Yon(i.Link);
                        }
                        catch { }
                    
                }
                II.SubItems.Add(i.Link);

                logger.AddMessage("Leeched Link: " + i.Info + " \n" + i.Link, Log.Type.OutPut);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(new ThreadStart(Doing));
            th.IsBackground = true;
            th.Start();
            button1.Enabled = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            logger.Clean();
            richTextBox1.Text = "";
            listView1.Items.Clear();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Visible = checkBox4.Checked;
            
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Visible = checkBox5.Checked;
            if (!checkBox5.Checked)
                richTextBox1.Dock = DockStyle.Fill;
            else richTextBox1.Dock = DockStyle.Left;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = Clipboard.GetText();
        }

        private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                System.Diagnostics.Process.Start(listView1.SelectedItems[0].SubItems[1].Text);
            }
            catch { }     
        }

        private void copyLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                Clipboard.SetText(listView1.SelectedItems[0].SubItems[1].Text);
            }
            catch { }
        }

        private void copyInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                Clipboard.SetText(listView1.SelectedItems[0].SubItems[0].Text);
            }
            catch { }
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            listView1.Columns[0].Width = listView1.Width / listView1.Columns.Count;
            listView1.Columns[1].Width = listView1.Width / listView1.Columns.Count;
        }

        private void downloadWhitIDMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\DownloadManager") != null)
            {
                try {
                    Microsoft.Win32.RegistryKey r = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\DownloadManager");

                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = r.GetValue("ExePath").ToString();
                    string argum = "/d " + @"""" + listView1.SelectedItems[0].SubItems[1].Text + @"""";
                    p.StartInfo.Arguments = argum;
                    p.Start();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("You Haven't Internet Download Manager!" + Environment.NewLine + "Use Link Button For Get Download Link.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        bool closemenu = true;
        int hold = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (closemenu)
            {
                hold += 5;
                panel4.Size = new Size(hold, 265);
                if (hold >= 155)
                {
                    closemenu = false;
                    timer1.Enabled = false;
                    button2.Enabled = true;
                }
            }
            else
            {
                hold -= 5;
                panel4.Size = new Size(hold, 265);
                if (hold == 15)
                {
                    closemenu = true;
                    timer1.Enabled = false;
                    button2.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (closemenu)
            {
                button2.Text = "<";
                button2.Enabled = false;
                timer1.Enabled = true;
            }
            else
            {
                button2.Text = ">";
                button2.Enabled = false;
                timer1.Enabled = true;
            }
        }
        TelegramServiceLeech tsl = new TelegramServiceLeech();

        public static string PlayLink { get; internal set; }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!tsl.stat)
            {
                TelegramForm ft = new TelegramForm();
                if (ft.ShowDialog() == DialogResult.OK)
                {
                    tsl.Token = ft.token;
                    Task.Run(() => tsl.Run());
                    button4.Text = "Stop Service";
                    tsl.stat = true;
                }
            }
            else
            {
                tsl.stat = false;
                logger.AddMessage("Bot Disconnected.", Log.Type.Infomation);
                button4.Text = "Start Telegram Bot";
            }
        }

        private void copyInfoLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(listView1.SelectedItems[0].SubItems[0].Text + "\n" + listView1.SelectedItems[0].SubItems[1].Text);
            }
            catch { }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string PLink = listView1.SelectedItems[0].SubItems[1].Text;

                if (PlayLink != "")
                {
                    PlayLink = PLink;
                    MediaPlayer mpl = new MediaPlayer(null);
                    mpl.Show();
                }
                else MessageBox.Show("Play Link Is Empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch
            {

            }
        }

        private void addAllToPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayLink = null;
            List<Item> it = new List<Item>();
            foreach(ListViewItem lvi in listView1.Items)
            {
                it.Add(new Item { Info = lvi.SubItems[0].Text, Link = lvi.SubItems[1].Text });
            }
            MediaPlayer mpl = new MediaPlayer(it);
            mpl.Show();
        }

        private void telegramBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Setting st = new Setting();
            st.ShowDialog();
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                string neh = "";
                foreach (ListViewItem lvi in listView1.Items)
                {
                    neh += lvi.SubItems[0].Text + " " + lvi.SubItems[1].Text + "\n";
                }
                Clipboard.SetText(neh);
            }
            catch { }
        }
    }
}
