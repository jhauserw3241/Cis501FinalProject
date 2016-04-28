using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serverChat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Application setup
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create objects
            ServerModel data = new ServerModel();
            ServerController cont = new ServerController(data);
            ServerView form = new ServerView();

            // Create connection from view to controller
            cont.output += form.HandleFormOutput;

            // Execute form
            Application.Run(form);
        }
    }
}
