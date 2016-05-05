using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serverChat
{
    public partial class ServerView : Form
    {
        // Declare objects
        private ServerModel data = ServerModel.Instance;
        private ServerUser user = new ServerUser();
        private ServerConversation conv = new ServerConversation();
        private List<ServerUser> viewUserList = new List<ServerUser>();
        private List<ServerConversation> viewConvList = new List<ServerConversation>();
        private bool userFlag = false;
        private bool convFlag = false;

        #region Class Modification
        // Constructor
        //
        // @param d The model object
        public ServerView()
        {
            // Initialize the form
            InitializeComponent();
        }
        #endregion

        #region Helper Methods
        // Add Conversation to Conversation ComboBox
        //
        // Add a conversation to the list of conversations in the
        // Conversations ComboBox
        // @arg name The name of the conversation to be added
        private void AddConvOption(string name)
        {
            eleListBox.Items.Add(name);
        }

        // Display Conversation Information
        //
        // Display the information for the conversation
        private void DisplayConvInfo()
        {
            currentInfo.AppendText(string.Format("Name: {0}", conv.GetConversationName()));
            currentInfo.AppendText(string.Format("Participants: {0}", conv.GetParticipantListUsernames().ToString()));
        }

        // Display User Information
        //
        // Display the information for the user
        private void DisplayUserInfo()
        {
            currentInfo.AppendText(string.Format("Username: {0}", user.GetUsername()));
            currentInfo.AppendText(string.Format("Display Name: {0}", user.GetName()));
            currentInfo.AppendText(string.Format("Password: {0}", user.GetPassword()));
            currentInfo.AppendText(string.Format("IP Address: {0}", user.GetIpAddress()));
            currentInfo.AppendText(string.Format("Status: {0}", user.GetStatus().ToString()));
            currentInfo.AppendText(string.Format("Contacts: {0}", user.GetContactListUsernames().ToString()));
        }

        // Populate Conversation List Box
        //
        // Populate the conversation list box with the list of conversations in the view
        private void PopulateConvListBox()
        {
            int size = viewConvList.Count;
            for (int i = 0; i < size; i++)
            {
                string name = viewConvList.ElementAt(i).GetConversationName();
                eleListBox.Items.Add(name);
            }
        }

        // Populate User List Box
        //
        // Populate the user list box with the list of users in the view
        private void PopulateUserListBox()
        {
            int size = viewUserList.Count;
            for (int i = 0; i < size; i++)
            {
                string username = viewUserList.ElementAt(i).GetUsername();
                eleListBox.Items.Add(username);
            }
        }

        // Remove All List Box Elements
        //
        // Remove all of the elements from the eleListBox on the form
        private void RemoveAllListBoxEles()
        {
            int size = eleListBox.Items.Count;
            for (int i = 0; i < size; i++)
            {
                eleListBox.Items.RemoveAt(i);
            }
        }
        #endregion

        #region Handle Input
        // Display Selected Information
        //
        // Display the information for the item selected by the user
        // @param index The index of the selected element in its list
        private void DisplaySelectedInformation(int index)
        {
            // TODO: Add check to make sure that there are elements in the listbox to select
            if ((userFlag == true) || (convFlag == false))
            {
                // Update the user object with the selected user element
                user = viewUserList.ElementAt(index);

                // Display the information of the selected user
                DisplayUserInfo();
            }
            else if ((userFlag == false) || (convFlag == true))
            {
                // Update the conversation object with the selected conversation element
                conv = viewConvList.ElementAt(index);

                // Display the information of the selected conversation
                DisplayConvInfo();
            }
            else
            {
                MessageBox.Show("ERROR: Invalid flag set selected");
            }
        }

        // Handle Form Output
        //
        // Handle any actions that need to be taken for the view
        // @param action What form needs to update
        public void HandleFormOutput(string action, params object[] vars)
        {
            switch (action)
            {
                case "Debug":
                    MessageBox.Show((string)vars[0]);
                    break;
                case "UpdateUserList":
                    UpdateUserList();
                    break;
                case "UpdateConversationList":
                    UpdateConvList();
                    break;
                case "ProvideEleName":
                    DisplaySelectedInformation((int)vars[0]);
                    break;
                case "InvalidInputOption":
                    MessageBox.Show("Error: You tried to user a GUI element incorrectly.");
                    break;
                default:
                    MessageBox.Show("ERROR: Invalid action provided to form.");
                    break;
            }
        }

        // Update Conversation List
        //
        // Update the conversation list both inside of the view and on the form
        private void UpdateConvList()
        {
            // Enable the listbox if it isn't already enabled
            if (!eleListBox.Enabled)
            {
                eleListBox.Enabled = true;
            }

            // Update view flags
            userFlag = false;
            convFlag = true;

            // Update conversation list in view
            viewConvList = data.GetConversationList();

            // Update conversation list in the listbox on the form
            RemoveAllListBoxEles();
            PopulateConvListBox();
        }

        // Update User List
        //
        // Update the user list both inside of the view and on the form
        private void UpdateUserList()
        {
            // Enable the listbox if it isn't already enabled
            if (!eleListBox.Enabled)
            {
                eleListBox.Enabled = true;
            }

            // Update view flags
            userFlag = true;
            convFlag = false;

            // Update user list in view
            viewUserList = data.GetUserList();

            // Update user list in the listbox on the form
            RemoveAllListBoxEles();
            PopulateUserListBox();
        }
        #endregion
    }
}
