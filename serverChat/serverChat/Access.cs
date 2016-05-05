using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace serverChat
{
    // Access
    //
    // Handle the login and the signup process
    class Access : WebSocketBehavior
    {
        ServerModel data = ServerModel.Instance;
        ModelDataInteraction dataInt = new ModelDataInteraction();
        public event SendMsgToClient sendMsgClient;
        public event SendMsgToServer sendMsgServer;

        #region Class Manipulation
        // Constructor
        public Access()
        {
        }
        #endregion

        #region Client Request Input
        // On Open
        //
        // Handle actions to take when the server is first started
        protected override void OnOpen()
        {
        }

        // On Message
        //
        // Handle actions when a message is received from one of the clients
        // @param e The client message information
        protected override void OnMessage(MessageEventArgs e)
        {
            Message input = new Message(e.Data);
            input.Deserialize();
            Dictionary<string, string> output = new Dictionary<string, string>();

            // Handle no action provided
            if (!input.ContainsKey("action"))
            {
                output = ProcessErrorNoAction();
                SendMsgResponse(output);
                return;
            }

            switch (input.GetValue("action"))
            {
                // Handle sign up request
                case "sign":
                    output = ProcessSignUpRequest(input);
                    SendMsgResponse(output);
                    break;
                // Handle login request
                case "login":
                    output = ProcessLoginRequest(input);
                    SendMsgResponse(output);
                    break;
                // Handle error case
                default:
                    output = ProcessErrorInvalidAction(input);
                    SendMsgResponse(output);
                    break;
            }
        }
        #endregion

        #region Handle Request
        // Process Sign Up Request
        //
        // Handle a request for a new user to be created
        // @param input The message containing the input information from the client
        // @return a string containing the xml response
        public Dictionary<string, string> ProcessSignUpRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<string, string> output = new Dictionary<string, string>();

            // Create the user
            string error = dataInt.CreateUser(input.GetValue("username"), input.GetValue("password"));
            ServerUser user = new ServerUser(input.GetValue("username"));
            user.SetID(dataInt.GetId());

            curMsg.AddElement("action", "sign");

            // Check if the creation was succesful
            if (error != "")
            {
                curMsg.AddElement("error", error);
            }

            // Update the server socket with the service for the user
            UpdateSevSoc(user);

            // Add people to send message to
            output.Add("source", curMsg.Serialize());
            return output;
        }

        // Process Login Request
        //
        // Process a request for an existing user to login
        // @param input The message containing the input information from the client
        // @return a string containing the xml response
        public Dictionary<string, string> ProcessLoginRequest(Message input)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            Message curMsg = new Message();

            curMsg.AddElement("action", "login");

            // Get the user username
            if (!input.ContainsKey("username"))
            {
                curMsg.AddElement("error", "The username wasn't provided.");
                output.Add("source", curMsg.Serialize());
                return output;
            }
            string username = input.GetValue("username");

            // Get the user
            ServerUser user = dataInt.GetUserObj(username);
            if (user == new ServerUser())
            {
                curMsg.AddElement("error", "The user doesn't exist.");
                output.Add("source", curMsg.Serialize());
                return output;
            }

            // Get the user password
            if (!input.ContainsKey("password"))
            {
                curMsg.AddElement("error", "The password wasn't provided.");
                output.Add("source", curMsg.Serialize());
                return output;
            }
            string password = input.GetValue("password");

            // Check if the password is correct
            if (user.GetPassword() != password)
            {
                curMsg.AddElement("error", "The password isn't correct.");
                output.Add("source", curMsg.Serialize());
                return output;
            }

            // Set the user status to online
            dataInt.UpdateUserStatus(username, STATUS.Online);

            LoginMessage sourceMsg = new LoginMessage(user);
            output.Add("source", sourceMsg.Serialize());
            return output;
        }

        // Process an Invalid Action Request
        //
        // Process a request that is not handled
        // @param input The message containing the input information from the client
        // @return a dictionary containing the xml response
        public Dictionary<string, string> ProcessErrorInvalidAction(Message input)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            Message curMsg = new Message();

            // Get the tag
            if (!input.ContainsKey("action"))
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "An action tag wasn't provided.");
            }
            string tag = input.GetValue("action");

            curMsg.AddElement("error", "The following action tag was not handled:  " + tag);

            output.Add("source", curMsg.Serialize());

            return output;
        }

        // Process an Invalid Request
        //
        // Process a request that isn't an action request\
        // @return a dictionary containing the xml response
        public Dictionary<string, string> ProcessErrorNoAction()
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            Message curMsg = new Message();

            curMsg.AddElement("action", "error");
            curMsg.AddElement("error", "There was no action tag");
            output.Add("source", curMsg.Serialize());

            return output;
        }
        #endregion

        #region Send Ouptut
        // Send Message Response
        //
        // Send the response to the recieed message
        // @param output A dictionary containing the IDs and the messages to send
        public void SendMsgResponse(Dictionary<string, string> output)
        {
            int size = output.Count;
            for (int i = 0; i < size; i++)
            {
                string repId = output.Keys.ElementAt(i);
                string msg = output[repId];
                if (repId == "source")
                {
                    Transmit(msg);
                }
                else
                {
                    sendMsgServer(repId, msg);
                }
            }
        }

        // Transmit
        //
        // Send the message provided to all the client associated with this session
        // @param id The current client's ID
        // @param msg The message to send to the client
        public void Transmit(string msg)
        {
            Send(msg);
        }

        // Update Server Socket
        //
        // Update the server socket to be able to transmit to this user
        // @param user The server user to add to the list of connected users
        public void UpdateSevSoc(ServerUser user)
        {
            ServerSocket soc = ServerSocket.Instance;
            soc.AddService(user.GetUsername());
        }
        #endregion
    }
}
