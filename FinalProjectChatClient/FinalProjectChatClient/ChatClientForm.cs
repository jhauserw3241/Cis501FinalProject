﻿using System;
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
        private int tabCount = 5;

        #endregion

        #region Properties

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
        public ChatClientForm()
        {
            InitializeComponent();
            clientModel = ChatClientModel.Instance;
            conversationTabs = new List<Tuple<TabPage, Label>>();
        }

        #region Form Output

        /// <summary>
        /// Updates the appropriate portions.
        /// </summary>
        /// <param name="action">What of the form to update.</param>
        public void HandleFormOutput(string action, string param1 = "", string param2 = "")
        {
            Contact cont;
            switch (action)
            {
                case "AddCont":
                    cont = clientModel.ContactList.Last();
                    Invoke((MethodInvoker)(() => contactsList.Items.Add(cont)));
                    break;
                case "RemoveCont":
                    cont = clientModel.ContactList.Find(x => x.Username.Equals(param1));
                    Invoke((MethodInvoker)(() => contactsList.Items.Remove(cont)));
                    RemoveContactTextBox.Text = String.Empty;
                    break;
                case "ClrAddCont":
                    addContactTextBox.Text = String.Empty;
                    break;
                case "ClrRemCont":
                    removeContactTextBox.Text = String.Empty;
                    break;
                case "CreateConv":
                    CreateConversationTab(param1);
                    break;
                case "RenameConv":
                    TabPage conv = conversationTabs.Find(x => x.Item1.Name.Equals(param1)).Item1;
                    conv.Name = param2;
                    conv.Text = param2;
                    break;
                case "AddPart":
                    AddParticipantTextBox.Text = String.Empty;
                    break;
                case "LeaveConv":
                    RemoveConversationTab(conversationTabController.SelectedTab);
                    break;
                case "Message":
                    Invoke((MethodInvoker)(() => conversationTabs.Find(x => x.Item1.Name.Equals(param1)).Item2.Text += param2));
                    break;
                case "ClrMsg":
                    messageBox.Text = String.Empty;
                    break;
                case "UpdateStatus":
                    userStatusLabel.Text = "Status: " + clientModel.Status;
                    break;
                case "UpdateName":
                    dispNameLabel.Text = "Name: " + clientModel.DisplayName;
                    changeDispNameTextBox.Text = String.Empty;
                    break;
                case "UpdateContList":
                    Invoke((MethodInvoker)(() => contactsList.Items.Clear()));
                    foreach (Contact cnt in clientModel.ContactList)
                    {
                        Invoke((MethodInvoker)(() => contactsList.Items.Add(cnt)));
                    }
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
            Invoke((MethodInvoker)(() => conversationTabController.Controls.Add(newPage)));

            // Enable the leave tab if this is the first conversation
            if (conversationTabController.TabCount == 1)
                Invoke((MethodInvoker)(() => leaveConversationOption.Enabled = true));
        }

        /// <summary>
        /// Removes the provided tab page from the conversation tab controller.
        /// </summary>
        /// <param name="tab">The tab to remove.</param>
        private void RemoveConversationTab(TabPage tab)
        {
            Invoke((MethodInvoker)(() => conversationTabController.TabPages.Remove(tab)));
            Invoke((MethodInvoker)(() => conversationTabs.Remove(conversationTabs.Find(x => x.Item1.Equals(tab)))));
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
    }
}
