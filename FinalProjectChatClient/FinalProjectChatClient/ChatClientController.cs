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
            List<Contact> party;
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
                    party = ((List<Contact>)vars[1]).Where(x => !x.Status.Equals("Offline")).ToList();

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

                    if (participant != null) AddConvParticipant(name, participant);
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
                        while (clientModel.WaitFlag) { }
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
                        while (clientModel.WaitFlag) { }
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
            Dictionary<string, string> mssg = ReadXML(e.Data);

            clientModel.WaitFlag = false;
            if (!mssg.ContainsKey("action")) return;

            switch (mssg["action"])
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
                case "leave":
                    HandleLeaveConvMessage(mssg);
                    break;
                case "crConv":
                    HandleConvCreateMessage(mssg);
                    break;
                case "addPa":
                    HandleParticipantMessage(mssg);
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
                        ChatClientForm.ShowError(mssg["error"]);
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
        private void AddConvParticipant(string name, Contact participant)
        {
            ws.Send(String.Format("<addPa username=\"{0}\" to=\"{1}\" />", participant.Username, name));
            // Wait for a response from the server
            clientModel.WaitFlag = true;
            while (clientModel.WaitFlag) { }
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
        private void CreateConversation(string name, List<Contact> party)
        {
            while (true)
            {
                // Make sure there are actually people in the conversation
                if (party.Count > 0)
                {
                    // Send initial request to server
                    ws.Send(String.Format("<crConv from=\"{0}\" to=\"{1}\"><content>{2}</content></crConv>", clientModel.Username, party[0].Username, name));
                    // Wait for a response from the server
                    clientModel.WaitFlag = true;
                    while (clientModel.WaitFlag) { }
                    // Make sure there were no errors
                    if (!clientModel.ErrorFlag)
                    {
                        // Add other member if there are some
                        for (int i = 1; i < party.Count; i++)
                        {
                            ws.Send(String.Format("<addPa username=\"{0}\" to=\"{1}\" />", party[i].Username, name));
                            // Wait for a response from the server
                            clientModel.WaitFlag = true;
                            while (clientModel.WaitFlag) { }
                            // If there was an error, remove that participant from the list and move on
                            if (clientModel.ErrorFlag)
                            {
                                party.RemoveAt(i);
                                clientModel.ErrorFlag = false;
                            }
                        }
                        // Update Client Side and leave loop
                        clientModel.ConversationList.Add(name, party);
                        if (Output != null) Output("CreateConv", name);
                        break;
                    }
                    // If there was an error, then remove the first contact and try to initialize the conversation with the next person
                    else
                    {
                        party.RemoveAt(0);
                        clientModel.ErrorFlag = false;
                    }
                }
                // If the party is empty, then show an error and leave the loop
                else
                {
                    ChatClientForm.ShowError("There was no one else in the conversation!");
                    break;
                }
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
        private void HandleAccessMessage(Dictionary<string, string> mssg)
        {
            if (mssg.ContainsKey("error"))
            {
                ChatClientForm.ShowError(mssg["error"]);
            }
            else
            {
                DataContractJsonSerializer srlzr = new DataContractJsonSerializer(typeof(List<Contact>));
                clientModel.ContactList = (List<Contact>)srlzr.ReadObject(new MemoryStream(Encoding.Default.GetBytes(mssg["content"])));
                clientModel.DisplayName = mssg["dispName"];
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
        private void HandleAddContactMessage(Dictionary<string, string> mssg)
        {
            if (mssg.ContainsKey("error"))
            {
                ChatClientForm.ShowError(mssg["error"]);
            }
            else
            {
                DataContractJsonSerializer srlzr = new DataContractJsonSerializer(typeof(Contact));
                Contact cont = (Contact)srlzr.ReadObject(new MemoryStream(Encoding.Default.GetBytes(mssg["content"])));
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
        private void HandleChatMessage(Dictionary<string, string> mssg)
        {
            Output("Message", mssg["from"], mssg["content"]);
        }

        /// <summary>
        /// Either tells the client that another user is starting a conversation with them, or informs the client of an error in their creation attempt.
        /// </summary>
        /// <param name="mssg">The contents of the message.</param>
        private void HandleConvCreateMessage(Dictionary<string, string> mssg)
        {
            if (mssg.ContainsKey("error"))
            {
                ChatClientForm.ShowError(mssg["error"]);
                clientModel.ErrorFlag = true;
            }
            else
            {
                Contact initiator = clientModel.ContactList.Find(x => x.Username.Equals(mssg["from"]));
                string name = mssg["to"];

                clientModel.ConversationList.Add(name, new List<Contact> { initiator });
                if (Output != null) Output("CreateConv", name);
            }
        }

        /// <summary>
        /// Handles when someone leaves the conversation.
        /// </summary>
        /// <param name="mssg">Who left what conversation.</param>
        private void HandleLeaveConvMessage(Dictionary<string, string> mssg)
        {
            List<Contact> conv = clientModel.ConversationList[mssg["from"]];
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals(mssg["username"]));

            conv.Remove(cont);
            Output("Message", mssg["from"], String.Format("{0} has left the conversation.", cont.DisplayName));
        }

        /// <summary>
        /// Updates the display name of a user in the conacts.
        /// </summary>
        /// <param name="mssg">The new name of the other user.</param>
        private void HandleNameChangeMessage(Dictionary<string, string> mssg)
        {
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals(mssg["username"]));
            if (cont != null)
            {
                cont.DisplayName = mssg["dispName"];
            }
        }

        /// <summary>
        /// Either adds a particpant to a conversation or informs the user of an error in adding a paricipant.
        /// </summary>
        /// <param name="mssg">The contents of this message.</param>
        private void HandleParticipantMessage(Dictionary<string, string> mssg)
        {
            if (mssg.ContainsKey("error"))
            {
                ChatClientForm.ShowError(mssg["error"]);
                clientModel.ErrorFlag = true;
            }
            else
            {
                Contact participant = clientModel.ContactList.Find(x => x.Username.Equals(mssg["from"]));
                string name = mssg["to"];

                if (clientModel.ConversationList.ContainsKey(name))
                {
                    clientModel.ConversationList[name].Add(participant);
                }
                else
                {
                    clientModel.ConversationList.Add(name, new List<Contact> { participant });
                    if (Output != null) Output("CreateConv", name);
                }
            }
        }

        /// <summary>
        /// Removes the contact provided by the server.
        /// </summary>
        /// <param name="mssg">The contact to remove.</param>
        private void HandleRemContactMessage(Dictionary<string, string> mssg)
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
        private void HandleStatusChangeMessage(Dictionary<string, string> mssg)
        {
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals(mssg["username"]));
            if (cont != null)
            {
                cont.Status = mssg["state"];

                if (mssg["state"].Equals("Offline"))
                {
                    foreach (KeyValuePair<string, List<Contact>> conv in clientModel.ConversationList)
                    {
                        if (conv.Value.Contains(cont))
                        {
                            conv.Value.Remove(cont);
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
        private Dictionary<string, string> ReadXML(string xml)
        {
            Dictionary<string, string> rtrn = new Dictionary<string, string>();
            XmlDocument message = new XmlDocument();
            string key = "";

            message.LoadXml(xml);
            foreach (XmlNode node in message)
            {
                switch (node.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (node.Attributes.Count > 0)
                        {
                            rtrn.Add("action", node.Name);

                            foreach (XmlAttribute attr in node.Attributes)
                            {
                                rtrn.Add(attr.Name, attr.Value);
                            }
                        }
                        else
                        {
                            key = node.Name;
                        }
                        break;
                    case XmlNodeType.Text:
                        rtrn.Add(key, node.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        if (node.Attributes.Count > 0)
                        {
                            rtrn.Add("action", node.Name);

                            foreach (XmlAttribute attr in node.Attributes)
                            {
                                rtrn.Add(attr.Name, attr.Value);
                            }
                        }
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