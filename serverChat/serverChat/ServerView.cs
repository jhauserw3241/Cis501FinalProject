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
        private ServerModel data;
        private ServerUser user = new ServerUser();
        private ServerConversation conv = new ServerConversation();

        // Constructor
        //
        // @arg d The model object instance
        public ServerView(ServerModel d)
        {
            // Update the server objects
            data = d;

            // Initialize the form
            InitializeComponent();
        }

        // Add User to User ComboBox
        //
        // Add a username to the list of usernames in the User ComboBox
        // @arg username The username of the user that was added
        private void addUserOption(string username)
        {
            usersComboBox.Items.Add(username);
        }

        // Remove User from User ComboBox
        //
        // Remove a username from the list of usernames in the User ComboBox
        // @arg username The username of the user that has been removed
        private void removeUserOption(string username)
        {
            usersComboBox.Items.Remove(username);
        }

        // Add Conversation to Conversation ComboBox
        //
        // Add a conversation to the list of conversations in the
        // Conversations ComboBox
        // @arg name The name of the conversation to be added
        private void addConvOption(string name)
        {
            convComboBox.Items.Add(name);
        }

        // Remove Conversation from Conversation ComboBox
        //
        // Remove a conversation from the list of conversations in
        // the Conversations ComboBox
        // @arg name The name of the conversation to be removed
        private void removeConvOption(string name)
        {
            convComboBox.Items.Remove(name);
        }

        // Users Combobox Selected Index Changed
        //
        // Handle the when the user chooses a value from the user drop down list
        // that isn't the base value
        private void usersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Disable the conversation drop down box
            convComboBox.AllowDrop = false;
            convComboBox.Enabled = false;

            // TODO: Get the user object from the list of all users
        }

        // Conversation Combobox Selected Index Changed
        //
        // Handle the when the user chooses a value from the conversation drop down
        // list that isn't the base value
        private void convComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Disable the conversation drop down box
            usersComboBox.AllowDrop = false;
            usersComboBox.Enabled = false;

            // TODO: Get the conversation object from the list of all users
        }

        // Enter Button Clicked
        //
        // Handle the when the enter button is clicked
        private void enterButton_Click(object sender, EventArgs e)
        {
            // Create any text currently in the textbox
            currentInfo.Clear();

            // Determine if the user object is present
            if (user != new ServerUser())
            {
                currentInfo.AppendText("User display name: ra");
                currentInfo.AppendText("Username: random");
                currentInfo.AppendText("Password: pass");
                return;
            }

            // Determine if the conversation object is present
            if (conv != new ServerConversation())
            {
                currentInfo.AppendText("Conversation name: current name");
                return;
            }

            // Send error to user
            MessageBox.Show("You need to select either a user or a conversation!");
        }
    }
}
