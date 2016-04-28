using System;
using System.IO;
using System.Xml;
using System.Text;
using WebSocketSharp;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    class ChatClientController
    {
        #region Fields

        private ChatClientForm clientForm;
        private ChatClientModel clientModel;
        private EntryPopUp entryForm;
        private LoginPopUp loginForm;
        private SignupPopUp signupForm;
        private WebSocket ws;

        #endregion

        #region Properties

        public ChatClientForm ClientForm
        {
            get { return clientForm; }
            set { clientForm = value; }
        }
        public EntryPopUp EntryForm
        {
            set { entryForm = value; }
        }
        public LoginPopUp LoginForm
        {
            set { loginForm = value; }
        }
        public SignupPopUp SignupForm
        {
            set { signupForm = value; }
        }

        #endregion

        #region Events

        public event ClientOutputHandler Output;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor for the ChatClientController.
        /// </summary>
        /// <param name="model">The model the constructor will be working with.</param>
        public ChatClientController(ChatClientModel model)
        {
            clientModel = model;
            ws = new WebSocket("ws://127.0.0.1:8001/chat");
            ws.OnMessage += HandleMessage;
            ws.Connect();
        }

        /// <summary>
        /// Delegates input from the form to various methods.
        /// </summary>
        /// <param name="action">The action the form is trying to perform.</param>
        public void HandleFormInput(string action, params object[] vars)
        {
            List<string> party;
            Contact participant;
            TabPage page;
            string name;

            switch (action)
            {
                case "Logout":
                    LogoutAction();
                    clientModel.State = FlowState.Entry;
                    HandleLoadIn(clientForm, new EventArgs());
                    break;
                case "AddCont":
                    ws.Send(String.Format("<addCont source=\"{0}\" username=\"{1}\" />", clientModel.Username, (string)vars[0]));
                    break;
                case "RemoveCont":
                    participant = clientModel.ContactList.Find(x => x.Username.Equals((string)vars[0]));

                    if (participant != null)
                    {
                        ws.Send(String.Format("<rmCont source=\"{0}\" username=\"{1}\" />", clientModel.Username, (string)vars[0]));
                        clientModel.ContactList.Remove(participant);
                        if (Output != null) Output("RemoveCont", participant);
                    }
                    break;
                case "CreateConv":
                    name = (string)vars[0];
                    // Only get those members who are explicity not online
                    party = ((List<Contact>)vars[1]).Where(x => !x.Status.Equals("Offline")).Select(x => x.Username).ToList();

                    CreateConversation(name, party);
                    break;
                case "LeaveConv":
                    page = (TabPage)vars[0];

                    ws.Send(String.Format("<udConv conv=\"{0}\"><leave username=\"{1}\" />", page.Text, clientModel.Username));
                    clientModel.ConversationList.Remove(page.Text);
                    if (Output != null) Output("LeaveConv", page);
                    break;
                case "AddPart":
                    name = (string)vars[0];
                    participant = clientModel.ContactList.Find(x => x.Username.Equals((string)vars[1]));

                    if (participant != null) AddConvParticipant(name, participant.Username);
                    else
                    {
                        ChatClientForm.ShowError("Username does not exist.");
                    }
                    break;
                case "ChangeStatus":
                    clientModel.Status = (string)vars[0];
                    if (Output != null) Output("UpdateStatus", (string)vars[0]);
                    ws.Send(String.Format("<udCont source=\"{0}\" state=\"{1}\" />", clientModel.Username, (string)vars[0]));
                    break;
                case "ChangeDispName":
                    clientModel.DisplayName = (string)vars[0];
                    if (Output != null) Output("UpdateName", (string)vars[0]);
                    ws.Send(String.Format("<udCont source=\"{0}\" dispName=\"{1}\" />", clientModel.Username, (string)vars[0]));
                    break;
                case "Message":
                    ws.Send(String.Format("<msg source=\"{0}\" conv=\"{1}\">{2}</msg>", clientModel.Username, clientModel.ConversationList[(string)vars[0]], FormatForChat((string)vars[1])));
                    break;
            }
        }

        /// <summary>
        /// Run entry loop until user manages to login or closes.
        /// </summary>
        public void HandleLoadIn(object sender, EventArgs e)
        {
            DialogResult st = DialogResult.None;
            bool exit = false;

            while (!exit)
            {
                if (clientModel.State == FlowState.Entry) st = entryForm.ShowDialog();
                switch (st)
                {
                    case DialogResult.Yes:
                        clientModel.State = FlowState.Access;
                        LoginAction();
                        while (clientModel.WaitFlag)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                        if (clientModel.State == FlowState.Main)
                        {
                            clientModel.Username = loginForm.Username;
                            exit = true;
                        }
                        exit = true;
                        break;
                    case DialogResult.No:
                        clientModel.State = FlowState.Access;
                        SignupAction();
                        while (clientModel.WaitFlag)
                        {
                            System.Threading.Thread.Sleep(1000);
                        }
                        if (clientModel.State == FlowState.Main)
                        {
                            clientModel.Username = signupForm.Username;
                            exit = true;
                        }
                        break;
                    case DialogResult.Cancel:
                        Application.Exit();
                        break;
                }
            }
        }

        /// <summary>
        /// Hub for all incoming messages. Will be redistributed to seperate methods for handeling.
        /// </summary>
        public void HandleMessage(object sender, MessageEventArgs e)
        {
            Dictionary<string, object> mssg = ReadXML(e.Data);

            clientModel.WaitFlag = false;
            if (!mssg.ContainsKey("action")) return;

            switch ((string)mssg["action"])
            {
                case "sign":
                case "login":
                    HandleAccessMessage(mssg);
                    break;
                case "addCont":
                    HandleAddContactMessage(mssg);
                    break;
                case "rmCont":
                    HandleRemContactMessage(mssg);
                    break;
                case "udConv":
                    if (mssg.ContainsKey("addPa"))
                    {
                        HandleParticipantMessage(mssg);
                    }
                    else if (mssg.ContainsKey("leave"))
                    {
                        HandleLeaveConvMessage(mssg);
                    }
                    break;
                case "udCont":
                    if (mssg.ContainsKey("state"))
                    {
                        HandleStatusChangeMessage(mssg);
                    }
                    else if (mssg.ContainsKey("dispName"))
                    {
                        HandleNameChangeMessage(mssg);
                    }
                    break;
                case "msg":
                    HandleChatMessage(mssg);
                    break;
                default:
                    if (mssg.ContainsKey("error"))
                    {
                        ChatClientForm.ShowError((string)mssg["error"]);
                        clientModel.ErrorFlag = true;
                    }
                    break;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Attempts to add a participant to a conversation.
        /// </summary>
        /// <param name="name">The conversation name.</param>
        /// <param name="participant">The participant to add.</param>
        private void AddConvParticipant(string name, string participant)
        {
            ws.Send(String.Format("<udCont conv=\"{0}\"><addPa username=\"{1}\" /></udCont>", name, participant));
            // Wait for a response from the server
            clientModel.WaitFlag = true;
            while (clientModel.WaitFlag)
            {
                System.Threading.Thread.Sleep(1000);
            }
            // If there was no error, add participant to client side
            if (!clientModel.ErrorFlag)
            {
                clientModel.ConversationList[name].Add(participant);
            }
            else
            {
                clientModel.ErrorFlag = false;
            }
        }

        /// <summary>
        /// Attempts to create a conversation with as many of the contacts provided as possible.
        /// </summary>
        /// <param name="name">The name of the conversation group.</param>
        /// <param name="party">The participants in the conversation.</param>
        private void CreateConversation(string name, List<string> party)
        {
            StringBuilder send = new StringBuilder("<udConv conv=\"" + name + "\">");
            
            // Make sure there are actually people in the conversation
            if (party.Count > 0)
            {
                // Build message to send
                for (int i = 0; i < party.Count; i++)
                {
                    send.Append("<addPa username=\"" + party[i] + "\" />");
                }
                send.Append("</udConv>");

                // Send initial request to server
                ws.Send(send.ToString());
                // Wait for a response from the server
                clientModel.WaitFlag = true;
                while (clientModel.WaitFlag)
                {
                    System.Threading.Thread.Sleep(1000);
                }
                // Make sure there were no errors
                if (!clientModel.ErrorFlag)
                {
                    // Update Client Side and leave loop
                    clientModel.ConversationList.Add(name, party);
                    if (Output != null) Output("CreateConv", name);
                }
            }
            // If the party is empty, then show an error and leave the loop
            else
            {
                ChatClientForm.ShowError("There was no one else in the conversation!");
            }
        }

        /// <summary>
        /// Returns the message in proper chat formating.
        /// </summary>
        /// <param name="msg">The content of the message.</param>
        /// <returns></returns>
        private string FormatForChat(string msg)
        {
            return String.Format("[{0:MM/dd/yyyy hh:mm:sstt}] {1}: {2}{3}", DateTime.Now, clientModel.DisplayName, msg, Environment.NewLine);
        }

        /// <summary>
        /// Handles messages from the server containing information about signing up or logging in.
        /// </summary>
        /// <param name="mssg">The dictionary of keywords and their values.</param>
        private void HandleAccessMessage(Dictionary<string, object> mssg)
        {
            if (mssg.ContainsKey("error"))
            {
                ChatClientForm.ShowError((string)mssg["error"]);
            }
            else
            {
                clientModel.ContactList = new List<Contact>();
                foreach (Tuple<string, string, string> cont in (List<Tuple<string, string, string>>)mssg["cont"])
                {
                    clientModel.ContactList.Add(new Contact(cont.Item1, cont.Item2, cont.Item3));
                }
                clientModel.DisplayName = (string)mssg["dispName"];
                clientModel.State = FlowState.Main;
                clientModel.Status = "Online";
                if (Output != null)
                {
                    Output("UpdateStatus", "Online");
                    Output("UpdateName", clientModel.DisplayName);
                }
            }
        }

        /// <summary>
        /// Adds the provided contact to the contact list, or diplays an error.
        /// </summary>
        /// <param name="mssg">The content of this message.</param>
        private void HandleAddContactMessage(Dictionary<string, object> mssg)
        {
            if (mssg.ContainsKey("error"))
            {
                ChatClientForm.ShowError((string)mssg["error"]);
            }
            else
            {
                Contact cont = new Contact((string)mssg["username"], (string)mssg["dispName"], (string)mssg["state"]);
                clientModel.ContactList.Add(cont);
                if (Output != null) Output("AddCont", cont);
            }
        }

        /// <summary>
        /// Handles messages from the server containing chatlogs.
        /// </summary>
        /// <param name="mssg">The dictionary of keywords and their values.</param>
        private void HandleChatMessage(Dictionary<string, object> mssg)
        {
            Output("Message", mssg["conv"], mssg["msg"]);
        }

        /// <summary>
        /// Handles when someone leaves the conversation.
        /// </summary>
        /// <param name="mssg">Who left what conversation.</param>
        private void HandleLeaveConvMessage(Dictionary<string, object> mssg)
        {
            List<string> conv = clientModel.ConversationList[(string)mssg["conv"]];
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals((string)mssg["leave"]));

            conv.Remove(cont.Username);
            Output("Message", (string)mssg["conv"], String.Format("{0} has left the conversation.", cont.DisplayName));
        }

        /// <summary>
        /// Updates the display name of a user in the conacts.
        /// </summary>
        /// <param name="mssg">The new name of the other user.</param>
        private void HandleNameChangeMessage(Dictionary<string, object> mssg)
        {
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals((string)mssg["source"]));
            if (cont != null)
            {
                cont.DisplayName = (string)mssg["dispName"];
            }
        }

        /// <summary>
        /// Either adds a particpant to a conversation or informs the user of an error in adding a paricipant.
        /// </summary>
        /// <param name="mssg">The contents of this message.</param>
        private void HandleParticipantMessage(Dictionary<string, object> mssg)
        {
            Dictionary<string, List<string>> content = (Dictionary<string, List<string>>)mssg["addPa"];

            if (content.ContainsKey("error"))
            {
                ChatClientForm.ShowError(content["error"][0]);
                clientModel.ErrorFlag = true;
            }
            else
            {
                List<string> participants = content["username"];
                string name = (string)mssg["conv"];

                if (clientModel.ConversationList.ContainsKey(name))
                {
                    clientModel.ConversationList[name].AddRange(participants);
                }
                else
                {
                    clientModel.ConversationList.Add(name, participants);
                    if (Output != null)
                    {
                        Output("CreateConv", name);
                        Output("Message", name, mssg["msg"]);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the contact provided by the server.
        /// </summary>
        /// <param name="mssg">The contact to remove.</param>
        private void HandleRemContactMessage(Dictionary<string, object> mssg)
        {
            Contact participant = clientModel.ContactList.Find(x => x.Username.Equals(mssg["source"]));

            if (participant != null)
            {
                clientModel.ContactList.Remove(participant);
                if (Output != null) Output("RemoveCont", participant);
            }
        }

        /// <summary>
        /// Updates the status of a user in the conacts.
        /// </summary>
        /// <param name="mssg">The status of the other user.</param>
        private void HandleStatusChangeMessage(Dictionary<string, object> mssg)
        {
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals((string)mssg["source"]));

            if (cont != null)
            {
                cont.Status = (string)mssg["state"];

                if (mssg["state"].Equals("Offline"))
                {
                    foreach (KeyValuePair<string, List<string>> conv in clientModel.ConversationList)
                    {
                        if (conv.Value.Contains(cont.Username))
                        {
                            conv.Value.Remove(cont.Username);
                            Output("Message", conv.Key, String.Format("{0} has left the conversation.", cont.DisplayName));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays and interprets info from a login popup.
        /// </summary>
        /// <returns>Whether or not to exit the loop.</returns>
        private void LoginAction()
        {
            switch (loginForm.ShowDialog())
            {
                case DialogResult.OK:
                    if (!loginForm.Username.Equals(String.Empty))
                    {
                        if (!loginForm.Password.Equals(String.Empty))
                        {
                            ws.Send(String.Format("<login username=\"{0}\" password=\"{1}\" />", signupForm.Username, signupForm.Password1));
                            clientModel.WaitFlag = true;
                        }
                        else
                        {
                            ChatClientForm.ShowError("The password cannot be empty.");
                        }
                    }
                    else
                    {
                        ChatClientForm.ShowError("The username cannot be empty.");
                    }
                    break;
                case DialogResult.Cancel:
                    clientModel.State = FlowState.Entry;
                    break;
            }
        }
        
        /// <summary>
        /// Logs out of the current user session.
        /// </summary>
        private void LogoutAction()
        {
            DataContractJsonSerializer srlzr = new DataContractJsonSerializer(typeof(List<Contact>));
            MemoryStream contList = new MemoryStream(); 
            srlzr.WriteObject(contList, clientForm.ContactsList);
            ws.Send(String.Format("<logout username=\"{0}\"><content>{1}</content></logout>", clientModel.Username, contList.ToString()));
        }

        /// <summary>
        /// Parses a xml string and returns the information in a more accessable form
        /// </summary>
        /// <param name="xml">The xml string to parse.</param>
        /// <returns>A dictionary containing keywords as keys and appropriately matched values.</returns>
        private Dictionary<string, object> ReadXML(string xml)
        {
            Dictionary<string, object> rtrn = new Dictionary<string, object>();
            XmlDocument message = new XmlDocument();
            Stack<string> layers = new Stack<string>();
            Dictionary<string, List<string>> dict;
            Tuple<string, string, string> tups;
            string top;

            message.LoadXml(xml);

            foreach(XmlElement node in message)
            {
                // Get the top layer
                top = layers.Peek() == null ? "default" : layers.Peek();

                switch (node.NodeType)
                {
                    case XmlNodeType.Element:
                        // Differentiate action based on what group we are in
                        switch(top)
                        {
                            case "udConv":
                                switch(node.Name)
                                {
                                    case "addPa":
                                        // If there is not already an entry for addPa
                                        if (!rtrn.ContainsKey("addPa"))
                                        {
                                            // Create a dictionary where the key is the attribute name, and the value is a list of all the values associated with that attribute
                                            rtrn.Add("addPa", new Dictionary<string, List<string>>());
                                        }

                                        dict = rtrn["addPa"] as Dictionary<string, List<string>>;

                                        foreach (XmlAttribute attr in node.Attributes)
                                        {
                                            // If the dictionary does not contain an instance of this key create a new list
                                            if (!dict.ContainsKey(attr.Name))
                                            {
                                                dict.Add(attr.Name, new List<string>());
                                            }
                                            // Add the value to the list
                                            dict[attr.Name].Add(attr.Value);
                                        }
                                        break;
                                    case "leave":
                                        // Add or update the leave key (leave should only be used once per message so it's unlikely to update a value)
                                        rtrn["leave"] = node.Attributes[0].Value;
                                        break;
                                    case "msg":
                                        // Create a new list of strings for the transcript of the conversation
                                        rtrn["msg"] = new List<string>();
                                        break;
                                }
                                break;
                            case "login":
                                // If this is the first contact
                                if (!rtrn.ContainsKey("cont"))
                                {
                                    // Create a list of tuples, where the tuple represents a contact
                                    rtrn.Add("cont", new List<Tuple<string, string, string>>());
                                }
                                // Construct a tuple from the attributes and add it to the list
                                tups = new Tuple<string, string, string>(node.Attributes[0].Value, node.Attributes[1].Value, node.Attributes[2].Value);
                                ((List<Tuple<string, string, string>>)rtrn["cont"]).Add(tups);
                                break;
                            default: // Presumably this is the top of the message
                                // Meaning the element name is the desired action
                                rtrn.Add("action", node.Name);
                                // Gather info from attributes
                                foreach (XmlAttribute attr in node.Attributes)
                                {
                                    rtrn.Add(attr.Name, attr.Value);
                                }
                                break;
                        }

                        // If entering a group, then add to the stack
                        if (!node.IsEmpty) layers.Push(node.Name);
                        break;
                    case XmlNodeType.Text:
                        // If the dictionary does not contain a key with the group name
                        if (!rtrn.ContainsKey(top))
                        {
                            // Create a list to add multiple items to
                            rtrn.Add(top, new List<string>());
                        }
                        // Add the text
                        ((List<string>)rtrn[top]).Add(node.Value);
                        break;
                    case XmlNodeType.EndElement:
                        // Leave the current group
                        if (layers.Count > 0) layers.Pop();
                        break;
                }
            }

            return rtrn;
        }

        /// <summary>
        /// Displays and interprets info from a signup popup.
        /// </summary>
        /// <returns>Whether or not to exit the loop.</returns>
        private void SignupAction()
        {
            switch (signupForm.ShowDialog())
            {
                case DialogResult.OK:
                    if (!signupForm.Username.Equals(String.Empty))
                    {

                        if (!signupForm.Password1.Equals(String.Empty))
                        {

                            if (signupForm.Password1.Equals(signupForm.Password2))
                            {
                                ws.Send(String.Format("<sign username=\"{0}\" password=\"{1}\" />", signupForm.Username, signupForm.Password1));
                                clientModel.WaitFlag = true;
                            }
                            else
                            {
                                ChatClientForm.ShowError("The passwords do not match.");
                            }
                        }
                        else
                        {
                            ChatClientForm.ShowError("The password cannot be empty.");
                        }
                    }
                    else
                    {
                        ChatClientForm.ShowError("The username cannot be empty.");
                    }
                    break;
                case DialogResult.Cancel:
                    clientModel.State = FlowState.Entry;
                    break;
            }
        }

        /// <summary>
        /// Deconstructor for the ChatClientController.
        /// </summary>
        ~ChatClientController()
        {
            if (clientModel.State == FlowState.Main) LogoutAction();
            ws.Close();
        }

        #endregion
    }
}