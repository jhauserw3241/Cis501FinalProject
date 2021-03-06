﻿using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using WebSocketSharp;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public class ChatClientController
    {
        #region Fields
        
        private ChatClientModel clientModel;
        private ChatClientForm clientForm;
        private ConvCreatePopUp createForm;
        private EntryPopUp entryForm;
        private LoginPopUp loginForm;
        private SignupPopUp signupForm;
        private WaitForm waitForm;
        private WebSocket wsa, wsc;

        #endregion
        
        #region Properties
        
        public ChatClientForm ClientForm
        {
            get { return clientForm; }
            set { clientForm = value; }
        }
        public ConvCreatePopUp CreateForm
        {
            set { createForm = value; }
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
        
        /// <summary>
        /// Constructor for the ChatClientController.
        /// </summary>
        /// <param name="model">The model the constructor will be working with.</param>
        public ChatClientController()
        {
            clientModel = ChatClientModel.Instance;
            waitForm = new WaitForm();
        }

        #region Input Handlers

        /// <summary>
        /// Handles input that takes generic event arguments.
        /// </summary>
        /// <param name="sender">The component that raised the event.</param>
        /// <param name="e">The supplied arguments.</param>
        public void HandleGenericInput(object sender, EventArgs e)
        {
            if (sender.Equals(clientForm.AwayStatusOption))
            {
                clientModel.Status = "Away";
                wsc.Send(String.Format("<udCont source=\"{0}\" state=\"Away\" />", clientModel.Username));
                if (Output != null) Output("UpdateStatus");
            }
            else if (sender.Equals(clientForm.OnlineStatusOption))
            {
                clientModel.Status = "Online";
                wsc.Send(String.Format("<udCont source=\"{0}\" state=\"Online\" />", clientModel.Username));
                if (Output != null) Output("UpdateStatus");
            }
            else if (sender.Equals(clientForm.CreateConversationOption))
            {
                if (createForm.ShowDialog() == DialogResult.OK)
                {
                    string name = createForm.ConvName;

                    if (!clientModel.ConversationList.ContainsKey(name))
                    {
                        // Only get those members who are explicity not online
                        List<string> party = (createForm.ParticipantListBox.Cast<Contact>().ToList()).Where(x => !x.Status.Equals("Offline")).Select(x => x.Username).ToList();

                        CreateConversation(name, party);
                    }
                    else ChatClientForm.ShowError("This conversation name already exists.");
                }
            }
            else if (sender.Equals(clientForm.LeaveConversationOption))
            {
                TabPage page = clientForm.ConversationTabController.SelectedTab;

                wsc.Send(String.Format("<udConv conv=\"{0}\" leave=\"{1}\" />", page.Text, clientModel.Username));
                clientModel.ConversationList.Remove(page.Text);
                if (Output != null) Output("LeaveConv");
            }
            else if (sender.Equals(clientForm.LogoutProfileOption))
            {
                LogoutAction();
                clientModel.State = FlowState.Entry;
                HandleLoadIn(null, new EventArgs());
            }
        }

        /// <summary>
        /// Handles input that takes key event arguments.
        /// </summary>
        /// <param name="sender">The component that raised the event.</param>
        /// <param name="e">The supplied arguments.</param>
        public void HandleKeyInput(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (sender.Equals(clientForm.AddContactTextBox))
                {
                    e.SuppressKeyPress = true;
                    if (!clientForm.AddContactTextBox.Text.Equals(String.Empty))
                    {
                        wsc.Send(String.Format("<addCont source=\"{0}\" username=\"{1}\" />", clientModel.Username, clientForm.AddContactTextBox.Text));

                        WaitForResponse();
                        if (Output != null) Output("ClrAddCont");
                    }
                }
                else if (sender.Equals(clientForm.RemoveContactTextBox))
                {
                    if (!clientForm.RemoveContactTextBox.Text.Equals(String.Empty))
                    {
                        Contact participant = clientModel.ContactList.Find(x => x.Username.Equals(clientForm.RemoveContactTextBox.Text));

                        if (participant != null)
                        {
                            wsc.Send(String.Format("<rmCont source=\"{0}\" username=\"{1}\" />", clientModel.Username, participant.Username));
                            WaitForResponse();
                        }
                        else
                        {
                            ChatClientForm.ShowError("That username is not in your contacts.");
                        }

                        if (Output != null) Output("ClrRemCont");
                    }
                    e.SuppressKeyPress = true;
                }
                else if (sender.Equals(clientForm.AddParticipantTextBox))
                {
                    if (clientForm.ConversationTabController.Controls.Count > 0 && !clientForm.AddParticipantTextBox.Text.Equals(String.Empty))
                    {
                        string name = clientForm.ConversationTabController.SelectedTab.Text;
                        Contact participant = clientModel.ContactList.Find(x => x.Username.Equals(clientForm.AddParticipantTextBox.Text));

                        if (participant != null)
                        {
                            AddConvParticipant(name, participant.Username);
                            if (Output != null) Output("AddPart");
                        }
                        else
                        {
                            ChatClientForm.ShowError("Username does not exist.");
                        }
                    }
                    e.SuppressKeyPress = true;
                }
                else if (sender.Equals(clientForm.RenameConversationTextBox))
                {
                    if (clientForm.ConversationTabController.Controls.Count > 0 && !clientForm.RenameConversationTextBox.Text.Equals(String.Empty))
                    {
                        string name = clientForm.ConversationTabController.SelectedTab.Text;
                        
                        RenameConv(name, clientForm.RenameConversationTextBox.Text);
                        if (Output != null) Output("RenameConv", name, clientForm.RenameConversationTextBox.Text);
                    }
                    e.SuppressKeyPress = true;
                }
                else if (sender.Equals(clientForm.MessageBox))
                {
                    if (e.Modifiers != Keys.Shift)
                    {
                        if (clientForm.ConversationTabController.Controls.Count > 0)
                        {
                            wsc.Send(String.Format("<msg source=\"{0}\" conv=\"{1}\" text=\"{2}\" />", clientModel.Username, clientForm.ConversationTabController.SelectedTab.Text, FormatForChat(clientForm.MessageBox.Text)));
                            if (Output != null) Output("ClrMsg");
                            clientModel.WaitFlag = true;
                            while (clientModel.WaitFlag)
                            {
                                System.Threading.Thread.Sleep(500);
                            }
                        }
                        e.SuppressKeyPress = true;
                    }
                }
                else if (sender.Equals(clientForm.ChangeDispNameTextBox))
                {
                    if (!clientForm.ChangeDispNameTextBox.Text.Equals(String.Empty))
                    {
                        clientModel.DisplayName = clientForm.ChangeDispNameTextBox.Text;
                        wsc.Send(String.Format("<udCont source=\"{0}\" dispName=\"{1}\" />", clientModel.Username, clientModel.DisplayName));
                        if (Output != null) Output("UpdateName");
                    }
                    e.SuppressKeyPress = true;
                }
            }
        }

        /// <summary>
        /// Handles input that takes mouse event arguments.
        /// </summary>
        /// <param name="sender">The component that raised the event.</param>
        /// <param name="e">The supplied arguments.</param>
        public void HandleMouseInput(object sender, MouseEventArgs e)
        {
            if (sender.Equals(clientForm.ContactsList) && clientModel.ContactList.Count > 0)
            {
                Contact cont = ((Contact)clientForm.ContactsList.SelectedItem);
                string name = cont.DisplayName;

                if (!clientModel.ConversationList.ContainsKey(name) && !cont.Status.Equals("Offline"))
                {
                    CreateConversation(name, new List<string>() { cont.Username });
                }
                else ChatClientForm.ShowError("Could not create a conversation:\nEither one has already been started, or they are offline.");
            }
        }

        /// <summary>
        /// Run entry loop until user manages to login or closes.
        /// </summary>
        public void HandleLoadIn(object sender, EventArgs e)
        {
            DialogResult st = DialogResult.None;
            bool exit = false;

            wsa = new WebSocket("ws://127.0.0.1:8001/Access");
            wsa.OnMessage += HandleMessage;
            wsa.Connect();

            while (!exit)
            {
                if (clientModel.State == FlowState.Entry) st = entryForm.ShowDialog();
                switch (st)
                {
                    case DialogResult.Yes:
                        clientModel.State = FlowState.Access;
                        LoginAction();
                        if (clientModel.State == FlowState.Main)
                        {
                            clientModel.Username = loginForm.Username;
                            loginForm.ClearTextFields();
                            exit = true;
                            
                            wsc = new WebSocket("ws://127.0.0.1:8001/" + clientModel.Username);
                            wsc.OnMessage += HandleMessage;
                            wsc.Connect();
                        }
                        else if (clientModel.ErrorFlag)
                        {
                            wsa.Send("<login username=\"" + loginForm.Username + "\" error=\"Invalid contact list.\">");
                            clientModel.ErrorFlag = false;
                        }
                        break;
                    case DialogResult.No:
                        clientModel.State = FlowState.Access;
                        SignupAction();
                        if (clientModel.State == FlowState.Main)
                        {
                            clientModel.Username = signupForm.Username;
                            clientModel.DisplayName = clientModel.Username;
                            signupForm.ClearTextFields();
                            if (Output != null) Output("UpdateName");
                            exit = true;

                            //ws.Close();
                            wsc = new WebSocket("ws://127.0.0.1:8001/" + clientModel.Username);
                            wsc.OnMessage += HandleMessage;
                            wsc.Connect();
                        }
                        break;
                    case DialogResult.Cancel:
                        exit = true;
                        wsa.Close();
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
                case "udConv":
                    if (mssg.ContainsKey("leave"))
                    {
                        HandleLeaveConvMessage(mssg);
                    }
                    else if (mssg.ContainsKey("par"))
                    {
                        HandleParticipantMessage(mssg);
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
                case "logout":
                    HandleLogoutMessage(mssg);
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

        /// <summary>
        /// Parses a xml string and returns the information in a more accessable form
        /// </summary>
        /// <param name="xml">The xml string to parse.</param>
        /// <returns>A dictionary containing keywords as keys and appropriately matched values.</returns>
        private Dictionary<string, string> ReadXML(string xml)
        {
            Dictionary<string, string> rtrn = new Dictionary<string, string>();
            XmlDocument message = new XmlDocument();
            XmlElement node;

            message.LoadXml(xml);
            node = message.DocumentElement;

            // Get the action from the element name
            rtrn.Add("action", node.Name);

            // Get other data from the element's attributes
            foreach (XmlAttribute attr in node.Attributes)
            {
                rtrn.Add(attr.Name, attr.Value);
            }

            return rtrn;
        }

        #endregion

        #region Conversation

        /// <summary>
        /// Attempts to add a participant to a conversation.
        /// </summary>
        /// <param name="name">The conversation name.</param>
        /// <param name="participant">The participant to add.</param>
        private void AddConvParticipant(string name, string participant)
        {
            wsc.Send(String.Format("<udConv conv=\"{0}\" par=\"{1}\" />", name, participant));
            // Wait for a response from the server
            WaitForResponse();
            // If there was no error, add participant to client side
            if (clientModel.ErrorFlag)
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
            party.Add(clientModel.Username);
            // Make sure there are actually people in the conversation
            if (party.Count > 1)
            {
                string conts = String.Join(",", party);

                // Send initial request to server
                wsc.Send(String.Format("<udConv conv=\"{0}\" par=\"{1}\" />", name, conts));
                // Wait for a response from the server
                WaitForResponse();
            }
            // If the party is empty, then show an error and leave the loop
            else
            {
                ChatClientForm.ShowError("There is no one to participate in the conversation!");
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
        /// Renames a conversation.
        /// </summary>
        /// <param name="conv">The conversation to rename.</param>
        /// <param name="newName">The new name of the conversation.</param>
        private void RenameConv(string conv, string newName)
        {
            // Send initial request to server
            wsc.Send(String.Format("<udConv conv=\"{0}\" newConv=\"{1}\" />", conv, newName));
            // Wait for a response from the server
            WaitForResponse();
            // Make sure there were no errors
            if (!clientModel.ErrorFlag)
            {
                // Update Client Side and leave loop
                clientModel.ConversationList.Add(newName, clientModel.ConversationList[conv]);
                clientModel.ConversationList.Remove(conv);
                if (Output != null) Output("CreateConv", conv, newName);
            }
        }

        #endregion

        #region Message Handlers

        /// <summary>
        /// Handles messages from the server containing information about signing up or logging in.
        /// </summary>
        /// <param name="mssg">The dictionary of keywords and their values.</param>
        private void HandleAccessMessage(Dictionary<string, string> mssg)
        {
            string[] un = new string[0],
                dn = new string[0],
                st = new string[0];

            if (mssg.ContainsKey("error"))
            {
                ChatClientForm.ShowError(mssg["error"]);
            }
            else if (mssg["action"].Equals("login"))
            {
                if (mssg.ContainsKey("contUsername"))
                {
                    un = mssg["contUsername"].Split(',');
                }
                if (mssg.ContainsKey("contDisplName"))
                {
                    dn = mssg["contDisplName"].Split(',');
                }
                if (mssg.ContainsKey("contState"))
                {
                    st = mssg["contState"].Split(',');
                }

                if (un.Length == dn.Length && un.Length == st.Length)
                {
                    clientModel.DisplayName = mssg["dispName"];
                    for (int i = 0; i < un.Length; i++)
                    {
                        clientModel.ContactList.Add(new Contact(un[i], dn[i], st[i]));
                    }
                    clientModel.State = FlowState.Main;
                    clientModel.Status = "Online";
                    if (Output != null)
                    {
                        Output("UpdateStatus");
                        Output("UpdateName");
                        Output("UpdateContList");
                    }
                }
                else
                {
                    clientModel.ErrorFlag = true;
                }
            }
            else if (mssg["action"].Equals("sign"))
            {
                clientModel.State = FlowState.Main;
                clientModel.Status = "Online";
                if (Output != null)
                {
                    Output("UpdateStatus");
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
                Contact cont = new Contact(mssg["username"], mssg["dispName"], mssg["state"]);
                clientModel.ContactList.Add(cont);
                if (Output != null) Output("AddCont");
            }
        }

        /// <summary>
        /// Handles messages from the server containing chatlogs.
        /// </summary>
        /// <param name="mssg">The dictionary of keywords and their values.</param>
        private void HandleChatMessage(Dictionary<string, string> mssg)
        {
            if (mssg.ContainsKey("error"))
            {
                Output("Message", mssg["conv"], mssg["error"]);
            }
            else
            {
                Output("Message", mssg["conv"], mssg["text"]);
            }
        }

        /// <summary>
        /// Handles when someone leaves the conversation.
        /// </summary>
        /// <param name="mssg">Who left what conversation.</param>
        private void HandleLeaveConvMessage(Dictionary<string, string> mssg)
        {
            List<string> conv = clientModel.ConversationList[mssg["conv"]];
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals(mssg["leave"]));

            conv.Remove(cont.Username);
            if (Output != null) Output("Message", mssg["conv"], String.Format("{0} has left the conversation.", cont.DisplayName));
        }

        /// <summary>
        /// A response from the server to determine if logoff was successful.
        /// </summary>
        /// <param name="mssg">The information in the message.</param>
        private void HandleLogoutMessage(Dictionary<string, string> mssg)
        {
            if (mssg.ContainsKey("error"))
            {
                ChatClientForm.ShowError(mssg["error"]);
            }
            else
            {
                clientModel.State = FlowState.Exit;
            }
        }

        /// <summary>
        /// Updates the display name of a user in the conacts.
        /// </summary>
        /// <param name="mssg">The new name of the other user.</param>
        private void HandleNameChangeMessage(Dictionary<string, string> mssg)
        {
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals(mssg["source"]));
            if (cont != null)
            {
                cont.DisplayName = mssg["dispName"];

                if (Output != null) Output("UpdateContList");
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
                string[] participants = mssg["par"].Split(',');
                string name = mssg["conv"];

                if (clientModel.ConversationList.ContainsKey(name))
                {
                    List<string> party = clientModel.ConversationList[name];
                    clientModel.ConversationList[name] = party.Union(participants.ToList()).ToList();
                }
                else
                {
                    clientModel.ConversationList.Add(name, participants.ToList());
                    if (Output != null)
                    {
                        Output("CreateConv", name);
                        Output("Message", name, mssg["text"]);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the contact provided by the server.
        /// </summary>
        /// <param name="mssg">The contact to remove.</param>
        private void HandleRemContactMessage(Dictionary<string, string> mssg)
        {
            Contact participant = clientModel.ContactList.Find(x => x.Username.Equals(mssg["source"]));

            if (participant != null)
            {
                clientModel.ContactList.Remove(participant);
                if (Output != null) Output("RemoveCont", mssg["source"]);
            }
        }

        /// <summary>
        /// Updates the status of a user in the conacts.
        /// </summary>
        /// <param name="mssg">The status of the other user.</param>
        private void HandleStatusChangeMessage(Dictionary<string, string> mssg)
        {
            Contact cont = clientModel.ContactList.Find(x => x.Username.Equals((string)mssg["source"]));

            if (cont != null)
            {
                cont.Status = mssg["state"];

                if (Output != null) Output("UpdateContList");

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

        #endregion

        #region Access and Exit

        /// <summary>
        /// Displays and interprets info from a login popup.
        /// </summary>
        /// <returns>Whether or not to exit the loop.</returns>
        private void LoginAction()
        {
            string result;

            switch (loginForm.ShowDialog())
            {
                case DialogResult.OK:
                    result = ValidateUsername(loginForm.Username);

                    if (result.Equals("Success"))
                    {
                        result = ValidatePassword(loginForm.Password);

                        if (result.Equals("Success"))
                        {
                            wsa.Send(String.Format("<login username=\"{0}\" password=\"{1}\" />", loginForm.Username, loginForm.Password));
                            WaitForResponse();
                        }
                        else
                        {
                            ChatClientForm.ShowError(result);
                        }
                    }
                    else
                    {
                        ChatClientForm.ShowError(result);
                    }
                    break;
                case DialogResult.Cancel:
                    loginForm.ClearTextFields();
                    clientModel.State = FlowState.Entry;
                    break;
            }
        }

        /// <summary>
        /// Logs out of the current user session.
        /// </summary>
        private void LogoutAction()
        {
            string contList = String.Join(",", clientModel.ContactList.Select(x => x.Username));
            bool exit = false;

            while (!exit)
            {
                wsc.Send(String.Format("<logout username=\"{0}\" cont=\"{1}\" />", clientModel.Username, contList));
                WaitForResponse();
                if (clientModel.State == FlowState.Exit)
                {
                    exit = true;
                }
            }

            wsa.Close();
            wsc.Close();
        }

        /// <summary>
        /// Displays and interprets info from a signup popup.
        /// </summary>
        /// <returns>Whether or not to exit the loop.</returns>
        private void SignupAction()
        {
            string result;

            switch (signupForm.ShowDialog())
            {
                case DialogResult.OK:
                    result = ValidateUsername(signupForm.Username);

                    if (result.Equals("Success"))
                    {
                        result = ValidatePasswords(signupForm.Password1, signupForm.Password2);

                        if (result.Equals("Success"))
                        {
                            wsa.Send(String.Format("<sign username=\"{0}\" password=\"{1}\" />", signupForm.Username, signupForm.Password1));
                            WaitForResponse();
                        }
                        else
                        {
                            ChatClientForm.ShowError(result);
                        }
                    }
                    else
                    {
                        ChatClientForm.ShowError(result);
                    }
                    break;
                case DialogResult.Cancel:
                    signupForm.ClearTextFields();
                    clientModel.State = FlowState.Entry;
                    break;
            }
        }

        #endregion

        #region Validation

        /// <summary>
        /// Check that the string has only valid characters.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>Whether or not the string passes the inspection.</returns>
        private bool CheckString(string str)
        {
            return Regex.IsMatch(str, "^[a-zA-Z0-9_!@#$%^&.<>]*$");
        }

        /// <summary>
        /// Check that the string contains at least one capital.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>Whether or not the string passes the inspection.</returns>
        private bool CheckCapital(string str)
        {
            return Regex.IsMatch(str, "[A-Z]");
        }

        /// <summary>
        /// Check that the string contains at least one numeric.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>Whether or not the string passes the inspection.</returns>
        private bool CheckNumber(string str)
        {
            return Regex.IsMatch(str, "[0-9]");
        }

        /// <summary>
        /// Check that the string contains at least one special character.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>Whether or not the string passes the inspection.</returns>
        private bool CheckChar(string str)
        {
            return Regex.IsMatch(str, "[_!@#$%^&.<>]");
        }

        /// <summary>
        /// Validates that the username is in an acceptable form.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>Either a success message or an error message.</returns>
        private string ValidateUsername(string username)
        {
            if (!username.Equals(String.Empty))
            {
                if (CheckString(username))
                {
                    return "Success";
                }
                else
                {
                    return "The username must contain uppercase, lowercase, or numeric characters or any of the following: _ ! @ # $ % ^ & . < >";
                }
            }
            else
            {
                return "The username cannot be emtpy.";
            }
        }
        
        /// <summary>
        /// Validates that the password is in an acceptable form.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <returns>Either a success message or an error message.</returns>
        private string ValidatePassword(string password)
        {
            if (password.Length > 8)
            {
                if (CheckString(password))
                {
                    if (CheckCapital(password))
                    {
                        if (CheckNumber(password))
                        {
                            if (CheckChar(password))
                            {
                                return "Success";
                            }
                            else
                            {
                                return "The password must contain at least one of the following: _ ! @ # $ % ^ & . < >";
                            }
                        }
                        else
                        {
                            return "The password must contain at least one numeric character.";
                        }
                    }
                    else
                    {
                        return "The password must contain at least one capital letter.";
                    }
                }
                else
                {
                    return "The password must contain uppercase, lowercase, or numeric characters or any of the following: _ ! @ # $ % ^ & . < >";
                }
            }
            else
            {
                return "The password must be 9 or more characters in length.";
            }
        }

        /// <summary>
        /// Validates that the passwords match, and they are in an acceptable form.
        /// </summary>
        /// <param name="pw1">The first password to check.</param>
        /// <param name="pw1">The second password to check.</param>
        /// <returns>Either a success message or an error message.</returns>
        private string ValidatePasswords(string pw1, string pw2)
        {
            if (pw1.Equals(pw2))
            {
                if (pw1.Length > 8)
                {
                    if (CheckString(pw1))
                    {
                        if (CheckCapital(pw1))
                        {
                            if (CheckNumber(pw1))
                            {
                                if (CheckChar(pw1))
                                {
                                    return "Success";
                                }
                                else
                                {
                                    return "The password must contain at least one of the following: _ ! @ # $ % ^ & . < >";
                                }
                            }
                            else
                            {
                                return "The password must contain at least one numeric character.";
                            }
                        }
                        else
                        {
                            return "The password must contain at least one capital letter.";
                        }
                    }
                    else
                    {
                        return "The password must contain uppercase, lowercase, or numeric characters or any of the following: _ ! @ # $ % ^ & . < >";
                    }
                }
                else
                {
                    return "The password must be 9 or more characters in length.";
                }
            }
            else
            {
                return "The passwords don't match.";
            }
        }

        #endregion

        #region Misc.

        /// <summary>
        /// Wait for a response from the server.
        /// </summary>
        private void WaitForResponse()
        {
            clientModel.WaitFlag = true;
            waitForm.Show();
            waitForm.Refresh();
            while (clientModel.WaitFlag)
            {
                System.Threading.Thread.Sleep(500);
            }
            waitForm.Hide();
        }

        #endregion

        /// <summary>
        /// Deconstructor for the ChatClientController.
        /// </summary>
        ~ChatClientController()
        {
            if (clientModel.State == FlowState.Main) LogoutAction();
        }
    }
}