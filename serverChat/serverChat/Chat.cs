using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace serverChat
{
    // Chat
    //
    // Handle generic messages sent to the server from the client
    class Chat : WebSocketBehavior
    {
        ServerSocket soc;
        ServerModel data = ServerModel.Instance;
        ModelDataInteraction dataInt = new ModelDataInteraction();
        //ServerSocket soc = ServerSocket.Instance;
        //public event SendMsgToClient sendMsgClient;
        //public event SendMsgToServer sendMsgServer;
        ServerUser user = new ServerUser();

        #region Class Manipulation
        // Constructor
        public Chat() : this(null, null)
        {
        }

        // Constructor
        //
        // @param u The user object for the service
        // @param s The server socket object
        public Chat(ServerUser u, ServerSocket s)
        {
            // Setup the server
            soc = s;

            // Update the user object
            user = u;

            // Create the delegate for the new client
            SendMsgToClient del = new SendMsgToClient(Transmit);
            if ((u != new ServerUser()) && (u != null))
            {
                soc.AddChat(u.GetId(), del);
            }
        }
        #endregion

        #region Handle Client Input
        // On Open
        //
        // Handle actions to take when the server is first started
        protected override void OnOpen()
        {
        }

        // On Close
        //
        // Handle actions when the client's connection to this server object is disconnected
        protected override void OnClose(CloseEventArgs e)
        {
            soc.RemoveChat(user.GetId());
            soc.RemoveService("/" + user.GetUsername());
        }

        // On Message
        //
        // Handle actions when a message is received from one of the clients
        // @param e The client message information
        protected override void OnMessage(MessageEventArgs e)
        {
            Message input = new Message(e.Data);
            input.Deserialize();
            //Message output = new Message();
            Dictionary<int, string> output = new Dictionary<int, string>();

            // Handle no action provided
            if (!input.ContainsKey("action"))
            {
                output = ProcessErrorNoAction();
                SendMsgResponse(output);
            }

            switch (input.GetValue("action"))
            {
                // Handle add contact request
                case "addCont":
                    output = ProcessAddContactRequest(input);
                    break;
                // Handle remove contact request
                case "rmCont":
                    output = ProcessRemoveContactRequest(input);
                    break;
                // Handle update conversation request
                case "udConv":
                    output = ProcessUpdateConvRequest(input);
                    break;
                // Handle update contact request
                case "udCont":
                    output = ProcessUpdateContRequest(input);
                    break;
                // Handle message request
                case "msg":
                    output = ProcessMessageRequest(input);
                    break;
                // Handle logout request
                case "logout":
                    output = ProcessLogoutRequest(input);
                    break;
                // Handle unsupported action
                default:
                    output = ProcessErrorInvalidAction(input);
                    break;
            }

            SendMsgResponse(output);
        }
        #endregion

        #region Process Input
        // Process Add Contact Request
        //
        // Process a request to add a contact to a specific user
        // @param input The message containing the input information from the client
        // @return a dictionary containing the username and the xml response
        public Dictionary<int, string> ProcessAddContactRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<int, string> output = new Dictionary<int, string>();

            curMsg.AddElement("action", "addCont");

            // Get source user username
            if (!input.ContainsKey("source"))
            {
                curMsg.AddElement("error", "The source username wasn't provided.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }
            string sourceUsername = input.GetValue("source");

            // Get new contact user username
            if (!input.ContainsKey("username"))
            {
                curMsg.AddElement("error", "The new contact username wasn't provided.");
            }
            string newContactUsername = input.GetValue("username");
            
            // Get source user
            ServerUser sourceUser = dataInt.GetUserObj(sourceUsername);
            if (sourceUser == null)
            {
                curMsg.AddElement("error", "The source user doesn't exist.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Get new contact user
            ServerUser newContUser = dataInt.GetUserObj(newContactUsername);
            if (newContUser == null)
            {
                curMsg.AddElement("error", "The new contact user doesn't exist.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Verify that the new contact isn't already a contact
            if (sourceUser.GetContactListUsernames().Contains(newContactUsername))
            {
                curMsg.AddElement("error", "The user to be added as a contact is already a contact.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Verify that the new contact is an existing user
            ServerUser newContact = dataInt.GetUserObj(newContactUsername);
            if (newContact == new ServerUser())
            {
                curMsg.AddElement("error", "The user to be added as a contact doesn't exist.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Update the contact relationships of all the users involved
            List<ServerUser> users = new List<ServerUser>();
            users.Add(sourceUser);
            users.Add(newContact);
            if (!dataInt.UpdateContactRelationships(users, "add"))
            {
                curMsg.AddElement("error", "An error occurred during the contact addition process.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Get output information
            AddContactMessage sourceMsg = new AddContactMessage(newContact);
            AddContactMessage newContMsg = new AddContactMessage(sourceUser);
            output.Add(sourceUser.GetId(), sourceMsg.GetMessage());
            output.Add(newContact.GetId(), newContMsg.GetMessage());

            return output;
        }

        // Process Remove Contact Request
        //
        // Process a request to remove a contact from a specific user
        // @param input The message containing the input information from the client
        // @return a dictionary containing the username and the xml response
        public Dictionary<int, string> ProcessRemoveContactRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<int, string> output = new Dictionary<int, string>();
            string sourceUsername = input.GetValue("source");
            string rContactUsername = input.GetValue("username");

            ServerUser sourceUser = dataInt.GetUserObj(sourceUsername);
            // Verify that the source user is an existing user
            if (sourceUser == new ServerUser())
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The source user doesn't exist.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Verify that the new contact isn't already a contact
            if (!sourceUser.GetContactListUsernames().Contains(rContactUsername))
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The user to be removed as a contact isn't a contact.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Verify that the new contact is an existing user
            ServerUser oldContact = dataInt.GetUserObj(rContactUsername);
            if (oldContact == new ServerUser())
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The user to be removed as a contact doesn't exist.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Update the contact relationships of all the users involved
            List<ServerUser> users = new List<ServerUser>();
            users.Add(sourceUser);
            users.Add(oldContact);
            if (!dataInt.UpdateContactRelationships(users, "remove"))
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "An error occurred during the contact addition process.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }

            // Get output information
            AddContactMessage sourceMsg = new AddContactMessage(oldContact);
            AddContactMessage newContMsg = new AddContactMessage(sourceUser);
            output.Add(sourceUser.GetId(), sourceMsg.GetMessage());
            output.Add(sourceUser.GetId(), newContMsg.GetMessage());

            return output;
        }

        // Process Update Conversation Request
        //
        // Process a request to update a conversation from a specific user
        // @param input The message containing the input information from the client
        // @return a dictionary containing the xml response
        public Dictionary<int, string> ProcessUpdateConvRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<int, string> output = new Dictionary<int, string>();

            curMsg.AddElement("action", "udConv");

            // Get conversation object
            if (!input.ContainsKey("conv"))
            {
                curMsg.AddElement("error", "The source user doesn't exist.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }
            string convName = input.GetValue("conv");
            curMsg.AddElement("conv", convName);
            ServerConversation conv = dataInt.GetConvObj(convName);
            // If the conversation doesn't exist, create a new conversation
            if (conv == null)
            {
                conv = new ServerConversation(convName);
                dataInt.AddConvToList(conv);
            }

            // Update the name
            if (input.ContainsKey("newConv"))
            {
                string newName = input.GetValue("newConv");
                dataInt.ChangeConvName(conv, newName);
                curMsg.AddElement("newConv", newName);
            }

            // Update the participant list
            if (input.ContainsKey("par"))
            {
                string error = dataInt.AddParsToConv(convName, input.GetValue("par"));
                if (error != "")
                {
                    curMsg.AddElement("error", error);
                    output.Add(-2, curMsg.Serialize());
                    return output;
                }
                else
                {
                    curMsg.AddElement("text", String.Join("\n", conv.GetMessageHistory()));
                }
            }

            if (input.ContainsKey("leave"))
            {
                string error = dataInt.RemoveParticipantFromConversation(convName, input.GetValue("leave"));
                if (error != "")
                {
                    curMsg.AddElement("error", error);
                    output.Add(-2, curMsg.Serialize());
                    return output;
                }
                else
                {
                    curMsg.AddElement("leave", input.GetValue("leave"));
                }
            }

            // Compile the success message for all the conversation participants
            List<ServerUser> participants = conv.GetParicipantList();
            List<string> party = new List<string>();
            int size = participants.Count;
            for (int i = 0; i < size; i++)
            {
                ServerUser curPar = participants.ElementAt(i);
                party = participants.Select(x => x.GetUsername()).ToList();
                party.Remove(curPar.GetUsername());
                curMsg.AddElement("par", String.Join(",", party));
                output.Add(curPar.GetId(), curMsg.Serialize());
                curMsg.RemoveElement("par");
            }

            return output;
        }

        // Process Update Contact Request
        //
        // Process a request to update a contact from a specific user
        // @param input The message containing the input information from the client
        // @return a dictionary containing the xml response
        public Dictionary<int, string> ProcessUpdateContRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<int, string> output = new Dictionary<int, string>();

            curMsg.AddElement("action", "udCont");

            // Get source username
            if (!input.ContainsKey("source"))
            {
                curMsg.AddElement("error", "The source username wasn't provided.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }
            string sourceUsername = input.GetValue("source");

            curMsg.AddElement("source", sourceUsername);

            // Get user
            ServerUser oldUser = dataInt.GetUserObj(sourceUsername);
            if (oldUser == new ServerUser())
            {
                curMsg.AddElement("error", "User doesn't exist.");
                output.Add(-2, curMsg.Serialize());
                return output;
            }
            ServerUser newUser = oldUser;

            // Update the display name
            if (input.ContainsKey("dispName"))
            {
                string newDispName = input.GetValue("dispName");
                newUser.SetName(newDispName);
                curMsg.AddElement("dispName", newDispName);
            }

            // Update the display name
            if (input.ContainsKey("password"))
            {
                string newPassword = input.GetValue("password");
                newUser.SetPassword(newPassword);
                input.AddElement("password", newPassword);
            }

            // Update the display name
            if (input.ContainsKey("state"))
            {
                string newStatus = input.GetValue("state");
                STATUS newStatusObj = dataInt.ConvertStringStatus(newStatus);
                dataInt.UpdateUserStatus(sourceUsername, newStatusObj);
                curMsg.AddElement("state", newStatus);
            }

            // Update the user object in the list
            dataInt.UpdateUserList(oldUser, newUser);

            // Compile the success message for all the conversation participants
            List<ServerUser> contacts = newUser.GetContactList();
            int size = contacts.Count;
            for (int i = 0; i < size; i++)
            {
                ServerUser cont = contacts.ElementAt(i);
                output.Add(cont.GetId(), curMsg.Serialize());
            }
            output.Add(-2, curMsg.Serialize());

            return output;
        }

        // Process Logout Request
        //
        // Process a request for an existing user to logout
        // @param input The message containing the input information from the client
        // @return a string containing the xml response
        public Dictionary<int, string> ProcessLogoutRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<int, string> output = new Dictionary<int, string>();

            curMsg.AddElement("action", "logout");

            // Get the username
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

            // Set the user status to online
            dataInt.UpdateUserStatus(username, STATUS.Offline);

            //curMsg.AddElement("action", "logout");
            output.Add(-2, curMsg.Serialize());

            List<ServerUser> contacts = user.GetContactList();
            int size = contacts.Count;
            for (int i = 0; i < size; i++)
            {
                ServerUser cont = contacts.ElementAt(i);
                curMsg = new Message();
                curMsg.AddElement("action", "udCont");
                curMsg.AddElement("source", username);
                curMsg.AddElement("state", STATUS.Offline.ToString());

                output.Add(cont.GetId(), curMsg.Serialize());
            }

            return output;
        }

        // Process Sending a Message
        //
        // Process a request to send a message to a conversation
        // @param input The message containing the input information from the client
        // @return a dictionary containing the xml response
        public Dictionary<int, string> ProcessMessageRequest(Message input)
        {
            Dictionary<int, string> output = new Dictionary<int, string>();
            input.RemoveElement("source");
            output.Add(-2, input.Serialize());
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
        // @param msg The message to send to the client
        public void Transmit(string msg)
        {
            Send(msg);
        }
        #endregion
    }
}
