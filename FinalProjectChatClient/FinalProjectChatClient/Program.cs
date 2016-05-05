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
            ChatClientModel clientModel = ChatClientModel.Instance;

            // Create controller for conversation creation form
            ConvCreateController convCreate = new ConvCreateController(new ConvCreatePopUp());

            // Create central controller
            ChatClientController clientController = new ChatClientController();
            // Create and add forms to controller
            clientController.ClientForm = new ChatClientForm();
            clientController.CreateForm = convCreate.PopUp;
            clientController.EntryForm = new EntryPopUp();
            clientController.LoginForm = new LoginPopUp();
            clientController.SignupForm = new SignupPopUp();

            // Assign controller's generic handler to the appropriate form components
            clientController.ClientForm.AwayStatusOption.Click += clientController.HandleGenericInput;
            clientController.ClientForm.OnlineStatusOption.Click += clientController.HandleGenericInput;
            clientController.ClientForm.CreateConversationOption.Click += clientController.HandleGenericInput;
            clientController.ClientForm.LogoutProfileOption.Click += clientController.HandleGenericInput;

            // Assign controller's key handler to the appropriate form components
            clientController.ClientForm.AddContactTextBox.KeyDown += clientController.HandleKeyInput;
            clientController.ClientForm.RemoveContactTextBox.KeyDown += clientController.HandleKeyInput;
            clientController.ClientForm.AddParticipantTextBox.KeyDown += clientController.HandleKeyInput;
            clientController.ClientForm.MessageBox.KeyDown += clientController.HandleKeyInput;
            clientController.ClientForm.ChangeDispNameTextBox.KeyDown += clientController.HandleKeyInput;

            // Assign Controller's mouse handler to the appropriate form components
            clientController.ClientForm.ContactsList.MouseDoubleClick += clientController.HandleMouseInput;

            // Add output handler from main form to central controller
            clientController.Output += clientController.ClientForm.HandleFormOutput;
            // Add start-up handler from central controller to main form
            clientController.ClientForm.Load += clientController.HandleLoadIn;
            // Add input handler from conversation creation controller to conversation creation form
            convCreate.PopUp.CreationInput += convCreate.HandleFormInput;

            Application.Run(clientController.ClientForm);
        }
    }
}
