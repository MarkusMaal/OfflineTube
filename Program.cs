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
        public static VideoBrowser vb = new VideoBrowser();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()) ;
        }
    }
}
