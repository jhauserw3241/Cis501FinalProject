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
        private ServerUser user = new ServerUser();
        private ServerConversation conv = new ServerConversation();
        private List<ServerUser> usersList = new List<ServerUser>();
        private List<ServerConversation> convsList = new List<ServerConversation>();
        private bool userFlag = false;
        private bool convFlag = false;

        #region Class Modification
        // Constructor
        public ServerView()
        {
            // Initialize the form
            InitializeComponent();
        }
        #endregion

        #region GUI Elements Handling
        // Handle Conversation Button Clicked
        //
        // Handle actions taken when the conversation button is clicked
        private void convButton_Click(object sender, EventArgs e)
        {
            // Set flags for category selected
            userFlag = false;
            convFlag = true;

            // Put all of the elements from the conversation list into the list box
            RemoveAllListBoxEles();
            UpdateConvListBox();
        }

        // Handle Item Selected from List Box
        //
        // Handle actions taken when the user selects an item from the list box
        private void eleListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear text box
            currentInfo.Clear();

            // Handle user selected
            if ((userFlag == true) || (convFlag == false))
            {
                // TODO: Get user object for selected user

                // Display username
                currentInfo.AppendText("Username: " + user.GetUsername());

                // Display display name
                currentInfo.AppendText("Display Name: " + user.GetName());

                // Display status
                currentInfo.AppendText("Status: " + user.GetStatus());

                // Display ip address
                currentInfo.AppendText("IP Address: " + user.GetIpAddress());
            }
            // Handle conversation selected
            else if ((userFlag == false) || (convFlag == true))
            {
                // TODO: Get conversation object for selected user

                // Display name
                currentInfo.AppendText("Name: " + conv.GetConversationName());

                // Display list of usernames for participants
                currentInfo.AppendText("List of Participants: " + conv.GetParicipantList().ToString());

                // Display most recent message
                currentInfo.AppendText("Most Recent Message: " + conv.GetCurrentMessage());
            }
            // Handle error case
            else
            {
                MessageBox.Show("ERROR: You need to pick either the 'Users' or 'Conversations' button before you can choose an option.");
            }
        }

        // Handle User Button Clicked
        //
        // Handle actions taken when the user button has been clicked
        private void usersButton_Click(object sender, EventArgs e)
        {
            // Set flags for category selected
            userFlag = true;
            convFlag = false;

            // Put all of the elements from the user list into the list box
            RemoveAllListBoxEles();
            UpdateUserListBox();
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

        // Handle Form Output
        //
        // Handle any actions that need to be taken for the view
        // @param action What form needs to update
        public void HandleFormOutput(string action, params object[] vars)
        {
            int size = vars.Count();

            switch (action)
            {
                case "UpdateUserList":
                    for (int i = 0; i < size; i++)
                    {
                        usersList.Add((ServerUser)vars[i]);
                    }
                    break;
                case "UpdateConversationList":
                    for (int i = 0; i < size; i++)
                    {
                        convsList.Add((ServerConversation)vars[i]);
                    }
                    break;
                default:
                    MessageBox.Show("ERROR: Invalid action provided to form.");
                    break;
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

        // Update User List Box
        //
        // Update the user list box with the list of ServerUser objects
        private void UpdateUserListBox()
        {
            int size = usersList.Count;
            for (int i = 0; i < size; i++)
            {
                string username = usersList.ElementAt(i).GetUsername();
                eleListBox.Items.Add(username);
            }
        }

        // Update Conversation List Box
        //
        // Update the conversation list box with the list of ServerConversation object
        private void UpdateConvListBox()
        {
            int size = convsList.Count;
            for (int i = 0; i < size; i++)
            {
                string name = convsList.ElementAt(i).GetConversationName();
                eleListBox.Items.Add(name);
            }
        }
        #endregion
    }
}
