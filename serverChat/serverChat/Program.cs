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
            // Create objects
            ServerModel data = new ServerModel();
            ServerController cont = new ServerController(data);
            ServerView form = new ServerView(data);

            // Create/display form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(form);

            // Create connection from view to controller
            cont.output += form.HandleFormOutput;
        }
    }
}
