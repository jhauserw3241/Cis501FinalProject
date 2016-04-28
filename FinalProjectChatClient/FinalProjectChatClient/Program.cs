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

            // Create model shared by major components
            ChatClientModel clientModel = new ChatClientModel();

            // Create controller for conversation creation form
            ConvCreateController convCreate = new ConvCreateController(clientModel, new ConvCreatePopUp());

            // Create central controller
            ChatClientController clientController = new ChatClientController(clientModel);
            // Create and add forms to controller
            clientController.EntryForm = new EntryPopUp();
            clientController.LoginForm = new LoginPopUp();
            clientController.SignupForm = new SignupPopUp();

            // Create the main view
            ChatClientForm clientForm = new ChatClientForm(clientModel, convCreate.PopUp);

            // Add output handler from main form to central controller
            clientController.Output += clientForm.HandleFormOutput;
            // Add start-up handler from central controller to main form
            clientForm.Load += clientController.HandleLoadIn;
            // Add input handler from central controller to main form
            clientForm.MainInput += clientController.HandleFormInput;
            // Add input handler from conversation creation controller to conversation creation form
            convCreate.PopUp.CreationInput += convCreate.HandleFormInput;

            Application.Run(clientForm);
        }
    }
}
