using System;
using System.Windows.Forms;

namespace Cyberbot_Part3
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(mainForm: new Chatform());
        }
    }
}