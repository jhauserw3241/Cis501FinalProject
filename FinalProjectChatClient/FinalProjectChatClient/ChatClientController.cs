using System;
using WebSocketSharp;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml;

namespace FinalProjectChatClient
{
    class ChatClientController
    {
        private ChatClientForm clientForm;
        private ChatClientModel clientModel;
        private EntryPopUp entryForm;
        private LoginPopUp loginForm;
        private SignupPopUp signupForm;
        private WebSocket ws;

        public ChatClientForm ClientForm
        {
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
        public event ClientOutputHandler Output;
        public SignupPopUp SignupForm
        {
            set { signupForm = value; }
        }

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
        public void HandleFormInput(FormInput action)
        {
            switch (action)
            {

            }
        }

        /// <summary>
        /// Run entry loop until user manages to login or closes.
        /// </summary>
        public void HandleLoadIn(object sender, EventArgs e)
        {
            bool exit = false;

            while (!exit)
            {
                switch (entryForm.ShowDialog())
                {
                    case DialogResult.Yes:
                        LoginAction();
                        while (clientModel.WaitingMsg) { }
                        break;
                    case DialogResult.No:
                        SignupAction();
                        while (clientModel.WaitingMsg) { }
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

            clientModel.WaitingMsg = false;
            if (!mssg.ContainsKey("action")) return;

            switch (mssg["action"])
            {
                case "sign":
                    HandleSignupMessage(mssg);
                    break;
                case "login":
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
                    break;
                case "addPa":
                    break;
                case "msg":
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles messages from the server pertainging towards signing up.
        /// </summary>
        /// <param name="mssg">The dictionary of keywords and their values.</param>
        private void HandleSignupMessage(Dictionary<string, string> mssg)
        {
            if (mssg.ContainsKey("error"))
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// Displays and interprets info from a login popup.
        /// </summary>
        /// <returns>Whether or not to exit the loop.</returns>
        private void LoginAction()
        {
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                if (!loginForm.Username.Equals(String.Empty))
                {
                    if (!loginForm.Password.Equals(String.Empty))
                    {
                        ws.Send(String.Format("<login username=\"{0}\" password=\"{1}\" />", signupForm.Username, signupForm.Password1));
                        clientModel.WaitingMsg = true;
                    }
                    else
                    {
                        clientForm.ShowError("The password cannot be empty.");
                    }
                }
                else
                {
                    clientForm.ShowError("The username cannot be empty.");
                }
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
            if (signupForm.ShowDialog() == DialogResult.OK)
            {
                if (!signupForm.Username.Equals(String.Empty))
                {

                    if (!signupForm.Password1.Equals(String.Empty))
                    {

                        if (signupForm.Password1.Equals(signupForm.Password2))
                        {
                            ws.Send(String.Format("<sign username=\"{0}\" password=\"{1}\" />", signupForm.Username, signupForm.Password1));
                            clientModel.WaitingMsg = true;
                        }
                        else
                        {
                            clientForm.ShowError("The passwords do not match.");
                        }
                    }
                    else
                    {
                        clientForm.ShowError("The password cannot be empty.");
                    }
                }
                else
                {
                    clientForm.ShowError("The username cannot be empty.");
                }
            }
        }

        /// <summary>
        /// Deconstructor for the ChatClientController.
        /// </summary>
        ~ChatClientController()
        {
            ws.Close();
        }
    }
}