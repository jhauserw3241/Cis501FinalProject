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
        ServerModel data = new ServerModel();
        ModelDataInteraction dataInt;

        #region Class Manipulation
        // Constructor
        public Chat() : this(null)
        {
        }

        // Constructor
        //
        // @param d The model object
        public Chat(ServerModel d)
        {
            data = d;
            dataInt = new ModelDataInteraction(data);
        }
        #endregion

        #region Handle Client Input
        // On Open
        //
        // Handle actions to take when the server is first started
        protected override void OnOpen()
        {
            // Add session for the current client
            //IWebSocketSession curSession = Sessions.Sessions.Aggregate<I>
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
            Dictionary<string, string> output = new Dictionary<string, string>();

            if (input.ContainsKey("action"))
            {
                switch (input.GetValue("action"))
                {
                    case "addCont":
                        // TODO: Send update to everyone involved
                        output = ProcessAddContactRequest(input);
                        if (output.ContainsKey("all"))
                        {
                            Sessions.Broadcast(output["all"]);
                        }
                        else
                        {
                            Sessions.Broadcast(output["source"]);
                        }
                        break;
                    case "rmCont":
                        // TODO: Update all users, not just source
                        output = ProcessRemoveContactRequest(input);
                        if (output.ContainsKey("all"))
                        {
                            Sessions.Broadcast(output["all"]);
                        }
                        else
                        {
                            Sessions.Broadcast(output["source"]);
                        }
                        break;
                    case "udConv":
                        // TODO: Update all the users, not just source
                        output = ProcessUpdateConvRequest(input);
                        Sessions.Broadcast(output["all"]);
                        break;
                    case "udCont":
                        // TODO: Verify user existence
                        // TODO: Update the specified parts of the user object
                        output = ProcessUpdateContRequest(input);
                        Sessions.Broadcast(output["all"]);
                        break;
                    case "msg":
                        // TODO: Pass message
                        output = ProcessMessageRequest(input);
                        Sessions.Broadcast(output["source"]);
                        break;
                    case "logout":
                        // TODO: Remove their cookie from the list
                        // TODO: Send update to everyone involved
                        output = ProcessLogoutRequest(input);
                        Sessions.Broadcast(output["source"]);
                        break;
                    default:
                        // TODO: Pass error message
                        output = ProcessErrorInvalidAction(input);
                        Sessions.Broadcast(output["source"]);
                        break;
                }
            }
            else
            {
                output = ProcessErrorNoAction();
                Sessions.Broadcast(output["source"]);
            }
        }
        #endregion

        #region Process Input
        // Process Add Contact Request
        //
        // Process a request to add a contact to a specific user
        // @param input The message containing the input information from the client
        // @return a dictionary containing the username and the xml response
        public Dictionary<string, string> ProcessAddContactRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<string, string> output = new Dictionary<string, string>();
            string sourceUsername = input.GetValue("source");
            string newContactUsername = input.GetValue("username");

            ServerUser sourceUser = dataInt.GetUserObj(sourceUsername);
            // Verify that the source user is an existing user
            if (sourceUser == new ServerUser())
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The source user doesn't exist.");
                output.Add("all", curMsg.Serialize());
                return output;
            }

            // Verify that the new contact isn't already a contact
            if (sourceUser.GetContactListUsernames().Contains(newContactUsername))
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The user to be added as a contact is already a contact.");
                output.Add("all", curMsg.Serialize());
                return output;
            }

            // Verify that the new contact is an existing user
            ServerUser newContact = dataInt.GetUserObj(newContactUsername);
            if (newContact == new ServerUser())
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The user to be added as a contact doesn't exist.");
                output.Add("all", curMsg.Serialize());
                return output;
            }

            // Update the contact relationships of all the users involved
            List<ServerUser> users = new List<ServerUser>();
            users.Add(sourceUser);
            users.Add(newContact);
            if (!dataInt.UpdateContactRelationships(users, "add"))
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "An error occurred during the contact addition process.");
                output.Add("all", curMsg.Serialize());
                return output;
            }

            // Get output information
            AddContactMessage sourceMsg = new AddContactMessage(newContact);
            AddContactMessage newContMsg = new AddContactMessage(sourceUser);
            output.Add("source", sourceMsg.GetMessage());
            output.Add("newCont", newContMsg.GetMessage());

            return output;
        }

        // Process Remove Contact Request
        //
        // Process a request to remove a contact from a specific user
        // @param input The message containing the input information from the client
        // @return a dictionary containing the username and the xml response
        public Dictionary<string, string> ProcessRemoveContactRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<string, string> output = new Dictionary<string, string>();
            string sourceUsername = curMsg.GetValue("source");
            string rContactUsername = curMsg.GetValue("username");

            ServerUser sourceUser = dataInt.GetUserObj(sourceUsername);
            // Verify that the source user is an existing user
            if (sourceUser == new ServerUser())
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The source user doesn't exist.");
                output.Add("all", curMsg.Serialize());
                return output;
            }

            // Verify that the new contact isn't already a contact
            if (!sourceUser.GetContactListUsernames().Contains(rContactUsername))
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The user to be removed as a contact isn't a contact.");
                output.Add("all", curMsg.Serialize());
                return output;
            }

            // Verify that the new contact is an existing user
            ServerUser oldContact = dataInt.GetUserObj(rContactUsername);
            if (oldContact == new ServerUser())
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The user to be removed as a contact doesn't exist.");
                output.Add("all", curMsg.Serialize());
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
                output.Add("all", curMsg.Serialize());
                return output;
            }

            // Get output information
            AddContactMessage sourceMsg = new AddContactMessage(oldContact);
            AddContactMessage newContMsg = new AddContactMessage(sourceUser);
            output.Add("source", sourceMsg.GetMessage());
            output.Add("oldCont", newContMsg.GetMessage());

            return output;
        }

        // Process Update Conversation Request
        //
        // Process a request to update a conversation from a specific user
        // @param input The message containing the input information from the client
        // @return a dictionary containing the xml response
        public Dictionary<string, string> ProcessUpdateConvRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<string, string> output = new Dictionary<string, string>();
            string convName = input.GetValue("conv");

            curMsg.AddElement("action", "udConv");

            // Get conversation object
            ServerConversation conv = dataInt.GetConvObj(convName);
            if (conv == new ServerConversation())
            {
                curMsg.AddElement("error", "The conversation doesn't exist.");
                output.Add("all", curMsg.Serialize());
                return output;
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
                    output.Add("all", curMsg.Serialize());
                    return output;
                }
            }

            //// Compile the success message for all the conversation participants
            //List<string> partUsernames = conv.GetParticipantListUsernames();
            //int size = partUsernames.Count;
            //for(int i = 0; i < size; i++)
            //{
            //    output.Add("")
            //}

            output.Add("all", curMsg.Serialize());

            return output;
        }

        // Process Update Contact Request
        //
        // Process a request to update a contact from a specific user
        // @param input The message containing the input information from the client
        // @return a dictionary containing the xml response
        public Dictionary<string, string> ProcessUpdateContRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<string, string> output = new Dictionary<string, string>();
            string sourceUsername = input.GetValue("username");

            curMsg.AddElement("action", "udCont");
            curMsg.AddElement("username", sourceUsername);

            // Get user
            ServerUser oldUser = dataInt.GetUserObj(sourceUsername);
            if (oldUser == new ServerUser())
            {
                curMsg.AddElement("error", "User doesn't exist.");
                // TODO: Update "source" with source username
                output.Add("source", curMsg.Serialize());
                return output;
            }
            ServerUser newUser = oldUser;

            // Update the display name
            if (input.ContainsKey("newDispName"))
            {
                string newDispName = input.GetValue("newDispName");
                newUser.SetName(newDispName);
                curMsg.AddElement("newDispName", newDispName);
            }

            // Update the display name
            if (input.ContainsKey("newPassword"))
            {
                string newPassword = input.GetValue("newPassword");
                newUser.SetPassword(newPassword);
                input.AddElement("newPassword", newPassword);
            }

            // Update the display name
            if (input.ContainsKey("newState"))
            {
                string newStatus = input.GetValue("newState");
                STATUS newStatusObj = dataInt.ConvertStringStatus(newStatus);
                dataInt.UpdateUserStatus(sourceUsername, newStatusObj);
                curMsg.AddElement("newState", newStatus);
            }

            //// Compile the success message for all the conversation participants
            //List<string> partUsernames = conv.GetParticipantListUsernames();
            //int size = partUsernames.Count;
            //for(int i = 0; i < size; i++)
            //{
            //    output.Add("")
            //}

            output.Add("all", curMsg.Serialize());

            return output;
        }

        // Process Logout Request
        //
        // Process a request for an existing user to logout
        // @param input The message containing the input information from the client
        // @return a string containing the xml response
        public Dictionary<string, string> ProcessLogoutRequest(Message input)
        {
            Message curMsg = new Message();
            Dictionary<string, string> output = new Dictionary<string, string>();
            string username = input.GetValue("username");

            // Get the user object that matches the provided username
            ServerUser user = dataInt.GetUserObj(username);

            // Check if the user exists
            if (user == new ServerUser())
            {
                curMsg.AddElement("action", "error");
                curMsg.AddElement("error", "The user doesn't exist.");
                output.Add("all", curMsg.Serialize());
                return output;
            }

            // Set the user status to online
            dataInt.UpdateUserStatus(username, STATUS.Offline);

            curMsg.AddElement("action", "logout");
            output.Add("source", curMsg.Serialize());

            List<string> contUnames = user.GetContactListUsernames();
            int contNum = contUnames.Count;
            for (int i = 0; i < contNum; i++)
            {
                string curUsername = contUnames.ElementAt(i);
                curMsg = new Message();
                curMsg.AddElement("action", "udCont");
                curMsg.AddElement("username", username);
                curMsg.AddElement("state", STATUS.Offline.ToString());

                output.Add(curUsername, curMsg.Serialize());
            }

            return output;
        }
        
        // Process Sending a Message
        //
        // Process a request to send a message to a conversation
        // @param input The message containing the input information from the client
        // @return a dictionary containing the xml response
        public Dictionary<string, string> ProcessMessageRequest(Message input)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            input.RemoveElement("source");
            output.Add("source", input.Serialize());
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
            string tag = input.GetValue("action");

            curMsg.AddElement("error", "The following action tag was not handled:  " + tag);

            output.Add("source", curMsg.Serialize());

            return output;
        }

        // Process an Invalid Request
        //
        // Process a request that isn't an action request\
        // @return a dictionary containing the xml response
        public Dictionary<string,string> ProcessErrorNoAction()
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            Message curMsg = new Message();

            curMsg.AddElement("error", "There was no action tag");

            output.Add("source", curMsg.Serialize());

            return output;
        }
        #endregion
    }
}
