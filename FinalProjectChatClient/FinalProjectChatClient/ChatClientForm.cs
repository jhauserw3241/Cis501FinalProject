using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public partial class ChatClientForm : Form
    {
        #region Fields

        private ChatClientModel clientModel;
        private ConvCreatePopUp createForm;
        private int tabCount = 5;

        #endregion

        #region Properties

        public ConvCreatePopUp CreateForm
        {
            set { createForm = value; }
        }

        public string Status
        {
            get { return userStatusLabel.Text.Substring(8); }
        }

        #endregion

        #region Events

        public event ClientInputHandler MainInput;

        #endregion
        
        /// <summary>
        /// Creates a new instance of a chat client form.
        /// </summary>
        /// <param name="model">The model from which most client side information is pulled.</param>
        public ChatClientForm(ChatClientModel model, ConvCreatePopUp create)
        {
            InitializeComponent();
            clientModel = model;
            createForm = create;
            conversationTabs = new List<Tuple<TabPage, Label>>();

            contactsList.Items.Add(new Contact("admin", "Admin", "Online"));
        }

        #region FormOutput

        /// <summary>
        /// Updates the appropriate portions.
        /// </summary>
        /// <param name="action">What of the form to update.</param>
        public void HandleFormOutput(string action, params object[] vars)
        {
            switch (action)
            {
                case "AddCont":
                    contactsList.Items.Add((Contact)vars[0]);
                    break;
                case "RemoveCont":
                    contactsList.Items.Remove((Contact)vars[0]);
                    break;
                case "CreateConv":
                    CreateConversationTab((string)vars[0]);
                    break;
                case "LeaveConv":
                    RemoveConversationTab((TabPage)vars[0]);
                    break;
                case "UpdateStatus":
                    userStatusLabel.Text = "Status: " + (string)vars[0];
                    break;
                case "UpdateName":
                    dispNameLabel.Text = "Name: " + (string)vars[0];
                    break;
                case "Message":
                    conversationTabs.Find(x => x.Item1.Name.Equals((string)vars[0])).Item2.Text += String.Join(Environment.NewLine, (List<string>)vars[1]);
                    break;
            }
        }

        /// <summary>
        /// Creates a new tab to contain a conversation.
        /// </summary>
        /// <param name="convName">The name of the conversation and tab.</param>
        private void CreateConversationTab(string convName)
        {
            TabPage newPage = new TabPage();
            Label newLabel = new Label();

            // Configuring new conversation tab
            newPage.Controls.Add(newLabel);
            newPage.Location = new Point(4, 29);
            newPage.Name = convName;
            newPage.Padding = new Padding(3);
            newPage.Size = new Size(447, 271);
            newPage.TabIndex = tabCount;
            newPage.Text = convName;
            newPage.UseVisualStyleBackColor = true;

            // Configuring new text space
            newLabel.Location = new Point(0, 0);
            newLabel.Name = convName + "Text";
            newLabel.Size = new Size(447, 271);
            newLabel.TabIndex = tabCount;
            tabCount++;

            // Adding tab and text box to form
            conversationTabs.Add(new Tuple<TabPage, Label>(newPage, newLabel));
            conversationTabController.Controls.Add(newPage);

            // Enable the leave tab if this is the first conversation
            if (conversationTabController.TabCount == 1)
                leaveConversationOption.Enabled = true;
        }

        /// <summary>
        /// Removes the provided tab page from the conversation tab controller.
        /// </summary>
        /// <param name="tab">The tab to remove.</param>
        private void RemoveConversationTab(TabPage tab)
        {
            tab.Controls.RemoveAt(0);
            conversationTabController.TabPages.Remove(tab);
            conversationTabs.Remove(conversationTabs.Find(x => x.Item1.Equals(tab)));
            if (conversationTabs.Count == 0) leaveConversationOption.Enabled = false;
        }

        /// <summary>
        /// Displays the provided error message.
        /// </summary>
        /// <param name="text">Detail of what error occured.</param>
        public static void ShowError(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion

        #region Contacts

        /// <summary>
        /// Attempts to add the username provided to the list of contacts.
        /// </summary>
        private void addContactTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!addContactTextBox.Text.Equals(String.Empty))
                {
                    if (MainInput != null) MainInput("AddCont", addContactTextBox.Text);
                }
                addContactTextBox.Text = String.Empty;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Attempts to remove the provided username from the contact list.
        /// </summary>
        private void removeContactTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!removeContactTextBox.Text.Equals(String.Empty))
                {
                    if (MainInput != null) MainInput("RemoveCont", removeContactTextBox.Text);
                }
                removeContactTextBox.Text = String.Empty;
                e.SuppressKeyPress = true;
            }
        }

        #endregion

        #region Conversations
        
        /// <summary>
        /// Attempts to add the username provided to the current conversation.
        /// </summary>
        private void addParticipantTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (conversationTabController.Controls.Count > 0 && !addParticipantTextBox.Text.Equals(String.Empty))
                {
                    if (MainInput != null) MainInput("AddPart", conversationTabController.SelectedTab.Text, addParticipantTextBox.Text);
                }
                addParticipantTextBox.Text = String.Empty;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Creates a new conversation with the contact selected.
        /// </summary>
        private void contactsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Contact cont = ((Contact)contactsList.SelectedItem);
            string name = cont.DisplayName;

            if (!clientModel.ConversationList.ContainsKey(name) && !cont.Status.Equals("Offline"))
            {
                if (MainInput != null) MainInput("CreateConv", name, new List<Contact> { (Contact)contactsList.SelectedItem });
            }
            else ShowError("Could not create a conversation:\nEither one has already been started, or they are offline.");
        }

        /// <summary>
        /// Creates a conversation with the possibilty of initializing multiple participants.
        /// </summary>
        private void createConversationOption_Click(object sender, EventArgs e)
        {
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                string name = createForm.Name;

                if (!clientModel.ConversationList.ContainsKey(name))
                {
                    if (MainInput != null) MainInput("CreateConv", name, createForm.ParticipantListBox.Cast<Contact>().ToList());
                }
                else ShowError("This conversation name already exists.");
            }
        }

        /// <summary>
        /// Leaves the conversation of the current tab.
        /// </summary>
        private void leaveConversationOption_Click(object sender, EventArgs e)
        {
            if (MainInput != null) MainInput("LeaveConv", conversationTabController.SelectedTab);
        }

        /// <summary>
        /// Checks to see if the user is attempting to submit a message.
        /// </summary>
        private void messageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Modifiers != Keys.Shift)
                {
                    if (conversationTabController.Controls.Count > 0)
                    {
                        if (MainInput != null) MainInput("Message", conversationTabController.SelectedTab.Text, messageBox.Text);
                    }
                    messageBox.Text = String.Empty;
                    e.SuppressKeyPress = true;
                }
            }
        }

        #endregion

        #region Statusi

        /// <summary>
        /// Change user's status to "invisible".
        /// </summary>
        private void awayStatusOption_Click(object sender, EventArgs e)
        {
            if (MainInput != null) MainInput("ChangeStatus", "Away");
        }

        /// <summary>
        /// Change user's status to "offline".
        /// </summary>
        private void offlineStatusOption_Click(object sender, EventArgs e)
        {
            if (MainInput != null) MainInput("ChangeStatus", "Offline");
        }

        /// <summary>
        /// Change user's status to "online".
        /// </summary>
        private void onlineStatusOption_Click(object sender, EventArgs e)
        {
            if (MainInput != null) MainInput("ChangeStatus", "Online");
        }

        #endregion

        #region Profile

        /// <summary>
        /// Changes the users display name.
        /// </summary>
        private void changeDispNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!changeDispNameTextBox.Text.Equals(String.Empty))
                {
                    if (MainInput != null) MainInput("ChangeDispName", changeDispNameTextBox.Text);
                }
                changeDispNameTextBox.Text = String.Empty;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        private void logoutProfileOption_Click(object sender, EventArgs e)
        {
            if (MainInput != null) MainInput("Logout");
        }

        #endregion
    }
}
