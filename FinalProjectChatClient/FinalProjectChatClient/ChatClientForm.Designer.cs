using System.Windows.Forms;
using System.Drawing;

namespace FinalProjectChatClient
{
    partial class ChatClientForm
    {
        private StatusStrip infoStrip;
        private ToolStripStatusLabel connectionStatus;
        private MenuStrip mainMenu;
        private ToolStripMenuItem profileMenu;
        private ListBox contactsList;
        private TabControl conversationTabController;
        private TextBox messageBox;
        
        public StatusStrip InfoStrip
        {
            get { return infoStrip; }
        }
        public ToolStripStatusLabel ConnectionStatus
        {
            get { return connectionStatus; }
        }
        public MenuStrip MainMenu
        {
            get { return mainMenu; }
        }
        public ToolStripMenuItem FileMenu
        {
            get { return profileMenu; }
        }
        public ListBox ContactsList
        {
            get { return contactsList; }
        }
        public TabControl ConversationTabController
        {
            get { return conversationTabController; }
        }
        public TextBox MessageBox
        {
            get { return messageBox; }
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
            this.connectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.profileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.contactsList = new System.Windows.Forms.ListBox();
            this.conversationTabController = new System.Windows.Forms.TabControl();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.contactsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.addContactOption = new System.Windows.Forms.ToolStripMenuItem();
            this.removeContactOption = new System.Windows.Forms.ToolStripMenuItem();
            this.displayNameProfileOption = new System.Windows.Forms.ToolStripMenuItem();
            this.conversationMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.createConversationOption = new System.Windows.Forms.ToolStripMenuItem();
            this.leaveConversationOption = new System.Windows.Forms.ToolStripMenuItem();
            this.profileStatusMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineStatusOption = new System.Windows.Forms.ToolStripMenuItem();
            this.offlineStatusOption = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutProfileOption = new System.Windows.Forms.ToolStripMenuItem();
            this.infoStrip.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // infoStrip
            // 
            this.infoStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.infoStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionStatus});
            this.infoStrip.Location = new System.Drawing.Point(0, 449);
            this.infoStrip.Name = "infoStrip";
            this.infoStrip.Size = new System.Drawing.Size(605, 30);
            this.infoStrip.TabIndex = 0;
            this.infoStrip.Text = "statusStrip1";
            // 
            // connectionStatus
            // 
            this.connectionStatus.Name = "connectionStatus";
            this.connectionStatus.Size = new System.Drawing.Size(271, 25);
            this.connectionStatus.Text = "Connection Status: Disconnected";
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
            this.mainMenu.Size = new System.Drawing.Size(605, 33);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // profileMenu
            // 
            this.profileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayNameProfileOption,
            this.profileStatusMenu,
            this.logoutProfileOption});
            this.profileMenu.Name = "profileMenu";
            this.profileMenu.Size = new System.Drawing.Size(74, 29);
            this.profileMenu.Text = "Profile";
            // 
            // contactsList
            // 
            this.contactsList.FormattingEnabled = true;
            this.contactsList.ItemHeight = 20;
            this.contactsList.Location = new System.Drawing.Point(12, 40);
            this.contactsList.Name = "contactsList";
            this.contactsList.Size = new System.Drawing.Size(120, 404);
            this.contactsList.TabIndex = 2;
            this.contactsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.contactsList_MouseDoubleClick);
            // 
            // conversationTabController
            // 
            this.conversationTabController.Location = new System.Drawing.Point(138, 36);
            this.conversationTabController.Name = "conversationTabController";
            this.conversationTabController.SelectedIndex = 0;
            this.conversationTabController.Size = new System.Drawing.Size(455, 304);
            this.conversationTabController.TabIndex = 3;
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(138, 342);
            this.messageBox.Multiline = true;
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(455, 104);
            this.messageBox.TabIndex = 4;
            this.messageBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.messageBox_KeyDown);
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
            this.addContactOption.Name = "addContactOption";
            this.addContactOption.Size = new System.Drawing.Size(211, 30);
            this.addContactOption.Text = "Add...";
            // 
            // removeContactOption
            // 
            this.removeContactOption.Name = "removeContactOption";
            this.removeContactOption.Size = new System.Drawing.Size(211, 30);
            this.removeContactOption.Text = "Remove...";
            // 
            // displayNameProfileOption
            // 
            this.displayNameProfileOption.Name = "displayNameProfileOption";
            this.displayNameProfileOption.Size = new System.Drawing.Size(211, 30);
            this.displayNameProfileOption.Text = "Display Name";
            // 
            // conversationMenu
            // 
            this.conversationMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createConversationOption,
            this.leaveConversationOption});
            this.conversationMenu.Name = "conversationMenu";
            this.conversationMenu.Size = new System.Drawing.Size(128, 29);
            this.conversationMenu.Text = "Conversation";
            // 
            // createConversationOption
            // 
            this.createConversationOption.Name = "createConversationOption";
            this.createConversationOption.Size = new System.Drawing.Size(211, 30);
            this.createConversationOption.Text = "Create...";
            // 
            // leaveConversationOption
            // 
            this.leaveConversationOption.Name = "leaveConversationOption";
            this.leaveConversationOption.Size = new System.Drawing.Size(211, 30);
            this.leaveConversationOption.Text = "Leave...";
            // 
            // profileStatusMenu
            // 
            this.profileStatusMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlineStatusOption,
            this.offlineStatusOption});
            this.profileStatusMenu.Name = "profileStatusMenu";
            this.profileStatusMenu.Size = new System.Drawing.Size(211, 30);
            this.profileStatusMenu.Text = "Status";
            // 
            // onlineStatusOption
            // 
            this.onlineStatusOption.Name = "onlineStatusOption";
            this.onlineStatusOption.Size = new System.Drawing.Size(211, 30);
            this.onlineStatusOption.Text = "Online";
            // 
            // offlineStatusOption
            // 
            this.offlineStatusOption.Name = "offlineStatusOption";
            this.offlineStatusOption.Size = new System.Drawing.Size(211, 30);
            this.offlineStatusOption.Text = "Offline";
            // 
            // logoutProfileOption
            // 
            this.logoutProfileOption.Name = "logoutProfileOption";
            this.logoutProfileOption.Size = new System.Drawing.Size(211, 30);
            this.logoutProfileOption.Text = "Logout";
            // 
            // ChatClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 479);
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

        private ToolStripMenuItem displayNameProfileOption;
        private ToolStripMenuItem profileStatusMenu;
        private ToolStripMenuItem onlineStatusOption;
        private ToolStripMenuItem offlineStatusOption;
        private ToolStripMenuItem logoutProfileOption;
        private ToolStripMenuItem contactsMenu;
        private ToolStripMenuItem addContactOption;
        private ToolStripMenuItem removeContactOption;
        private ToolStripMenuItem conversationMenu;
        private ToolStripMenuItem createConversationOption;
        private ToolStripMenuItem leaveConversationOption;
    }
}

