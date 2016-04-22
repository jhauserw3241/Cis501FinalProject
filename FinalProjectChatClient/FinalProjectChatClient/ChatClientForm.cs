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
        private ChatClientModel clientModel;
        private int tabCount = 5;
        
        public event ClientInputHandler Input;

        public ChatClientForm(ChatClientModel model)
        {
            InitializeComponent();
            clientModel = model;
            conversationTabs = new List<Tuple<TabPage, Label>>();
            contactsList.Items.Add("admin");
        }
        
        /// <summary>
        /// Creates a new conversation with the contact selected.
        /// </summary>
        private void contactsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CreateConversationTab(contactsList.Text);
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
        /// Updates the appropriate portions.
        /// </summary>
        /// <param name="action">What of the form to update.</param>
        public void HandleFormOutput(FormOutput action)
        {
            switch (action)
            {

            }
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
                        ((Label)conversationTabController.SelectedTab.Controls[0]).Text += messageBox.Text + Environment.NewLine;
                    }
                    messageBox.Text = String.Empty;
                    e.SuppressKeyPress = true;
                }
            }
        }

        /// <summary>
        /// Displays the provided error message.
        /// </summary>
        /// <param name="text">Detail of what error occured.</param>
        public void ShowError(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "Signup Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
