using MSC;
using MSC.Brute;
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
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
        }
        private void ErrorProxy(string input)
        {
            MessageBox.Show(input, "Proxy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            proxy = null;
        }
        public static Proxy proxy
        {
            set; get;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text != "")
                {
                    proxy.Username = textBox2.Text;
                    if (textBox3.Text != "")
                        proxy.Password = textBox3.Text;
                    else { ErrorProxy("Fill Password field!"); return; }
                }
                var value = int.Parse(numericUpDown1.Value.ToString());
                proxy.Port = value;
                if (textBox1.Text != "")
                    proxy.Ip = textBox1.Text;
                else { ErrorProxy("Enter ip"); return; }
                ProxyService.proxy = proxy;
                MessageBox.Show("Proxy Seted!", "Proxy Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch { ErrorProxy("Fill valid fields"); return; }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            proxy = null;
            ProxyService.proxy = proxy;
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            proxy = ProxyService.proxy;
            if (proxy != null)
            {
                textBox1.Text = proxy.Ip;
                numericUpDown1.Value = (decimal)proxy.Port;
                textBox2.Text = proxy.Username;
                textBox3.Text = proxy.Password;
                checkBox1.Checked = ProxyService.UseInAllRequest;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ProxyService.UseInAllRequest = checkBox1.Checked;
        }
    }
}
