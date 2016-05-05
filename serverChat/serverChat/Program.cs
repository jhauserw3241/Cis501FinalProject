using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace serverChat
{
    public delegate void ServerInputHandler(string action, params object[] vars);

    public delegate void ServerOutputHandler(string action, params object[] vars);

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
            ServerView view = new ServerView(data);

            // Load the users from the default file
            ModelDataInteraction.DeserializeUserList(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "ServerUsers.xml");

            // Create connection from view to controller
            view.ConversationsButton.Click += cont.HandleGenericInput;
            view.UsersButton.Click += cont.HandleGenericInput;
            view.ElementListBox.MouseDoubleClick += cont.HandleMouseInput;
            cont.Output += view.HandleFormOutput;

            // Create connection to the websocket
            // Start a websocket server at port 8001
            var wss = new WebSocketServer(8001);

            // Add the Echo websocket service
            wss.AddWebSocketService<Access>("/Access", () => new Access(data));
            wss.AddWebSocketService<Chat>("/Chat", ()=>new Chat(data));

            //WebSocket curWs = new WebSocket("www.example.com");

            // Start the server
            wss.Start();

            // Execute form
            Application.Run(view);

            // Stop the server
            wss.Stop();

            // Save the users to the default file
            ModelDataInteraction.SerializeUserList(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "ServerUsers.xml");
        }
    }
}
