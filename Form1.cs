using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OfflineTube
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) { 
                if (!Directory.Exists(Environment.SpecialFolder.ApplicationData.ToString()))
                {
                    Directory.CreateDirectory(Environment.SpecialFolder.ApplicationData.ToString());
                }
                if (File.Exists(Environment.SpecialFolder.ApplicationData.ToString() + "\\road.txt"))
                {
                    File.Delete(Environment.SpecialFolder.ApplicationData.ToString() + "\\road.txt");
                }
                File.WriteAllText(Environment.SpecialFolder.ApplicationData.ToString() + "\\road.txt", textBox1.Text);
            }
            this.DialogResult = DialogResult.OK;
            if (Directory.Exists(textBox1.Text))
            { 
                VideoBrowser vb = new VideoBrowser();
                this.Hide();
                vb.repository = textBox1.Text;
                vb.ShowDialog();
                if (Program.closeall)
                {
                    this.Close();
                    return;
                }
                this.Show();
            }
            else
            {
                MessageBox.Show("Repository does not exist", "Cannot continue", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                button2.PerformClick();
                return;
            }
            if (File.Exists(Environment.SpecialFolder.ApplicationData.ToString() + "\\road.txt"))
            {
                textBox1.Text = File.ReadAllText(Environment.SpecialFolder.ApplicationData.ToString() + "\\road.txt");
                button2.PerformClick();
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            DataObject data = (DataObject)e.Data;
            if (data.ContainsFileDropList())
            {
                string[] rawFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                if ((rawFiles != null) && (rawFiles.Length == 1))
                {
                    textBox1.Text = rawFiles[0];
                    button2.PerformClick();
                }
            }
        }
    }
}
