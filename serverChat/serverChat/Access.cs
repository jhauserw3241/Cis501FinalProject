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
        ModelDataInteraction dataInt;

        #region Class Manipulation
        // Constructor
        public Access() : this(null)
        {
        }

        // Constructor
        //
        // @param d The model object
        public Access(ServerModel d)
        {
            data = d;
            dataInt = new ModelDataInteraction(data);
        }
        #endregion

        #region Get Access Input
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
            Message output = new Message();

            switch (input.GetValue("action"))
            {
                case "sign":
                    output.SetSerMsg(HandleSignUpRequest(input));
                    break;
                case "login":
                    // Assign cookie and add it to the list
                    output.SetSerMsg(HandleLoginRequest(input));
                    break;
                default:
                    // TODO: Pass error message

                    break;
            }

            Sessions.Broadcast(output.Serialize());
        }
        #endregion

        #region Handle Requests
        // Handle Sign Up Request
        //
        // Handle a request for a new user to be created
        // @param input The message containing the input information from the client
        // @return a string containing the xml response
        public string HandleSignUpRequest(Message input)
        {
            Message output = new Message();

            // Create the user
            string error = dataInt.CreateUser(input.GetValue("username"), input.GetValue("password"));

            // Check if the creation was succesful
            if (error == "")
            {
                output.AddElement("action", "sign");
            }
            else
            {
                output.AddElement("action", "error");
                output.AddElement("error", error);
            }

            return output.Serialize();
        }

        // Handle Login Request
        //
        // Process a request for an existing user to login
        // @param input The message containing the input information from the client
        // @return a string containing the xml response
        public string HandleLoginRequest(Message input)
        {
            Message output = new Message();

            // Get the user object that matches the provided username
            ServerUser user = dataInt.GetUserObj(input.GetValue("username"));

            // Check if the user exists
            if (user == new ServerUser())
            {
                output.AddElement("action", "error");
                output.AddElement("error", "The username isn't valid.");
                return output.Serialize();
            }

            // Check if the password is correct
            if (user.GetPassword() != input.GetValue("password"))
            {
                output.AddElement("action", "error");
                output.AddElement("error", "The password isn't correct.");
                return output.Serialize();
            }

            // Set the user status to online
            dataInt.UpdateUserStatus(input.GetValue("username"), STATUS.Online);

            LoginMessage sourceMsg = new LoginMessage(user);
            return sourceMsg.GetMessage();
        }
        #endregion
    }
}
