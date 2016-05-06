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
        // Websocket server info
        private WebSocketServer wss;
        private ModelDataInteraction dataInt = new ModelDataInteraction();
        private Dictionary<string, SendMsgToClient> clients = new Dictionary<string, SendMsgToClient>();

        // Constructor
        public ServerSocket()
        {
            wss = new WebSocketServer(8001);

            // Create a generic login/signup page
            wss.AddWebSocketService<Access>("/Access", () => new Access(this));
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

        // Add Service
        //
        // Add a socket service to the list
        // @param username The username for the user
        public void AddService(string username)
        {
            ServerUser curUser = dataInt.GetUserObj(username);
            string url = "/" + username;
            wss.AddWebSocketService<Chat>(url, () => new Chat(curUser, this));
        }

        // Get Chat
        //
        // Get a chat delegate
        // @param id The id for the user that the delegate belongs to
        public SendMsgToClient GetChat(string id)
        {
            if (clients.ContainsKey(id))
            {
                return clients[id];
            }
            return null;
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
