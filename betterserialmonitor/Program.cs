using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BetterSerialMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static string[] Partition(this string str, int n)
        {
            string[] res = new string[str.Length / 2 + str.Length % 2];
            int strIndex = 0;
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = str.Substring(strIndex, strIndex+n > str.Length ? 1 : 2);
                strIndex += 2;
            }

            return res;
        }
    }
}
