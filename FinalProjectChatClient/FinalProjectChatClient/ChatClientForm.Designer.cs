using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace FinalProjectChatClient
{
    partial class ChatClientForm
    {
        private ListBox contactsList;
        private TabControl conversationTabController;
        private List<Tuple<TabPage, Label>> conversationTabs;
        private TextBox messageBox;
        private MenuStrip mainMenu;
        private ToolStripMenuItem profileMenu;
        private ToolStripMenuItem displayNameProfileOption;
        private ToolStripTextBox changeDispNameTextBox;
        private ToolStripMenuItem statusProfileOption;
        private ToolStripMenuItem offlineStatusOption;
        private ToolStripMenuItem awayStatusOption;
        private ToolStripMenuItem onlineStatusOption;
        private ToolStripMenuItem logoutProfileOption;
        private ToolStripMenuItem contactsMenu;
        private ToolStripMenuItem addContactOption;
        private ToolStripTextBox addContactTextBox;
        private ToolStripMenuItem removeContactOption;
        private ToolStripTextBox removeContactTextBox;
        private ToolStripMenuItem conversationMenu;
        private ToolStripMenuItem createConversationOption;
        private ToolStripMenuItem leaveConversationOption;
        private ToolStripMenuItem addParticipantOption;
        private ToolStripTextBox addParticipantTextBox;
        private StatusStrip infoStrip;
        private ToolStripStatusLabel dispNameLabel;
        private ToolStripStatusLabel userStatusLabel;

        public ListBox ContactsList
        {
            get { return contactsList; }
        }
        public TabControl ConversationTabController
        {
            get { return conversationTabController; }
        }
        public List<Tuple<TabPage, Label>> ConversationTabs
        {
            get { return conversationTabs; }
        }
        public TextBox MessageBox
        {
            get { return messageBox; }
        }
        public MenuStrip MainMenu
        {
            get { return mainMenu; }
        }
        public ToolStripMenuItem ProfileMenu
        {
            get { return profileMenu; }
        }
        public ToolStripMenuItem DisplayNameProfileOption
        {
            get { return displayNameProfileOption; }
        }
        public ToolStripTextBox ChangeDispNameTextBox
        {
            get { return changeDispNameTextBox; }
        }
        public ToolStripMenuItem StatusProfileOption
        {
            get { return statusProfileOption; }
        }
        public ToolStripMenuItem OfflineStatusOption
        {
            get { return offlineStatusOption; }
        }
        public ToolStripMenuItem AwayStatusOption
        {
            get { return awayStatusOption; }
        }
        public ToolStripMenuItem OnlineStatusOption
        {
            get { return onlineStatusOption; }
        }
        public ToolStripMenuItem LogoutProfileOption
        {
            get { return logoutProfileOption; }
        }
        private ToolStripMenuItem ContactsMenu
        {
            get { return contactsMenu; }
        }
        private ToolStripMenuItem AddContactOption
        {
            get { return addContactOption; }
        }
        private ToolStripTextBox AddContactTextBox
        {
            get { return addContactTextBox; }
        }
        private ToolStripMenuItem RemoveContactOption
        {
            get { return removeContactOption; }
        }
        private ToolStripTextBox RemoveContactTextBox
        {
            get { return removeContactTextBox; }
        }
        private ToolStripMenuItem ConversationMenu
        {
            get { return conversationMenu; }
        }
        private ToolStripMenuItem CreateConversationOption
        {
            get { return createConversationOption; }
        }
        private ToolStripMenuItem LeaveConversationOption
        {
            get { return leaveConversationOption; }
        }
        private ToolStripMenuItem AddParticipantOption
        {
            get { return addParticipantOption; }
        }
        private ToolStripTextBox AddParticipantTextBox
        {
            get { return addParticipantTextBox; }
        }
        private StatusStrip InfoStrip
        {
            get { return infoStrip; }
        }
        private ToolStripStatusLabel DispNameLabel
        {
            get { return dispNameLabel; }
        }
        private ToolStripStatusLabel UserStatusLabel
        {
            get { return userStatusLabel; }
        }
        
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.infoStrip = new System.Windows.Forms.StatusStrip();
            this.dispNameLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.userStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.profileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.displayNameProfileOption = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDispNameTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.statusProfileOption = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineStatusOption = new System.Windows.Forms.ToolStripMenuItem();
            this.awayStatusOption = new System.Windows.Forms.ToolStripMenuItem();
            this.offlineStatusOption = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutProfileOption = new System.Windows.Forms.ToolStripMenuItem();
            this.contactsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.addContactOption = new System.Windows.Forms.ToolStripMenuItem();
            this.addContactTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.removeContactOption = new System.Windows.Forms.ToolStripMenuItem();
            this.removeContactTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.conversationMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.createConversationOption = new System.Windows.Forms.ToolStripMenuItem();
            this.addParticipantOption = new System.Windows.Forms.ToolStripMenuItem();
            this.addParticipantTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.leaveConversationOption = new System.Windows.Forms.ToolStripMenuItem();
            this.contactsList = new System.Windows.Forms.ListBox();
            this.conversationTabController = new System.Windows.Forms.TabControl();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.infoStrip.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoStrip
            // 
            this.infoStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.infoStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dispNameLabel,
            this.userStatusLabel});
            this.infoStrip.Location = new System.Drawing.Point(0, 571);
            this.infoStrip.Name = "infoStrip";
            this.infoStrip.Size = new System.Drawing.Size(818, 30);
            this.infoStrip.TabIndex = 0;
            this.infoStrip.Text = "statusStrip1";
            // 
            // dispNameLabel
            // 
            this.dispNameLabel.Name = "dispNameLabel";
            this.dispNameLabel.Size = new System.Drawing.Size(68, 25);
            this.dispNameLabel.Text = "Name: ";
            // 
            // userStatusLabel
            // 
            this.userStatusLabel.Name = "userStatusLabel";
            this.userStatusLabel.Size = new System.Drawing.Size(122, 25);
            this.userStatusLabel.Text = "Status: Offline";
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.profileMenu,
            this.contactsMenu,
            this.conversationMenu});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(818, 33);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // profileMenu
            // 
            this.profileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayNameProfileOption,
            this.statusProfileOption,
            this.logoutProfileOption});
            this.profileMenu.Name = "profileMenu";
            this.profileMenu.Size = new System.Drawing.Size(74, 29);
            this.profileMenu.Text = "Profile";
            // 
            // displayNameProfileOption
            // 
            this.displayNameProfileOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeDispNameTextBox});
            this.displayNameProfileOption.Name = "displayNameProfileOption";
            this.displayNameProfileOption.Size = new System.Drawing.Size(211, 30);
            this.displayNameProfileOption.Text = "Display Name";
            // 
            // changeDispNameTextBox
            // 
            this.changeDispNameTextBox.Name = "changeDispNameTextBox";
            this.changeDispNameTextBox.Size = new System.Drawing.Size(100, 31);
            this.changeDispNameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.changeDispNameTextBox_KeyDown);
            // 
            // statusProfileOption
            // 
            this.statusProfileOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlineStatusOption,
            this.awayStatusOption,
            this.offlineStatusOption});
            this.statusProfileOption.Name = "statusProfileOption";
            this.statusProfileOption.Size = new System.Drawing.Size(211, 30);
            this.statusProfileOption.Text = "Status";
            // 
            // onlineStatusOption
            // 
            this.onlineStatusOption.Name = "onlineStatusOption";
            this.onlineStatusOption.Size = new System.Drawing.Size(211, 30);
            this.onlineStatusOption.Text = "Online";
            this.onlineStatusOption.Click += new System.EventHandler(this.onlineStatusOption_Click);
            // 
            // awayStatusOption
            // 
            this.awayStatusOption.Name = "awayStatusOption";
            this.awayStatusOption.Size = new System.Drawing.Size(211, 30);
            this.awayStatusOption.Text = "Away";
            this.awayStatusOption.Click += new System.EventHandler(this.awayStatusOption_Click);
            // 
            // offlineStatusOption
            // 
            this.offlineStatusOption.Name = "offlineStatusOption";
            this.offlineStatusOption.Size = new System.Drawing.Size(211, 30);
            this.offlineStatusOption.Text = "Offline";
            this.offlineStatusOption.Click += new System.EventHandler(this.offlineStatusOption_Click);
            // 
            // logoutProfileOption
            // 
            this.logoutProfileOption.Name = "logoutProfileOption";
            this.logoutProfileOption.Size = new System.Drawing.Size(211, 30);
            this.logoutProfileOption.Text = "Logout";
            this.logoutProfileOption.Click += new System.EventHandler(this.logoutProfileOption_Click);
            // 
            // contactsMenu
            // 
            this.contactsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addContactOption,
            this.removeContactOption});
            this.contactsMenu.Name = "contactsMenu";
            this.contactsMenu.Size = new System.Drawing.Size(93, 29);
            this.contactsMenu.Text = "Contacts";
            // 
            // addContactOption
            // 
            this.addContactOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addContactTextBox});
            this.addContactOption.Name = "addContactOption";
            this.addContactOption.Size = new System.Drawing.Size(173, 30);
            this.addContactOption.Text = "Add...";
            // 
            // addContactTextBox
            // 
            this.addContactTextBox.Name = "addContactTextBox";
            this.addContactTextBox.Size = new System.Drawing.Size(100, 31);
            this.addContactTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.addContactTextBox_KeyDown);
            // 
            // removeContactOption
            // 
            this.removeContactOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeContactTextBox});
            this.removeContactOption.Name = "removeContactOption";
            this.removeContactOption.Size = new System.Drawing.Size(173, 30);
            this.removeContactOption.Text = "Remove...";
            // 
            // removeContactTextBox
            // 
            this.removeContactTextBox.Name = "removeContactTextBox";
            this.removeContactTextBox.Size = new System.Drawing.Size(100, 31);
            this.removeContactTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.removeContactTextBox_KeyDown);
            // 
            // conversationMenu
            // 
            this.conversationMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createConversationOption,
            this.addParticipantOption,
            this.leaveConversationOption});
            this.conversationMenu.Name = "conversationMenu";
            this.conversationMenu.Size = new System.Drawing.Size(128, 29);
            this.conversationMenu.Text = "Conversation";
            // 
            // createConversationOption
            // 
            this.createConversationOption.Name = "createConversationOption";
            this.createConversationOption.Size = new System.Drawing.Size(159, 30);
            this.createConversationOption.Text = "Create...";
            this.createConversationOption.Click += new System.EventHandler(this.createConversationOption_Click);
            // 
            // addParticipantOption
            // 
            this.addParticipantOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addParticipantTextBox});
            this.addParticipantOption.Name = "addParticipantOption";
            this.addParticipantOption.Size = new System.Drawing.Size(159, 30);
            this.addParticipantOption.Text = "Add";
            // 
            // addParticipantTextBox
            // 
            this.addParticipantTextBox.Name = "addParticipantTextBox";
            this.addParticipantTextBox.Size = new System.Drawing.Size(100, 31);
            this.addParticipantTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.addParticipantTextBox_KeyDown);
            // 
            // leaveConversationOption
            // 
            this.leaveConversationOption.Enabled = false;
            this.leaveConversationOption.Name = "leaveConversationOption";
            this.leaveConversationOption.Size = new System.Drawing.Size(159, 30);
            this.leaveConversationOption.Text = "Leave...";
            this.leaveConversationOption.Click += new System.EventHandler(this.leaveConversationOption_Click);
            // 
            // contactsList
            // 
            this.contactsList.FormattingEnabled = true;
            this.contactsList.ItemHeight = 20;
            this.contactsList.Location = new System.Drawing.Point(12, 40);
            this.contactsList.Name = "contactsList";
            this.contactsList.Size = new System.Drawing.Size(188, 524);
            this.contactsList.TabIndex = 2;
            this.contactsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.contactsList_MouseDoubleClick);
            // 
            // conversationTabController
            // 
            this.conversationTabController.Location = new System.Drawing.Point(206, 36);
            this.conversationTabController.Name = "conversationTabController";
            this.conversationTabController.SelectedIndex = 0;
            this.conversationTabController.Size = new System.Drawing.Size(600, 418);
            this.conversationTabController.TabIndex = 3;
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(206, 460);
            this.messageBox.Multiline = true;
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(600, 104);
            this.messageBox.TabIndex = 4;
            this.messageBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messageBox_KeyDown);
            // 
            // ChatClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 601);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.conversationTabController);
            this.Controls.Add(this.contactsList);
            this.Controls.Add(this.infoStrip);
            this.Controls.Add(this.mainMenu);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "ChatClientForm";
            this.Text = "NetworkChat Instant Messaging Client (α)";
            this.infoStrip.ResumeLayout(false);
            this.infoStrip.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}

