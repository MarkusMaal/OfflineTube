using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OfflineTube
{
    public partial class DescView : Form
    {
        internal string text = "";
        public DescView()
        {
            InitializeComponent();
        }

        private void DescView_Load(object sender, EventArgs e)
        {
            textBox1.Text = text.Replace("\n", Environment.NewLine);
            textBox1.Select(0, 0);
        }
    }
}
