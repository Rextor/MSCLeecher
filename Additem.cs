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
    public partial class Additem : Form
    {
        public Additem()
        {
            InitializeComponent();
        }
        public Item im = new Item();
        private void button1_Click(object sender, EventArgs e)
        {
            im.Info = textBox1.Text;
            if (textBox2.Text != null)
            {
                im.Link = textBox2.Text;
            }
            else { MessageBox.Show("Please enter a link!", "Enter link", MessageBoxButtons.OK, MessageBoxIcon.Error);  }
            if (im.Link != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
