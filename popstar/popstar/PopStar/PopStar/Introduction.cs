using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PopStar
{
    public partial class Introduction : UserControl
    {
        public Introduction()
        {
            InitializeComponent();
        }

        public int Column { get; set; }
        public int Row { get; set; }

        private Introduction introduction;
        private void button1_Click(object sender, EventArgs e)
        {
            introduction.Hide();
        }
    }
}
