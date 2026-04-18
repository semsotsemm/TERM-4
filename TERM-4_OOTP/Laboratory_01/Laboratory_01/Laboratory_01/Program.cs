using System;
using System.Windows.Forms;

namespace Laboratory_01
{
    static class Program
    {
        [STAThread] 
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
        }
    }
}