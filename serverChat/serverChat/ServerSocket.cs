using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace serverChat
{
    class ServerSocket
    {
        // Instance of the singleton
        private static ServerSocket inst;

        // Websocket server info
        private WebSocketServer wss = new WebSocketServer(8001);
        private ModelDataInteraction dataInt = new ModelDataInteraction();
        private Dictionary<string, SendMsgToClient> clients = new Dictionary<string, SendMsgToClient>();

        // Constructor
        public static ServerSocket Instance
        {
            get
            {
                if (inst == null)
                {
                    inst = new ServerSocket();
                }

                return inst;
            }
        }

        // Add Service
        //
        // Add a socket service to the list
        // @param username The username for the user
        public void AddService(string username)
        {
            ServerUser curUser = dataInt.GetUserObj(username);
            wss.AddWebSocketService<Chat>("/" + username, () => new Chat(curUser));
        }

        // Add Chat
        //
        // Add a chat delegate for further interaction with that person
        // @param id The ID of the user
        // @param del The delegate to interact with the user
        public void AddChat(string id, SendMsgToClient del)
        {
            clients.Add(id, del);
        }

        // Remove Chat
        //
        // Add a chat delegate for further interaction with that person
        // @param id The ID of the user
        public void RemoveChat(string id)
        {
            clients.Remove(id);
        }

        // Remove Service
        //
        // Remove a socket service from the list
        // @param username The username for the user
        public void RemoveService(string username)
        {
            wss.RemoveWebSocketService("/" + username);
        }

        // Start
        //
        // Start the socket server
        public void Start()
        {
            // Create a generic login/signup page
            wss.AddWebSocketService<Access>("/Access");

            // Start the server
            wss.Start();
        }

        // Stop
        //
        // Stop the socket server
        public void Stop()
        {
            wss.Stop();
        }

        // Transmit Message
        //
        // Transmit the specified message to the all the specified recipients
        // @param ids The id of the user
        // @param msg The message for the user
        public void TransmitMsg(string id, string msg)
        {
            if (!clients.ContainsKey(id))
            {
                return;
            }

            SendMsgToClient del = clients[id];
            del(msg);
        }
    }
}
