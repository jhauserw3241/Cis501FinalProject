﻿using System;
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
        public static ServerModel ServerModel
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public static ServerController ServerController
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

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
            ServerController cont = new ServerController(data);
            ServerView view = new ServerView(data);

            // Create connection from view to controller
            view.ConversationsButton.Click += cont.HandleGenericInput;
            view.UsersButton.Click += cont.HandleGenericInput;
            view.ElementListBox.MouseDoubleClick += cont.HandleMouseInput;
            cont.Output += view.HandleFormOutput;

            // Create connection to the websocket
            // Start a websocket server at port 8001
            var ws = new WebSocketServer(8001);

            // Add the Echo websocket service
            ws.AddWebSocketService<ServerController>("/Chat", ()=>new ServerController(data));

            // Start the server
            ws.Start();

            // Execute form
            Application.Run(view);

            // Stop the server
            ws.Stop();
        }
    }
}
