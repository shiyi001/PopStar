using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PopStar
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private GridPicture gridPicture;
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Hide();
            button2.Hide();
            button3.Hide();
            gridPicture = new GridPicture();
            gridPicture.Column = 10;
            gridPicture.Row = 10;
            gridPicture.HbHeight = 30;
            gridPicture.HbWidth = 30;
            panel.Controls.Add(gridPicture);
        }

        private Introduction introduction; 
        private void button2_Click(object sender, EventArgs e)
        {
            introduction = new Introduction();
            introduction.Column = 1;
            introduction.Row = 1;
            panel.Controls.Add(introduction);
            button1.Hide();
            button2.Hide();
            button3.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
