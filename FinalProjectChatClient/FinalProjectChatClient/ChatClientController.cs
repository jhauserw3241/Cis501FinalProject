using System;
using WebSocketSharp;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            entryForm = new EntryPopUp();
            entryForm.Input += HandleEntry;
            loginForm = new LoginPopUp();
            loginForm.Input += HandleLogin;
            signupForm = new SignupPopUp();
            signupForm.Input += HandleSignup;
            ws = new WebSocket("ws://127.0.0.1:8001/chat");

            Application.Run(entryForm);
            switch (clientModel.Status)
            {
                case States.SigningUp:
                    Application.Run(signupForm);
                    break;
                case States.LoggingIn:
                    Application.Run(loginForm);
                    break;
            }
            Application.Run(clientForm);
        }

        static void HandleEntry(EntryOption action)
        {
            switch (action)
            {
                case EntryOption.Login:
                    clientModel.Status = States.LoggingIn;
                    break;
                case EntryOption.Signup:
                    clientModel.Status = States.SigningUp;
                    break;
            }

            entryForm.Dispose();
        }

        static void HandleLogin(DialogOption action)
        {
            loginForm.Dispose();
        }

        static void HandleSignup(DialogOption action)
        {
            signupForm.Dispose();
        }
    }
}
