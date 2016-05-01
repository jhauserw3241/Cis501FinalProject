using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace serverChat
{
    public class ServerController : WebSocketBehavior
    {
        private ServerModel data = new ServerModel();
        public event ServerOutputHandler output;

        #region Manipulate Class
        // Constructor
        public ServerController() : this(null)
        {
        }

        // Constructor
        //
        // @arg d The model object instance
        public ServerController(ServerModel d)
        {
            // Update the server objects
            data = d;
        }

        // Destructor
        ~ServerController()
        {
        }
        #endregion

        #region Handle Client Input
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
            Dictionary<string, string> input = DeserializeXml(e.Data);
            string output = "";

            //if (!msg.ContainsKey("action")) return;

            switch (input["action"])
            {
                case "sign":
                    // TODO: Check if username is already used
                    // TODO: Handle creation of user

                    break;
                case "login":
                    // TODO: Check if the username and password match
                    // a currently existing user

                    break;
                case "addCont":
                    // TODO: Check if the user currently has the specified user as a contact
                    //// Otherwise, TODO: Add the specified user as a contact

                    break;
                case "rmCont":
                    // TODO: Verify contact is part of user contact list
                    // TODO: Remove contact

                    break;
                case "udConv":
                    // TODO: Verify conversation exists
                    // TODO: Update the specified aspects of the conversation

                    break;
                case "udCont":
                    // TODO: Verify user existence
                    // TODO: Update the specified parts of the user object

                    break;
                case "msg":
                    // TODO: Pass message

                    break;
                default:
                    // TODO: Pass error message

                    break;
            }

            Send(output);
        }
        #endregion

        #region Process Input
        // Process Sign Up Request
        //
        // Process a request for a new user to be created
        // @param uInfo The information for the new user
        // @return a string containing the xml
        public string ProcessSignUpRequest(Dictionary<string, string> uInfo)
        {
            string uname = uInfo["username"];
            string pass = uInfo["password"];
            string output = "";

            // Check if the provided username is already being used
            if (IsUsernameUsed(uname))
            {
                // Create error data
                Dictionary<string, string> errorData = new Dictionary<string, string>();
                errorData.Add("action", "error");
                errorData.Add("error", "Username is already being used.");
                output = SerializeXml(errorData);
            }
            else
            {
                // Setup user creation
                ServerUser curUser = new ServerUser();
                //List<ServerUser> userList = data.GetUserList();

                // Update the object with provided data
                curUser.SetUsername(uname);
                curUser.SetName(uname);
                curUser.SetPassword(pass);

                // Update user list in model
                AddUserToList(curUser);
                //userList.Add(curUser);
                //data.SetUserList(userList);
            }

            //return false;
            return output;
        }
        #endregion

        #region Add Elements to Model Lists
        // Add User To List
        //
        // Add the user to the list of all of the users in the model
        // @arg user The current user
        public void AddUserToList(ServerUser user)
        {
            // Get the list of all users from the model
            List<ServerUser> userList = data.GetUserList();

            // Add the current user object
            userList.Add(user);

            // Set the current user list to the model user list
            data.SetUserList(userList);
        }

        // Add Conversation To List
        //
        // Add the conversation to the list of all conversations in the model
        // @param conv The current conversation
        public void AddConvToList(ServerConversation conv)
        {
            // Get the list of all conversations from the model
            List<ServerConversation> convList = data.GetConversationList();

            // Add the current conversation object
            convList.Add(conv);

            // Set the current conversation list to the model conversation list
            data.SetConversationList(convList);
        }
        #endregion

        // Create Conversation
        //
        // Create conversation and add it to the list of conversations
        // @param name The conversation name of the new conversation
        // @return the error message string if there is one+
        public void AddConversation(string name)
        {
            // Check if the username is already being used
            if (IsUsernameUsed(uname))
            {
                return "Username already used.";
            }

            // Create conversation object from conversation info
            ServerConversation conv = new ServerConversation(name);

            // Add conversation to list
            AddConvToList(conv);

            // Update the view
            output("UpdateConversationList", data.GetConversationList());
        }

        // Create User
        //
        // Create user and add it to list of users
        // @param uname The username of the new user
        // @param pass The password of the new user
        // @return the error message string if there is one
        public string CreateUser(string uname, string pass)
        {
            // Check if the username is already being used
            if (IsUsernameUsed(uname))
            {
                return "Username already used.";
            }

            // Create user object from user info
            ServerUser user = new ServerUser(uname);
            user.SetPassword(pass);
            user.SetStatus(STATUS.Online);

            // Add user to list
            AddUserToList(user);

            // Update the view
            output("UpdateUserList", data.GetUserList());

            return "";
        }

        // Add Participant To Conversation
        //
        // Add the specified participant to the specified conversation
        // @arg convName The name of the conversation
        // @arg username The username of the new participant
        // @return whether or not the participant was added to the conversation
        public bool AddParticipantToConversation(string convName, string username)
        {
            Dictionary<string, List<string>> convRelationships = data.GetContactRelationshipDict();
            List<string> participants = GetConvParticipants(convName);

            // Determine if the current conversation exists or not
            if (participants == new List<string>())
            {
                return false;
            }

            // Put the username of the new participant into the list of participants
            participants.Add(username);

            // Remove the current conversation from the list
            convRelationships.Remove(convName);

            // Add the new list of participants to the contact relationship list
            convRelationships.Add(convName, participants);

            // Set the current list of names to the list on the server
            data.SetContactRelationshipList(convRelationships);

            return true;
        }

        // Get User Obj Message
        //
        // Get the output message that is from the new or previously
        // existing user object
        // @param userObj The object for the user
        // @return a string that creates a new user on the client side
        public string GetUserObjMessage(ServerUser user)
        {
            // Convert the contact list to a single string
            string contUNames = string.Join(",", user.GetContactListUsernames());
            string contDNames = string.Join(",", user.GetContactListNames());

            Dictionary<string, string> output = new Dictionary<string, string>();
            output.Add("action", "login");
            output.Add("username", user.GetUsername());
            output.Add("dispName", user.GetName());
            output.Add("password", user.GetPassword());
            output.Add("contUsername", contUNames);
            output.Add("contDisplName", contDNames);
            output.Add("state", "Online");
            return SerializeXml(output);
        }

        // Get Conversation Message
        //
        // Get the output message that is for the new information for the conversation
        // @param convObj The object for the conversation
        // @return a string that creates a new conversation on the client side
        public string GetConvMessage(ServerConversation convObj)
        {
            // Convert the participants list to a single string
            string parUNames = string.Join(",", convObj.GetParticipantListUsernames());

            Dictionary<string, string> output = new Dictionary<string, string>();
            output.Add("action", "udConv");
            output.Add("conv", convObj.GetConversationName());
            output.Add("par", parUNames);
            return SerializeXml(output);
        }

        // Get the Add Contact Message
        //
        // Get the output message that is for adding a new contact to the user's profile
        // @param contact The user object for the new contact
        // @return a string that creates a new contact on the client side
        public string GetAddContMessage(ServerUser contact)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            output.Add("action", "addCont");
            output.Add("username", contact.GetUsername());
            output.Add("dispName", contact.GetName());
            output.Add("state", contact.GetStatus().ToString());
            return SerializeXml(output);
        }

        // Add User
        //
        // Add user to list of users
        // @param username The username of the new user
        // @param dName The display name of the new user
        // @param status The status of the new user
        public void AddUser(string username, string dName, STATUS status)
        {
            // Create user object from user info
            ServerUser user = new ServerUser();
            user.SetUsername(username);
            user.SetName(dName);
            user.SetStatus(status);

            // Add user to list
            List<ServerUser> userList = data.GetUserList();
            userList.Add(user);

            // Update the view
            output("UpdateUserList", userList);
        }

        // Get Conversation Object
        //
        // Get the conversation from the list in the model
        // @arg name The name of the conversation
        // @return the conversation object
        public ServerConversation GetConvObj(string name)
        {
            List<ServerConversation> convList = data.GetConversationList();
            int convListLen = convList.Count;

            if (convListLen != 0)
            {
                for (int i = 0; i < convListLen; i++)
                {
                    // TODO: Check if the conversation is in the list and return it
                    return new ServerConversation();
                }
            }

            return new ServerConversation();
        }

        // Get Conversation Participants
        //
        // Get the list of participants from the specified conversation
        // @arg convName The name of the conversation
        public List<string> GetConvParticipants(string convName)
        {
            // Get list of all conversation relationships
            Dictionary<string, List<string>> convRelationships = data.GetContactRelationshipDict();

            // Get the list of participants in the current conversation
            List<string> participants = new List<string>();
            convRelationships.TryGetValue(convName, out participants);

            return participants;
        }

        // Get Conversation Relationship List
        //
        // Get the conversation relationship from the list in the model
        // @arg groupName The name of the group of contacts
        // @return the list of usernames for the specified group
        public List<string> GetConvRelList(string groupName)
        {
            Dictionary<string, List<string>> groupList = data.GetContactRelationshipDict();
            int relationLen = groupList.Count;

            if (relationLen != 0)
            {
                for (int i = 0; i < relationLen; i++)
                {
                    // TODO: Check if the conversation relationship list and return username list
                    return new List<string>();
                }
            }

            return new List<string>();
        }

        // Get User Object
        //
        // Get the user object from the list in the model
        // @arg username The username of the requested user object
        // @return the user object
        public ServerUser GetUserObj(string username)
        {
            List<ServerUser> usersList = data.GetUserList();
            int usersListLen = usersList.Count;

            if (usersListLen != 0)
            {
                for (int i = 0; i < usersListLen; i++)
                {
                    // TODO: Check if the user's username matches and return it
                    return new ServerUser();
                }
            }

            return new ServerUser();
        }

        // Is Username Used
        //
        // Check if hte username is currently being used by another user
        // @param name The username being requested
        public bool IsUsernameUsed(string name)
        {
            List<ServerUser> userList = data.GetUserList();
            int size = userList.Count;
            for (int i = 0; i < size; i++)
            {
                if (name == userList.ElementAt(i).GetUsername())
                {
                    return true;
                }
            }

            return false;
        }

        // Remove Conversation From List
        //
        // Remove the conversation from the list in the model
        // @arg name The name of the conversation
        public void RemoveConvFromList(string name)
        {
            // Get the list of all conversations from the model
            List<ServerConversation> convList = new List<ServerConversation>();

            // Get the conversation object from the list in the model
            ServerConversation conv = GetConvObj(name);

            // Remove the conversation object from the list of all conversations from the model
            convList.Remove(conv);

            // Set the conversation list to the updated list
            data.SetConversationList(convList);
        }

        // Remove Participant From Conversation Participants
        //
        // Remove the user with the specified username
        // @arg username The username for the current user
        // @arg name The name of the conversation
        public void RemoveParFromConvPars(string username, string name)
        {
            // Get the list of contact relationships
            Dictionary<string, List<string>> contRels = data.GetContactRelationshipDict();

            // Get the list of participants from the current conversation
            List<string> participants = GetConvParticipants(name);

            // Remove the current conversation group from the list
            contRels.Remove(name);

            // Add new user to the current contact relationship
            participants.Add(username);

            // Add the new contact relationship
            contRels.Add(name, participants);

            // Set the new list of participants as a new relationship
            data.SetContactRelationshipList(contRels);
        }

        // Remove User From List
        //
        // Remove the user from the list in the model
        // @arg username The username for the current user
        public void RemoveUserFromList(string username)
        {
            // Get the list of all users from the model
            List<ServerUser> userList = data.GetUserList();

            // Get the user object from the list in the model
            ServerUser user = GetUserObj(username);

            // Remove the user object from the list of all users from the model
            userList.Remove(user);

            // Set the user list to the updated list
            data.SetUserList(userList);
        }

        #region XML Serialization
        // Deserialize XML
        //
        // Deserialize the provided XML
        // @param msg The message containing the unparsed XML string
        // @return the dictionary containing the keywords and their values
        public Dictionary<string, string> DeserializeXml(string msg)
        {
            // Convert the message string to a XML object
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(msg);

            // Initialize the dictionary
            Dictionary<string, string> output = new Dictionary<string, string>();

            // Add the action element to the dictionary
            output.Add("action", xml.DocumentElement.Name);

            // Go through all the nodes in the provided XML object
            foreach (XmlElement node in xml)
            {
                // Go through all the attributes for the provided node
                XmlAttributeCollection attList = node.Attributes;
                foreach (XmlAttribute at in attList)
                {
                    // Add the current attribute and its value to the dictionary
                    output.Add(at.LocalName, at.Value);
                }
            }

            return output;
        }

        // Serialize XML
        //
        // Serialize the provided XML
        // @param infoDict The dictionary containing the information to be convert to XML
        // @return a string containing the message with the unparsed XML string
        public string SerializeXml(Dictionary<string, string> infoDict)
        {
            string output = "<";

            // Add the correct action to the XML string
            if (infoDict.ContainsKey("action"))
            {
                output += (infoDict["action"] + " ");

                infoDict.Remove("action");
            }
            else
            {
                output += "error ";
            }

            // Add each of the attributes to the XML string
            int size = infoDict.Count;
            for (int i = 0; i < size; i++)
            {
                KeyValuePair<string, string> curEle = infoDict.ElementAt(i);

                output += string.Format("{0}=\"{1}\" ", curEle.Key, curEle.Value);

                infoDict.Remove(curEle.Key);
            }

            output += "/>";

            return output;
        }
        #endregion
    }
}
