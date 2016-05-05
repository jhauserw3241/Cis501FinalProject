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
        WebSocketServer wss = new WebSocketServer(8001);
        ServerModel data = new ServerModel();
        public event ChatHandler cOutput;
        public event AccessHandler aOutput;

        // Add
        //
        // Add a socket service to the list
        // @param username The username for the user
        public void Add(string username)
        {
            wss.AddWebSocketService<Chat>(string.Format("/{0}", username), () => new Chat(data));
        }

        // Remove
        //
        // Remove a socket service from the list
        // @param username The username for the user
        public void Remove(string username)
        {
            wss.RemoveWebSocketService(string.Format("/{0}", username));
        }

        // Access Transmit
        //
        // Transmit the message to the respective recipient for the access
        // @param recipient The person who will be recieving the message
        // @param msg The message to send to the recipient
        public void AccessTransmit(string recipient, string msg)
        {
            aOutput(recipient, msg);
        }

        // Chat Messsage Transmit
        //
        // Transmit the message to the respective recipient for the chat
        // @param recipient The person who will be recieving the message
        // @param msg The message to send to the recipient
        public void ChatMsgTransmit(string recipient, string msg)
        {
            cOutput(recipient, msg);
        }

        //// Send
        ////
        //// Send the provided message to the provided recipient
        //// @param recipient The user to send the message to
        //// @param msg The message to send to the client
        //public void Send(string clientId, string msg)
        //{
        //    //WebSocketServer cur = web
        //    //wss.WebSocketServices
        //    //Send(clientId, msg);
        //    //base.Sessions.SendTo(clientId, msg);
        //}

        // Start
        //
        // Start the socket server
        // @param d The model object
        public void Start(ServerModel d)
        {
            data = d;

            // Create a generic login/signup page
            wss.AddWebSocketService<Access>("/Access", () => new Access(data));

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
    }
}
