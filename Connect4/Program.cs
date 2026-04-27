using System;
using System.Windows.Forms;

namespace Ex05
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new UI.FormGameSettings());
        }
    }
}