using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;

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

            // Create connection to the websocket
            // Start a websocket server at port 8001
            var ws = new WebSocketServer(8001);

            // Add the Echo websocket service
            ws.AddWebSocketService<ServerController>("/", ()=>new ServerController(data));

            // Start the server
            ws.Start();

            // Execute form
            Application.Run(form);

            // Stop the server
            ws.Stop();
        }
    }
}
