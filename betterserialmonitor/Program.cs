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

        public static string format(this string me, params object[] args)
        {
            return String.Format(me, args);
        }

        public static byte[] replaceZeroes(this byte[] me)
        {
            int N = me.Length;
            byte[] temp = new byte[N];
            
            for(int i = 0; i < N; i++)
            {
                if (me[i] == 0)
                    temp[i] = 0x3F;
                else
                    temp[i] = me[i];
            }

            return temp;
        }
    }
}
