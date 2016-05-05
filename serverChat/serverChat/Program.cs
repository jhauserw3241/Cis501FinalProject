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
    public delegate void SendMsgToClient(string msg);
    public delegate void SendMsgToServer(string id, string msg);

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
            ServerModel data = ServerModel.Instance;
            GuiController cont = new GuiController();
            ServerView view = new ServerView();
            ServerSocket sevSoc = new ServerSocket();
            Chat c = new Chat();
            Access a = new Access();

            // Load the users from the default file
            ModelDataInteraction.DeserializeUserList(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "ServerUsers.xml");

            // Create connection from view to controller
            view.ConversationsButton.Click += cont.HandleGenericInput;
            view.UsersButton.Click += cont.HandleGenericInput;
            view.ElementListBox.MouseDoubleClick += cont.HandleMouseInput;
            cont.Output += view.HandleFormOutput;

            // Create connection from handlers to server socket
            a.sendMsgServer += sevSoc.TransmitMsg;
            c.sendMsgServer += sevSoc.TransmitMsg;
            
            // Create connection to the websocket
            sevSoc.Start();

            // Execute form
            Application.Run(view);

            // End connection to the websocket
            sevSoc.Stop();

            // Save the users to the default file
            ModelDataInteraction.SerializeUserList(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "ServerUsers.xml");
        }
    }
}
