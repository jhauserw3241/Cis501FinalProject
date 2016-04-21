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
        private static ChatClientForm clientForm;
        private static ChatClientModel clientModel;
        private static EntryPopUp entryForm;
        private static LoginPopUp loginForm;
        private static ClientOutputHandler output;
        private static SignupPopUp signupForm;
        private static WebSocket ws;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            clientModel = new ChatClientModel();
            clientForm = new ChatClientForm();
            clientForm.Load += HandleLoadIn;
            entryForm = new EntryPopUp();
            loginForm = new LoginPopUp();
            signupForm = new SignupPopUp();
            ws = new WebSocket("ws://127.0.0.1:8001/chat");
            ws.OnMessage += HandleMessage;
            ws.Connect();
            
            Application.Run(clientForm);
        }

        static void HandleMessage(object sender, EventArgs e)
        {
            switch (clientModel.Status)
            {
                case States.SigningUp:
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

        static void HandleLoadIn(object sender, EventArgs e)
        {
            bool exit = false;

            while (!exit)
            {
                switch (entryForm.ShowDialog())
                {
                    case DialogResult.Yes:
                        switch (loginForm.ShowDialog())
                        {
                            case DialogResult.OK:
                                exit = true;
                                break;
                            case DialogResult.Cancel:
                                break;
                        }
                        break;
                    case DialogResult.No:
                        switch (signupForm.ShowDialog())
                        {
                            case DialogResult.OK:
                                exit = true;
                                break;
                            case DialogResult.Cancel:
                                break;
                        }
                        break;
                    case DialogResult.Cancel:
                        Application.Exit();
                        break;
                }
            }
        }
    }
}