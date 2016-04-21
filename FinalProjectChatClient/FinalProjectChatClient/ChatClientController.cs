using System;
using WebSocketSharp;
using System.Windows.Forms;
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
        private ClientOutputHandler output;
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
                        exit = LoginAction();
                        break;
                    case DialogResult.No:
                        exit = SignupAction();
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
        public void HandleMessage(object sender, EventArgs e)
        {
            switch (clientModel.Status)
            {
                case States.SigningUp:
                    // Read e.Data as an xml string
                    break;
                case States.LoggingIn:
                    break;
                case States.Connected:
                    break;
                case States.LoggingOut:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Displays and interprets info from a login popup.
        /// </summary>
        /// <returns>Whether or not to exit the loop.</returns>
        private bool LoginAction()
        {
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                if (!loginForm.Username.Equals(String.Empty))
                {
                    if (!loginForm.Password.Equals(String.Empty))
                    {
                        ws.Send(String.Format("<sign username=\"{0}\" password=\"{1}\" />", signupForm.Username, signupForm.Password1));

                        return true;
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

            return false;
        }

        /// <summary>
        /// Displays and interprets info from a signup popup.
        /// </summary>
        /// <returns>Whether or not to exit the loop.</returns>
        private bool SignupAction()
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

                            return true;
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

            return false;
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