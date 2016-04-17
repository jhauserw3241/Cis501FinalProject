using System.Windows.Forms;
using System.Drawing;

namespace FinalProjectChatClient
{
    partial class ChatClientForm
    {
        private StatusStrip infoStrip;
        private ToolStripStatusLabel connectionStatus;
        private MenuStrip mainMenu;
        private ToolStripMenuItem fileMenu;
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
            get { return fileMenu; }
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
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
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
            this.fileMenu});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(605, 33);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(50, 29);
            this.fileMenu.Text = "File";
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
    }
}

