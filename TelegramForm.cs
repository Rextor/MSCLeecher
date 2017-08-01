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
    public partial class TelegramForm : Form
    {
        public TelegramForm()
        {
            InitializeComponent();
        }
        public string token;

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            token = textBox1.Text;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
