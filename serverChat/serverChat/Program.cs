using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace serverChat
{
    public delegate void GuiInputHandler(string action, params object[] vars);
    public delegate void GuiOutputHandler(string action, params object[] vars);
    public delegate void ChatHandler(string id, string message);
    public delegate void AccessHandler(string id, string message);

    public enum STATUS { Online, Away, Offline };

    static class Program
    {
        // Main
        //
        // Create server for chat and show view
        [STAThread]
        static void Main()
        {
            // Application setup
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create objects
            ServerModel data = new ServerModel();
            GuiController cont = new GuiController();
            ServerView view = new ServerView(data);
            ServerSocket sevSoc = new ServerSocket();
            Chat c = new Chat();
            Access a = new Access();

            // Create connection from view to controller
            view.ConversationsButton.Click += cont.HandleGenericInput;
            view.UsersButton.Click += cont.HandleGenericInput;
            view.ElementListBox.MouseDoubleClick += cont.HandleMouseInput;
            cont.Output += view.HandleFormOutput;

            // Create connection from message handling to message output
            c.Input += sevSoc.Transmit;
            sevSoc.Output += 

            // Create connection to the websocket
            sevSoc.Start(data);

            // Execute form
            Application.Run(view);

            // End connection to the websocket
            sevSoc.Stop();
        }
    }
}
