// Program.cs
using System;
using System.Windows.Forms;
using Prison_Escape_Game.View.Forms;
using PrisonEscapeGame.View.Forms;

namespace PrisonEscapeGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenuForm());
        }
    }
}