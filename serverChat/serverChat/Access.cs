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
        ServerSocket soc;
        ModelDataInteraction dataInt = new ModelDataInteraction();
        public event SendMsgToServer sendMsgServer;

        #region Class Manipulation
        // Constructor
        public Access() : this(null)
        {
        }

        // Constructor
        //
        // @param s The server socket object
        public Access(ServerSocket s)
        {
            soc = s;
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
            Dictionary<int, string> output = new Dictionary<int, string>();

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
        public Dictionary<int, string> ProcessSignUpRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<int, string> output = new Dictionary<int, string>();

            curMsg.AddElement("action", "sign");

            // Get user username
            if (!input.ContainsKey("username"))
            {
                curMsg.AddElement("error", "The username was not provided.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }
            string username = input.GetValue("username");

            // Get user password
            if (!input.ContainsKey("password"))
            {
                curMsg.AddElement("error", "The password was not provided.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }
            string password = input.GetValue("password");

            // Create the user
            string error = dataInt.CreateUser(username, password);
            ServerUser user = dataInt.GetUserObj(username);

            // Check if the creation was succesful
            if (error != "")
            {
                curMsg.AddElement("error", error);
            }

            // Update the server socket with the service for the user
            UpdateSevSoc(user);

            // Add people to send message to
            output.Add(-2, curMsg.Serialize());
            return output;
        }

        // Process Login Request
        //
        // Process a request for an existing user to login
        // @param input The message containing the input information from the client
        // @return a string containing the xml response
        public Dictionary<int, string> ProcessLoginRequest(Message input)
        {
            Dictionary<int, string> output = new Dictionary<int, string>();
            Message curMsg = new Message();

            curMsg.AddElement("action", "login");

            // Get the user username
            if (!input.ContainsKey("username"))
            {
                curMsg.AddElement("error", "The username wasn't provided.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }
            string username = input.GetValue("username");

            // Get the user
            ServerUser user = dataInt.GetUserObj(username);
            if (user == new ServerUser())
            {
                curMsg.AddElement("error", "The user doesn't exist.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Get the user password
            if (!input.ContainsKey("password"))
            {
                curMsg.AddElement("error", "The password wasn't provided.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }
            string password = input.GetValue("password");

            // Check if the password is correct
            if (user.GetPassword() != password)
            {
                curMsg.AddElement("error", "The password isn't correct.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Set the user status to online
            dataInt.UpdateUserStatus(username, STATUS.Online);

            // Update the server socket with the service for the user
            UpdateSevSoc(user);

            LoginMessage sourceMsg = new LoginMessage(user);
            output.Add(-2, sourceMsg.Serialize());
            return output;
        }

        // Process an Invalid Action Request
        //
        // Process a request that is not handled
        // @param input The message containing the input information from the client
        // @return a dictionary containing the xml response
        public Dictionary<int, string> ProcessErrorInvalidAction(Message input)
        {
            Dictionary<int, string> output = new Dictionary<int, string>();
            Message curMsg = new Message();

            // Get the tag
            if (!input.ContainsKey("action"))
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "An action tag wasn't provided.");
            }
            string tag = input.GetValue("action");

            curMsg.AddElement("error", "The following action tag was not handled:  " + tag);

            output.Add(-2, curMsg.Serialize());

            return output;
        }

        // Process an Invalid Request
        //
        // Process a request that isn't an action request\
        // @return a dictionary containing the xml response
        public Dictionary<int, string> ProcessErrorNoAction()
        {
            Dictionary<int, string> output = new Dictionary<int, string>();
            Message curMsg = new Message();

            curMsg.AddElement("action", "error");
            curMsg.AddElement("error", "There was no action tag");
            output.Add(-2, curMsg.Serialize());

            return output;
        }
        #endregion

        #region Send Ouptut
        // Send Message Response
        //
        // Send the response to the recieed message
        // @param output A dictionary containing the IDs and the messages to send
        public void SendMsgResponse(Dictionary<int, string> output)
        {
            int size = output.Count;
            for (int i = 0; i < size; i++)
            {
                int repId = output.Keys.ElementAt(i);
                string msg = output[repId];
                if (repId == -2)
                {
                    Transmit(msg);
                }
                else
                {
                    SendMsgToClient recSend = soc.GetChat(repId);
                    if (recSend != null)
                    {
                        recSend(msg);
                    }
                    else
                    {
                        Transmit("<error error=\"Delegate could not be found for recipient.\" />");
                    }
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
            //ServerSocket soc = ServerSocket.Instance;
            soc.AddService(user.GetUsername());
        }
        #endregion
    }
}
