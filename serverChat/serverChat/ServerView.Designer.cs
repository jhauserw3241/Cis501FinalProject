namespace serverChat
{
    partial class ServerView
    {
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
            this.usersButton = new System.Windows.Forms.Button();
            this.currentInfo = new System.Windows.Forms.RichTextBox();
            this.eleListBox = new System.Windows.Forms.ListBox();
            this.convButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // usersButton
            //
            this.usersButton.Location = new System.Drawing.Point(12, 12);
            this.usersButton.Name = "usersButton";
            this.usersButton.Size = new System.Drawing.Size(141, 23);
            this.usersButton.TabIndex = 4;
            this.usersButton.Text = "Users";
            this.usersButton.UseVisualStyleBackColor = true;
            this.usersButton.Click += new System.EventHandler(this.usersButton_Click);
            //
            // currentInfo
            //
            this.currentInfo.Location = new System.Drawing.Point(159, 9);
            this.currentInfo.Name = "currentInfo";
            this.currentInfo.Size = new System.Drawing.Size(354, 299);
            this.currentInfo.TabIndex = 5;
            this.currentInfo.Text = "";
            //
            // eleListBox
            //
            this.eleListBox.FormattingEnabled = true;
            this.eleListBox.Location = new System.Drawing.Point(12, 70);
            this.eleListBox.Name = "eleListBox";
            this.eleListBox.Size = new System.Drawing.Size(141, 238);
            this.eleListBox.TabIndex = 6;
            this.eleListBox.SelectedIndexChanged += new System.EventHandler(this.eleListBox_SelectedIndexChanged);
            //
            // convButton
            //
            this.convButton.Location = new System.Drawing.Point(12, 41);
            this.convButton.Name = "convButton";
            this.convButton.Size = new System.Drawing.Size(141, 23);
            this.convButton.TabIndex = 7;
            this.convButton.Text = "Conversations";
            this.convButton.UseVisualStyleBackColor = true;
            this.convButton.Click += new System.EventHandler(this.convButton_Click);
            //
            // ServerView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 319);
            this.Controls.Add(this.convButton);
            this.Controls.Add(this.eleListBox);
            this.Controls.Add(this.currentInfo);
            this.Controls.Add(this.usersButton);
            this.Name = "ServerView";
            this.Text = "Server Information";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button usersButton;
        private System.Windows.Forms.RichTextBox currentInfo;
        private System.Windows.Forms.ListBox eleListBox;
        private System.Windows.Forms.Button convButton;
    }
}

