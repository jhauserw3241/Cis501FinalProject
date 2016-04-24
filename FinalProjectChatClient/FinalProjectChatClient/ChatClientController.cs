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
            switch (action)
            {
                case "AddCont":
                    break;
                case "RemoveCont":
                    break;
                case "CreateConv":
                    string name = (string)vars[0];
                    List<Contact> party = ((List<Contact>)vars[1]).Where(x => x.Status.Equals("Online")).ToList();
                    
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
                                clientForm.CreateConversationTab(name);
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
                    break;
                case "LeaveCont":
                    break;
                case "AddPart":
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
                        if (clientModel.State == FlowState.Main) exit = true;
                        break;
                    case DialogResult.No:
                        clientModel.State = FlowState.Access;
                        SignupAction();
                        while (clientModel.WaitFlag) { }
                        if (clientModel.State == FlowState.Main) exit = true;
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
                case "logout":
                    break;
                case "addCont":
                    break;
                case "rmCont":
                    break;
                case "leave":
                    break;
                case "crConv":
                    Contact initiator = clientModel.ContactList.Where(x => x.Username.Equals(mssg["from"])).First();
                    string name = mssg["to"];

                    clientModel.ConversationList.Add(name, new List<Contact> { initiator });
                    clientForm.CreateConversationTab(name);
                    break;
                case "addPa":
                    break;
                case "msg":
                    HandleChatMessage(mssg);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Private Methods

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
                clientModel.Username = mssg["ip"];
                clientModel.State = FlowState.Main;
                clientModel.Status = DispState.Online;
            }
        }

        /// <summary>
        /// Handles messages from the server containing chatlogs.
        /// </summary>
        /// <param name="mssg">The dictionary of keywords and their values.</param>
        private void HandleChatMessage(Dictionary<string, string> mssg)
        {
            Output("Message", clientModel.ConversationList[mssg["from"]], mssg["content"]);
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
            ws.Close();
        }

        #endregion
    }
}