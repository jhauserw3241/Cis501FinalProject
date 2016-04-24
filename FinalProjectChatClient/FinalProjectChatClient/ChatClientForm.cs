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

        #endregion

        #region Events

        public event ClientInputHandler MainInput;

        #endregion

        #region Public Methods

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
            contactsList.Items.Add(new Contact("admin", "Admin"));
        }

        /// <summary>
        /// Updates the appropriate portions.
        /// </summary>
        /// <param name="action">What of the form to update.</param>
        public void HandleFormOutput(string action, params object[] vars)
        {
            switch (action)
            {
                case "Message":
                    ((Label)conversationTabController.TabPages[(int)vars[0]].Controls[0]).Text += (string)vars[1];
                    break;
            }
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

        #region Private Methods

        /// <summary>
        /// Creates a new conversation with the contact selected.
        /// </summary>
        private void contactsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Conversation conv = new Conversation(contactsList.Text, (Contact)contactsList.SelectedItem);
            CreateConversationTab(contactsList.Text);
            if (MainInput != null) MainInput("CreateConv", conv);
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
                        if (MainInput != null) MainInput("Message", conversationTabController.SelectedIndex, messageBox.Text);
                    }
                    messageBox.Text = String.Empty;
                    e.SuppressKeyPress = true;
                }
            }
        }

        /// <summary>
        /// Creates a conversation with the possibilty of initializing multiple participants.
        /// </summary>
        private void createConversationOption_Click(object sender, EventArgs e)
        {
            if (createForm.ShowDialog() == DialogResult.OK)
            {
                    CreateConversationTab(createForm.Name);
                    if (MainInput != null) MainInput("CreateConv", new Conversation(createForm.Name, createForm.ParticipantListBox.Cast<Contact>().ToArray()));
            }
            createForm.Reset();
        }

        #endregion
    }
}
