using System;
using System.Windows.Forms;
using BatManager.Forms;

namespace BatManager
{
    internal static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione Windows Forms.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
