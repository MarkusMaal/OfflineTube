using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace OfflineTube
{
    public partial class VideoBrowser : Form
    {
        bool threadrunning = false;
        internal string repository = "";
        public string Keywords = "";
        private bool closethis = true;
        private string excludedtitle = "";
        private string repo_root = "";
        List<string> videos = new List<string>();
        List<string> frontvideos = new List<string>();
        List<string> subrepos = new List<string>();
        string reponame = "Repository";
        public VideoBrowser()
        {
            InitializeComponent();
        }

        private void VideoBrowser_Load(object sender, EventArgs e)
        {
            repo_root = repository;
            RePopulate();
        }

        void RePopulate()
        {
            threadrunning = true;
            populationTimer.Enabled = true;
            ThreadStart ts = new ThreadStart(populate);
            Thread t = new Thread(ts);
            t.Start();
        }

        void populate()
        {
            videos.Clear();
            frontvideos.Clear();
            subrepos.Clear();
            string[] moreconfusion = repository.Split('\\');
            reponame = moreconfusion[moreconfusion.Length - 1];
            foreach (string fi in Directory.GetFiles(repository))
            {
                if (fi.Replace(repository + "\\", "").EndsWith(".mp4"))
                {
                    videos.Add(fi.Replace(repository + "\\", "").Replace(".mp4", ""));
                }
            }
            foreach (DirectoryInfo di in new DirectoryInfo(repo_root).GetDirectories())
            {
                subrepos.Add(di.Name);
            }
            frontvideos.AddRange(videos);
            for (int i = 0; i < frontvideos.Count; i++)
            {
                int aa = frontvideos[i].ToString().Split('(').Count() - 1;
                frontvideos[i] = frontvideos[i].ToString().Replace("(" + frontvideos[i].ToString().Split('(')[aa], "");
            }
            threadrunning = false;
            Thread.CurrentThread.Abort();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                button7.Enabled = true;
                string rawname = "";
                string[] moreconfusion = listBox1.SelectedItem.ToString().Split('(');
                for (int i = 0; i < moreconfusion.Length - 1; i++)
                {
                    rawname += moreconfusion[i] + "(";
                }
                rawname = rawname.Substring(0, rawname.Length - 2);
                if (File.Exists(repository + "\\" + rawname + " (BQ).jpg"))
                { 
                    pictureBox1.Image = Image.FromFile(repository + "\\" + rawname + " (BQ).jpg");
                }
                else if (File.Exists(repository + "\\" + rawname + " (HQ).jpg"))
                {
                    pictureBox1.Image = Image.FromFile(repository + "\\" + rawname + " (HQ).jpg");
                }
                else
                {
                    pictureBox1.Image = null;
                }
                if (File.Exists(repository + "\\" + rawname + " (Description).txt"))
                {
                    label7.Text = File.ReadAllText(repository + "\\" + rawname + " (Description).txt").Split('\n')[0];
                }
                else
                {
                    label7.Text = "(this video has no description)";
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                Process.Start(repository + "\\" + listBox1.SelectedItem.ToString() + ".mp4");
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            closethis = false;
            this.Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {

                string rawname = "";
                string[] moreconfusion = listBox1.SelectedItem.ToString().Split('(');
                for (int i = 0; i < moreconfusion.Length - 1; i++)
                {
                    rawname += moreconfusion[i] + "(";
                }
                rawname = rawname.Substring(0, rawname.Length - 2);
                if (File.Exists(repository + "\\" + rawname + " (BQ).jpg"))
                {
                    Process.Start(repository + "\\" + rawname + " (BQ).jpg");
                }
                else if (File.Exists(repository + "\\" + rawname + " (HQ).jpg"))
                {
                    Process.Start(repository + "\\" + rawname + " (HQ).jpg");
                }
                else
                {
                    MessageBox.Show("This video has no thumbnail to display", "Cannot display thumbnail image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Process.Start(repository + "\\");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            { 
                string rawname = "";
                string[] moreconfusion = listBox1.SelectedItem.ToString().Split('(');
                for (int i = 0; i < moreconfusion.Length - 1; i++)
                {
                    rawname += moreconfusion[i] + "(";
                }
                rawname = rawname.Substring(0, rawname.Length - 2);
                VideoPage vp = new VideoPage();
                excludedtitle = listBox1.SelectedItem.ToString();
                vp.repository = repository;
                vp.simplename = rawname;
                vp.fullname = listBox1.SelectedItem.ToString();
                if (this.Visible) { this.Hide(); }
                Program.vb.listBox2.Items.Clear();
                Program.vb.listBox1.Items.Clear();
                Program.vb.listBox2.Items.AddRange(this.listBox2.Items);
                Program.vb.listBox1.Items.AddRange(this.listBox1.Items);
                vp.ShowDialog();
                button9.Enabled = true;
                this.Keywords = vp.recommendation;
                this.Show();
                if (Program.playthis)
                {
                    Program.playthis = false;
                    listBox1.Items.Clear();
                    listBox1.Items.AddRange(Program.vb.listBox1.Items);
                    listBox2.Items.Clear();
                    listBox2.Items.AddRange(Program.vb.listBox1.Items);
                    listBox2.SelectedIndex = Program.playrec;
                    button1.PerformClick();
                }
                button7.PerformClick();
                if (Program.diceroll == true)
                {
                    Program.diceroll = false;
                    button6.PerformClick();
                }
                if (Program.recommend)
                {
                    button9.PerformClick();
                }
            }
            else
            {
                MessageBox.Show("Please select a video", "Cannot show the video page", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                button1.PerformClick();
            }
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            listBox1.SelectedIndex = rnd.Next(0, listBox1.Items.Count - 1);
            button1.PerformClick();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (button7.Enabled == true) { listBox1.Items.Clear(); }
            listBox2.Items.Clear();
            RePopulate();
            textBox1.Text = "";
            button7.Enabled = false;
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter (a) keyword(s) to perform a search!", "Search field left blank", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            listBox1.Items.Clear();
            foreach (string fi in Directory.GetFiles(repository))
            {
                if (fi.Replace(repository + "\\", "").EndsWith(".mp4"))
                {
                    listBox1.Items.Add(fi.Replace(repository + "\\", "").Replace(".mp4", ""));
                }
            }
            button7.Enabled = false;
            ListBox results = new ListBox();
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string rawname = "";
                string teststring = "";
                string[] moreconfusion = listBox1.Items[i].ToString().Split('(');
                for (int x = 0; x < moreconfusion.Length - 1; x++)
                {
                    rawname += moreconfusion[x] + "(";
                }
                rawname = rawname.Substring(0, rawname.Length - 2);
                if (File.Exists(repository + "\\" + rawname + " (Description).txt"))
                {
                    teststring = File.ReadAllText(repository + "\\" + rawname + " (Description).txt").ToUpper();
                }
                if (teststring.Length > 0) { 
                    if ((teststring.Replace(textBox1.Text.ToUpper(), "") != teststring) || (listBox1.Items[i].ToString().ToUpper().Replace(textBox1.Text.ToUpper(), "") != listBox1.Items[i].ToString().ToUpper()))
                    {
                        results.Items.Add(listBox1.Items[i]);
                    }
                }
            }
            listBox1.Items.Clear();
            listBox1.Items.AddRange(results.Items);
            listBox2.Items.Clear();
            listBox2.Items.AddRange(listBox1.Items);
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                int aa = listBox2.Items[i].ToString().Split('(').Count() - 1;
                listBox2.Items[i] = listBox2.Items[i].ToString().Replace("(" + listBox2.Items[i].ToString().Split('(')[aa], "");
            }
            button7.Enabled = true;
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button8.PerformClick();
            }
        }

        private void VideoBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closethis)
            { 
                Program.closeall = true;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = new Random().Next(100, 1500);
            if (label2.ForeColor == Color.Blue)
            {
                label1.ForeColor = Color.Blue;
                label2.ForeColor = Color.White;
                return;
            }
            else
            {
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.Blue;
                return;
            }
        }

        private void Label7_DoubleClick(object sender, EventArgs e)
        {

            string rawname = "";
            string[] moreconfusion = listBox1.SelectedItem.ToString().Split('(');
            for (int i = 0; i < moreconfusion.Length - 1; i++)
            {
                rawname += moreconfusion[i] + "(";
            }
            rawname = rawname.Substring(0, rawname.Length - 2);
            if (File.Exists(repository + "\\" + rawname + " (Description).txt"))
            {
                DescView dv = new DescView();
                dv.text = File.ReadAllText(repository + "\\" + rawname + " (Description).txt");
                dv.Text = listBox2.SelectedItem.ToString();
                dv.ShowDialog();
            }
        }

        private void Label7_Click(object sender, EventArgs e)
        {

        }

        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = listBox2.SelectedIndex;
        }

        private void ListBox2_DoubleClick(object sender, EventArgs e)
        {
            button1.PerformClick();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (string fi in Directory.GetFiles(repository))
            {
                if (fi.Replace(repository + "\\", "").EndsWith(".mp4"))
                {
                    listBox1.Items.Add(fi.Replace(repository + "\\", "").Replace(".mp4", ""));
                }
            }
            button7.Enabled = false;
            ListBox results = new ListBox();
            ListBox midresults = new ListBox();
            ListBox smallresults = new ListBox();
            ListBox bigresults = new ListBox();
            int maxhits = 0;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string rawname = "";
                string teststring = "";
                string[] moreconfusion = listBox1.Items[i].ToString().Split('(');
                for (int x = 0; x < moreconfusion.Length - 1; x++)
                {
                    rawname += moreconfusion[x] + "(";
                }
                rawname = rawname.Substring(0, rawname.Length - 2);
                if (File.Exists(repository + "\\" + rawname + " (Description).txt"))
                {
                    teststring = File.ReadAllText(repository + "\\" + rawname + " (Description).txt").ToUpper();
                }
                if (teststring.Length > 0)
                {
                    string tester = Keywords;
                    int hits = 0;
                    foreach (string element in teststring.Split(' '))
                    {
                        if (tester.Length == 0)
                        {
                            break;
                        }
                        else
                        {
                            if (element != "")
                            { 
                                if (tester != tester.Replace(element.ToLower(), ""))
                                {
                                    hits += 1;
                                    if (hits > maxhits) { maxhits = hits; }
                                }
                                tester = tester.Replace(element.ToLower(), "");

                            }
                        }
                    }
                    ListBox hitlist = new ListBox();
                    for (int y = 0; y < hits; y++)
                    {
                        hitlist.Items.Add(y);
                    }
                    if (tester != Keywords)
                    {
                        if (new Random().Next(0, maxhits) <= hitlist.Items.Count)
                        {
                            if (hits >= 18)
                            {
                                if (!(tester == ""))
                                {
                                    if (listBox1.Items[i].ToString() != excludedtitle)
                                    {
                                        bigresults.Items.Add(listBox1.Items[i]);
                                        System.Threading.Thread.Sleep(10);
                                    }
                                }
                            }
                            if (hits >= 15 && hits < 18)
                            {
                                if (!(tester == ""))
                                {
                                    if (listBox1.Items[i].ToString() != excludedtitle)
                                    { 
                                        results.Items.Add(listBox1.Items[i]);
                                        System.Threading.Thread.Sleep(10);
                                    }
                                }
                            }
                            if (hits > 6 && hits <= 14)
                            {
                                if (!(tester == ""))
                                {
                                    if (listBox1.Items[i].ToString() != excludedtitle)
                                    {
                                        midresults.Items.Add(listBox1.Items[i]);
                                        System.Threading.Thread.Sleep(10);
                                    }
                                }
                            }
                            if (hits <= 6)
                            {
                                if (hits > 3) { 
                                    if (!(tester == ""))
                                    {
                                        if (listBox1.Items[i].ToString() != excludedtitle)
                                        {
                                            smallresults.Items.Add(listBox1.Items[i]);
                                            System.Threading.Thread.Sleep(10);
                                        }
                                    }
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(5);
                    }
                }
            }
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox1.Items.AddRange(bigresults.Items);
            foreach (string item in bigresults.Items)
            {
                listBox2.Items.Add("*** " + item);
            }
            listBox1.Items.AddRange(results.Items);
            foreach (string item in results.Items)
            {
                listBox2.Items.Add("** " + item);
            }
            listBox1.Items.AddRange(midresults.Items);
            foreach (string item in midresults.Items)
            {
                listBox2.Items.Add("* " + item);
            }
            listBox1.Items.AddRange(smallresults.Items);
            listBox2.Items.AddRange(smallresults.Items);
            if (listBox2.Items.Count > 1)
            {
                if (bigresults.Items.Count > 0)
                {
                    listBox2.SelectedIndex = new Random().Next(0, bigresults.Items.Count - 1);
                }
                else if (results.Items.Count > 0)
                { 
                    listBox2.SelectedIndex = new Random().Next(0, results.Items.Count - 1);
                }
                else if (midresults.Items.Count > 0)
                {
                    listBox2.SelectedIndex = new Random().Next(0, midresults.Items.Count - 1);
                }
                else if (smallresults.Items.Count > 0)
                {
                    listBox2.SelectedIndex = new Random().Next(0, smallresults.Items.Count - 1);
                }
            }
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                int aa = listBox2.Items[i].ToString().Split('(').Count() - 1;
                listBox2.Items[i] = listBox2.Items[i].ToString().Replace("(" + listBox2.Items[i].ToString().Split('(')[aa], "");
            }
            button7.Enabled = true;
            if (Program.recommend == true)
            {
                Program.recommend = false;
                button1.PerformClick();
            }
        }

        private void VideoBrowser_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
            } else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }

        private void populationTimer_Tick(object sender, EventArgs e)
        {
            if (!threadrunning)
            {
                listBox1.Items.AddRange(videos.ToArray());
                listBox2.Items.AddRange(frontvideos.ToArray());
                listBox3.Items.AddRange(subrepos.ToArray());
                label3.Text = reponame;
                populationTimer.Enabled = false;
            }
        }

        private void listBox3_DoubleClick(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndices.Count > 0)
            {
                repository = repo_root + "\\" + listBox3.Items[listBox3.SelectedIndex];
                RePopulate();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
            }
        }
    }
}
