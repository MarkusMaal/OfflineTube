using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OfflineTube
{
    static class Program
    {
        public static bool diceroll = false;
        public static bool closeall = false;
        public static bool recommend = false;
        public static bool playthis = false;
        public static int playrec = 0;
        public static VideoBrowser vb;
        private static Form1 f1;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetCompatibleTextRenderingDefault(false);
            vb = new VideoBrowser();
            f1 = new Form1();
            Application.EnableVisualStyles();
            if (args.Length > 0)
            {
                f1.textBox1.Text = string.Join(" ", args);
            }
            Application.Run(f1) ;
        }
    }
}
