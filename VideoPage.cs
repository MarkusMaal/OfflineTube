using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace OfflineTube
{
    public partial class VideoPage : Form
    {
        internal string repository = "";
        internal string simplename = "";
        internal string fullname = "";
        internal string recommendation = "";
        internal int selection = 0;
        private string description = "";
        private string title = "";
        private string[] recommended_videos = { };
        private int ticks = 0;
        private bool initialtest = true;
        public VideoPage()
        {
            InitializeComponent();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (!timer2.Enabled)
            {
                timer1.Enabled = true;
                pictureBox1.Visible = false;
                axWindowsMediaPlayer1.Visible = true;
                trackBar1.Visible = true;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VideoPage_Load(object sender, EventArgs e)
        {
            int timeMin = DateTime.Now.Hour;
            int timeSec = DateTime.Now.Minute;
            string stimeMin = timeMin.ToString();
            string stimeSec = timeSec.ToString();
            if (timeMin < 10) { stimeMin = "0" + stimeMin; }
            if (timeSec < 10) { stimeSec = "0" + stimeSec; }
            if (!label9.Text.Contains(':'))
            {
                label9.Text = stimeMin + ":" + stimeSec;
            }
            else
            {
                label9.Text = stimeMin + " " + stimeSec;
            }
            if (File.Exists(repository + "\\" + simplename + " (BQ).jpg"))
            { 
                pictureBox1.BackgroundImage = Image.FromFile(repository + "\\" + simplename + " (BQ).jpg");
            }
            if (File.Exists(repository + "\\" + simplename + " (HQ).jpg"))
            {
                pictureBox1.BackgroundImage = Image.FromFile(repository + "\\" + simplename + " (HQ).jpg");
            }

            if (File.Exists(repository + "\\" + simplename + " (Description).txt"))
            {
                textBox1.Text = File.ReadAllText(repository + "\\" + simplename + " (Description).txt").Replace("\n", Environment.NewLine);
            }
            else
            {
                textBox1.Text = "(this video has no description)";
            }
            textBox1.Select(0, 0);
            axWindowsMediaPlayer1.URL = repository + "\\" + fullname + ".mp4";
            //label2.Text = axWindowsMediaPlayer1.currentMedia.getItemInfo("Title");
            //string[] moreconfusion = repository.Split('\\');
            //label3.Text = "Created by: " + axWindowsMediaPlayer1.currentMedia.getItemInfo("Artist") + " (from " + moreconfusion[moreconfusion.Length - 1] + ")";
            axWindowsMediaPlayer1.Ctlcontrols.play();
            axWindowsMediaPlayer1.settings.volume = 0;
            description = textBox1.Text;
            title = label2.Text;
            timer2.Enabled = true;
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void AxWindowsMediaPlayer1_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                return;
            }
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                return;
            }
        }

        void CollectTags()
        {
            for (int x = 0; x < 15; x++)
            {
                int suglength = description.Replace(Environment.NewLine, " ").Split(' ').Count();
                for (int i = 0; i < 10; i++)
                {
                    if (!((description.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].Contains("http://") || (description.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].Contains("\"")) || (description.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].Contains("_") || (description.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].Contains("#") || (description.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].Contains("https://")) || (description.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].Contains(":")))) || (recommendation.Contains(description.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].ToLower())))))
                    {
                        if (description.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].ToLower().Length > 4)
                        {
                            recommendation += description.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].ToLower() + " ";
                        }
                        Thread.Sleep(5);
                    }
                    Thread.Sleep(5);
                }
                suglength = title.Replace(Environment.NewLine, " ").Split(' ').Count();
                for (int i = 0; i < 3; i++)
                {
                    if (!recommendation.Contains(title.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].ToLower()))
                    {
                        if (title.Split(' ')[new Random().Next(0, suglength - 1)].ToLower().Length > 4)
                        {
                            recommendation += title.Replace(Environment.NewLine, " ").Split(' ')[new Random().Next(0, suglength - 1)].ToLower() + " ";
                            Thread.Sleep(5);
                        }
                    }
                    Thread.Sleep(5);
                }
            }
            description += Environment.NewLine + Environment.NewLine + "Keywords (added by OfflineTube): " + Environment.NewLine;
            foreach (string element in recommendation.Split(' '))
            {
                description += element + ", ";
            }
            Program.vb.Keywords = this.recommendation;
            Program.vb.button9.PerformClick();
            this.recommended_videos = Program.vb.listBox2.Items.OfType<string>().ToArray();
            Thread.CurrentThread.Abort();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                trackBar1.Maximum = Convert.ToInt32(axWindowsMediaPlayer1.currentMedia.duration);
                try
                {
                    trackBar1.Value = Convert.ToInt32(axWindowsMediaPlayer1.Ctlcontrols.currentPosition);
                }
                catch { }
                if (axWindowsMediaPlayer1.currentMedia.getItemInfo("Title") != "")
                {
                    label2.Text = axWindowsMediaPlayer1.currentMedia.getItemInfo("Title");
                }
                else
                {
                    label2.Text = simplename;
                }
                string[] moreconfusion = repository.Split('\\');
                if (axWindowsMediaPlayer1.currentMedia.getItemInfo("Artist") != "")
                {
                    label3.Text = "Created by: " + axWindowsMediaPlayer1.currentMedia.getItemInfo("Artist");
                    if (timer2.Interval != 100) { timer2.Interval = 100; }
                }
                else
                {
                    label3.Text = "Repository: " + moreconfusion[moreconfusion.Length - 1];
                }
                if (!timer2.Enabled)
                {
                    double percentage = Math.Round((Convert.ToDouble(trackBar1.Value) / Convert.ToDouble(trackBar1.Maximum)) * 100.0, 2);
                    label5.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString + "/" + axWindowsMediaPlayer1.currentMedia.durationString + " (" + percentage.ToString() + "%)";
                }
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                if (!timer2.Enabled)
                {
                    label5.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString + "/" + axWindowsMediaPlayer1.currentMedia.durationString + " (paused)";
                }
            }
            else if ((axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded) || (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped))
            {
                axWindowsMediaPlayer1.Visible = false;
                pictureBox1.Visible = true;
                trackBar1.Visible = false;
                timer1.Enabled = false;
                button4.Enabled = true;

                ThreadStart ts = new ThreadStart(CollectTags);
                Thread t = new Thread(ts);
                t.Start();
            }


    }

        private void TrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = trackBar1.Value;
            timer1.Enabled = true;
        }

        private void TrackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false;
        }

        private void VideoPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Visible = true;
            timer1.Enabled = false;
            axWindowsMediaPlayer1.Dispose();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            try
            { 
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                axWindowsMediaPlayer1.settings.volume = 100;
                timer2.Enabled = false;
                label5.Text = "Click on the thumbnail to play the video";
                pictureBox1.Image = Properties.Resources.playbutton;
                pictureBox1.Cursor = Cursors.Hand;
                button3.Enabled = true;
                button2.Enabled = true;
                button1.Enabled = true;
            } catch
            {

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Program.diceroll = true;
            this.Close();
        }

        private void AxWindowsMediaPlayer1_MouseMoveEvent(object sender, AxWMPLib._WMPOCXEvents_MouseMoveEvent e)
        {
            this.ticks = 0;
            if (!timeTicker.Enabled) { timeTicker.Enabled = true; }
            trackBar1.Visible = true;
        }

        private void TrackBar1_MouseMove(object sender, MouseEventArgs e)
        {
            this.ticks = 0;
        }

        private void TimeTicker_Tick(object sender, EventArgs e)
        {
            this.ticks += 1;
            if (ticks > 10)
            {
                trackBar1.Visible = false;
            }
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                int seconds = trackBar1.Value;
                int minutes = 0;
                int hours = 0;
                while (seconds >= 60)
                {
                    seconds -= 60;
                    minutes += 1;
                }
                while (minutes >= 60)
                {
                    minutes -= 60;
                    hours += 1;
                }
                string secondstring = seconds.ToString();
                string minutestring = minutes.ToString();
                string hourstring = hours.ToString();
                if (secondstring.Length == 1) { secondstring = "0" + secondstring; }
                if (minutestring.Length == 1) { minutestring = "0" + minutestring; }
                if (hourstring.Length == 1) { hourstring = "0" + hourstring; }
                if (hourstring == "00")
                {
                    label5.Text = minutestring + ":" + secondstring + "/" + axWindowsMediaPlayer1.currentMedia.durationString + " (seeking)";
                }
                else
                {
                    label5.Text = hourstring + ":" + minutestring + ":" + secondstring + "/" + axWindowsMediaPlayer1.currentMedia.durationString + " (seeking)";
                }
            }
        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.uiMode = "full";
            axWindowsMediaPlayer1.fullScreen = true;
        }

        private void AxWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void VideoPage_Activated(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.uiMode = "none";
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Program.recommend = true;
            this.Close();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            string rawname = "";
            string[] moreconfusion = Program.vb.listBox1.Items[vScrollBar1.Value].ToString().Split('(');
            for (int i = 0; i < moreconfusion.Length - 1; i++)
            {
                rawname += moreconfusion[i] + "(";
            }
            rawname = rawname.Substring(0, rawname.Length - 2);
            if (File.Exists(repository + "\\" + rawname + " (BQ).jpg"))
            {
                pictureBox2.Image = Image.FromFile(repository + "\\" + rawname + " (BQ).jpg");
            }
            else if (File.Exists(repository + "\\" + rawname + " (HQ).jpg"))
            {
                pictureBox2.Image = Image.FromFile(repository + "\\" + rawname + " (HQ).jpg");
            }
            else
            {
                pictureBox2.Image = null;
            }
            label7.Text = recommended_videos[vScrollBar1.Value].Replace("* ", "").Replace("** ", "").Replace("*** ", "");
            try
            {
                Program.vb.listBox2.SelectedIndex = vScrollBar1.Value + 1;
                rawname = "";
                moreconfusion = Program.vb.listBox1.Items[vScrollBar1.Value + 1].ToString().Split('(');
                for (int i = 0; i < moreconfusion.Length - 1; i++)
                {
                    rawname += moreconfusion[i] + "(";
                }
                rawname = rawname.Substring(0, rawname.Length - 2);
                if (File.Exists(repository + "\\" + rawname + " (BQ).jpg"))
                {
                    pictureBox3.Image = Image.FromFile(repository + "\\" + rawname + " (BQ).jpg");
                }
                else if (File.Exists(repository + "\\" + rawname + " (HQ).jpg"))
                {
                    pictureBox3.Image = Image.FromFile(repository + "\\" + rawname + " (HQ).jpg");
                }
                else
                {
                    pictureBox3.Image = null;
                }
                label8.Text = recommended_videos[vScrollBar1.Value + 1].Replace("* ", "").Replace("** ", "").Replace("*** ", "");
            }
            catch
            {
                pictureBox3.Image = null;
                label8.Text = "(you have reached the end)";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Program.vb.listBox2.SelectedIndex = vScrollBar1.Value;
            Program.playthis = true;
            Program.playrec = vScrollBar1.Value;
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                foreach (string s in Program.vb.listBox2.Items)
                {
                    if (s.Contains(label8.Text))
                    {
                        break;
                    }
                    i++;
                }
                Program.vb.listBox2.SelectedIndex = i;
                Program.playrec = i;
                Program.playthis = true;
                this.Close();
            }
            catch {
            
            }
        }

        private void ConstTime_Tick(object sender, EventArgs e)
        {
            int timeMin = DateTime.Now.Hour;
            int timeSec = DateTime.Now.Minute;
            string stimeMin = timeMin.ToString();
            string stimeSec = timeSec.ToString();
            if (timeMin < 10) { stimeMin = "0" + stimeMin; }
            if (timeSec < 10) { stimeSec = "0" + stimeSec; }
            if (!label9.Text.Contains(':'))
            {
                label9.Text = stimeMin + ":" + stimeSec;
            }
            else
            {
                label9.Text = stimeMin + " " + stimeSec;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

            if (description != textBox1.Text)
            {
                textBox1.Text = description;
                vScrollBar1.Maximum = recommended_videos.Length - 1;

                Program.vb.listBox2.SelectedIndex = vScrollBar1.Value;
                string rawname = "";
                string[] moreconfusion = Program.vb.listBox1.Items[vScrollBar1.Value].ToString().Split('(');
                for (int i = 0; i < moreconfusion.Length - 1; i++)
                {
                    rawname += moreconfusion[i] + "(";
                }
                rawname = rawname.Substring(0, rawname.Length - 2);
                if (File.Exists(repository + "\\" + rawname + " (BQ).jpg"))
                {
                    pictureBox2.Image = Image.FromFile(repository + "\\" + rawname + " (BQ).jpg");
                }
                else if (File.Exists(repository + "\\" + rawname + " (HQ).jpg"))
                {
                    pictureBox2.Image = Image.FromFile(repository + "\\" + rawname + " (HQ).jpg");
                }
                else
                {
                    pictureBox2.Image = null;
                }
                label7.Text = recommended_videos[vScrollBar1.Value].Replace("* ", "").Replace("** ", "").Replace("*** ", "");
                try
                {
                    Program.vb.listBox2.SelectedIndex = vScrollBar1.Value + 1;
                    rawname = "";
                    moreconfusion = Program.vb.listBox1.Items[vScrollBar1.Value + 1].ToString().Split('(');
                    for (int i = 0; i < moreconfusion.Length - 1; i++)
                    {
                        rawname += moreconfusion[i] + "(";
                    }
                    rawname = rawname.Substring(0, rawname.Length - 2);
                    if (File.Exists(repository + "\\" + rawname + " (BQ).jpg"))
                    {
                        pictureBox3.Image = Image.FromFile(repository + "\\" + rawname + " (BQ).jpg");
                    }
                    else if (File.Exists(repository + "\\" + rawname + " (HQ).jpg"))
                    {
                        pictureBox3.Image = Image.FromFile(repository + "\\" + rawname + " (HQ).jpg");
                    }
                    else
                    {
                        pictureBox3.Image = null;
                    }
                    label8.Text = recommended_videos[vScrollBar1.Value + 1].Replace("* ", "").Replace("** ", "").Replace("*** ", "");
                }
                catch
                {
                    pictureBox3.Image.Dispose();
                    pictureBox3.Image = null;
                    label8.Text = "(you have reached the end)";
                }
                label5.Text = "Click on the thumbnail to play the video";
                progressBar1.Visible = false;
                vScrollBar1.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                pictureBox2.Visible = true;
                pictureBox3.Visible = true;
                timer3.Enabled = false;
            }
        }
    }
}
