using Leecher.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSC_Leecher
{
    public partial class MediaPlayer : Form
    {
        int ib = 1;

        public MediaPlayer(List<Item> items)
        {
            InitializeComponent();
            if (items != null)
            {
                if (items.Count != 0)
                {
                    foreach (Item i in items)
                    {
                        ListViewItem II;

                        II = listView1.Items.Add(ib++.ToString());

                        II.SubItems.Add(i.Info);
                        II.SubItems.Add(i.Link);
                    }
                }
            }
        }
        
        private void MediaPlayer_Load(object sender, EventArgs e)
        {
            if (Main.PlayLink != null)
            {
                Play(Main.PlayLink);
                listView1.Visible = false;
            }
        }

        private void Play(string url)
        {
            label1.Text = "URL: " + url;
            Player.stretchToFit = true;
            Player.URL = url;
            Player.currentPlaylist.appendItem(Player.newMedia(url));
            Player.Ctlcontrols.play();
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Additem ai = new Additem();
            if(ai.ShowDialog() == DialogResult.OK)
            {
                ListViewItem II;

                II = listView1.Items.Add(ib++.ToString());

                II.SubItems.Add(ai.im.Info);
                II.SubItems.Add(ai.im.Link);
            }
        }

        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Play(listView1.SelectedItems[0].SubItems[2].Text);
            }
            catch { }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Play(listView1.SelectedItems[0].SubItems[2].Text);
            }
            catch { }
        }
    }
}
