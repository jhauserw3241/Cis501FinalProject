using System;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FinalProjectChatClient
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ChatClientModel clientModel = new ChatClientModel();
            ChatClientController clientController = new ChatClientController(clientModel);
            ChatClientForm clientForm = new ChatClientForm(clientModel);
            clientForm.Load += clientController.HandleLoadIn;
            clientController.ClientForm = clientForm;
            clientController.EntryForm = new EntryPopUp();
            clientController.LoginForm = new LoginPopUp();
            clientController.SignupForm = new SignupPopUp();

            Application.Run(clientForm);
        }
    }
}
