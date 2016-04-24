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
                    ws.Send(String.Format("<addCont username=\"{0}\" to=\"{1}\" />", (string)vars[0], clientModel.Username));
                    break;
                case "RemoveCont":
                    participant = clientModel.ContactList.Find(x => x.Username.Equals((string)vars[0]));

                    if (participant != null)
                    {
                        ws.Send(String.Format("<rmCont username=\"{0}\" from=\"{1}\" />", (string)vars[0], clientModel.Username));
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

                    ws.Send(String.Format("<leave username=\"{0}\" from=\"{1}\" />", clientModel.Username, page.Text));
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
                    ws.Send(String.Format("<udCont username=\"{0}\" state=\"{1}\" />", clientModel.Username, (string)vars[0]));
                    break;
                case "ChangeDispName":
                    clientModel.DisplayName = (string)vars[0];
                    if (Output != null) Output("UpdateName", (string)vars[0]);
                    ws.Send(String.Format("<udCont username=\"{0}\" dispName=\"{1}\" />", clientModel.Username, (string)vars[0]));
                    break;
                case "Message":
                    ws.Send(String.Format("<msg from=\"{0}\" to=\"{1}\"><content>{2}</content></msg>", clientModel.Username, clientModel.ConversationList[(string)vars[0]], FormatForChat((string)vars[1])));
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
            ws.Send(String.Format("<udCont dispName=\"{0}\"><addPa username=\"{1}\" /></udCont>", name, participant));
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
            StringBuilder send = new StringBuilder("<udConv dispName=\"" + name + "\">");
            
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
                DataContractJsonSerializer srlzr = new DataContractJsonSerializer(typeof(List<Contact>));
                clientModel.ContactList = (List<Contact>)srlzr.ReadObject(new MemoryStream(Encoding.Default.GetBytes((string)mssg["content"])));
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
                DataContractJsonSerializer srlzr = new DataContractJsonSerializer(typeof(Contact));
                Contact cont = (Contact)srlzr.ReadObject(new MemoryStream(Encoding.Default.GetBytes((string)mssg["content"])));
                if (cont != null)
                {
                    clientModel.ContactList.Add(cont);
                    if (Output != null) Output("AddCont", cont);
                }
            }
        }

        /// <summary>
        /// Handles messages from the server containing chatlogs.
        /// </summary>
        /// <param name="mssg">The dictionary of keywords and their values.</param>
        private void HandleChatMessage(Dictionary<string, object> mssg)
        {
            Output("Message", mssg["from"], mssg["content"]);
        }

        /// <summary>
        /// Handles when someone leaves the conversation.
        /// </summary>
        /// <param name="mssg">Who left what conversation.</param>
        private void HandleLeaveConvMessage(Dictionary<string, object> mssg)
        {
            List<string> conv = clientModel.ConversationList[(string)mssg["dispName"]];
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals(((Dictionary<string, List<string>>)mssg["leave"])["username"][0]));

            conv.Remove(cont.Username);
            Output("Message", mssg["dispName"], String.Format("{0} has left the conversation.", cont.DisplayName));
        }

        /// <summary>
        /// Updates the display name of a user in the conacts.
        /// </summary>
        /// <param name="mssg">The new name of the other user.</param>
        private void HandleNameChangeMessage(Dictionary<string, object> mssg)
        {
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals((string)mssg["username"]));
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
                string name = (string)mssg["dispName"];

                if (clientModel.ConversationList.ContainsKey(name))
                {
                    clientModel.ConversationList[name].AddRange(participants);
                }
                else
                {
                    clientModel.ConversationList.Add(name, participants);
                    if (Output != null) Output("CreateConv", name);
                }
            }
        }

        /// <summary>
        /// Removes the contact provided by the server.
        /// </summary>
        /// <param name="mssg">The contact to remove.</param>
        private void HandleRemContactMessage(Dictionary<string, object> mssg)
        {
            Contact participant = clientModel.ContactList.Find(x => x.Username.Equals(mssg["username"]));

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
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals((string)mssg["username"]));

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
                            Output("Message", mssg["from"], String.Format("{0} has left the conversation.", cont.DisplayName));
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
        /// Reads the provided string and returns a dictionary containing the data it was able to parse.
        /// </summary>
        /// <param name="xml">The xml string to parse.</param>
        /// <returns>A dictionary containing keywords and their corresponding values.</returns>
        private Dictionary<string, object> ReadXML(string xml)
        {
            Dictionary<string, object> rtrn = new Dictionary<string, object>();
            XmlDocument message = new XmlDocument();
            string key = "";

            message.LoadXml(xml);
            foreach (XmlNode node in message)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (key.Equals("udConv"))
                        {
                            // Get update action
                            if (node.Attributes.Count > 0)
                            {
                                // Make a new list if it doesn't already exist
                                if (!rtrn.ContainsKey(node.Name))
                                    rtrn.Add(node.Name, new Dictionary<string, List<string>>());

                                // Add value to list
                                foreach (XmlAttribute attr in node.Attributes)
                                {
                                    if (!((Dictionary<string, List<string>>)rtrn[node.Name]).ContainsKey(attr.Name))
                                    {
                                        ((Dictionary<string, List<string>>)rtrn[node.Name]).Add(attr.Name, new List<string>() { attr.Value });
                                    }
                                    else
                                    {
                                        ((Dictionary<string, List<string>>)rtrn[node.Name])[attr.Name].Add(attr.Value);
                                    }
                                }
                            }
                            // or some kind of content region
                            else
                            {
                                key = node.Name;
                            }
                        }
                        else
                        {
                            // If it isn't inside a udConv element then it's a regular action
                            if (node.Attributes.Count > 0)
                            {
                                // Determine action from element name
                                rtrn.Add("action", node.Name);
                                // Fill out other info from attributes
                                foreach (XmlAttribute attr in node.Attributes)
                                {
                                    rtrn.Add(attr.Name, attr.Value);
                                }
                                // If the action is to update a conversation
                                if (node.Name.Equals("udConv"))
                                {
                                    key = "udConv";
                                }
                            }
                            // or some kind of content region
                            else
                            {
                                key = node.Name;
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        rtrn.Add(key, node.Value);
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