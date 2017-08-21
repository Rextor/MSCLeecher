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
    public partial class QuestionBox : Form
    {
        
        public QuestionBox()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        public IQuestionBox box { set; get; }
        private void QuestionBox_Load(object sender, EventArgs e)
        {
            label3.Text = box.Title;
            label1.Text = box.Body;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            box.Answer = textBox1.Text;
            Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(label1.Text);
        }
    }
}
