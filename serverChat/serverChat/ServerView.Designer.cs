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
            this.usersLabel = new System.Windows.Forms.Label();
            this.usersComboBox = new System.Windows.Forms.ComboBox();
            this.convLabel = new System.Windows.Forms.Label();
            this.convComboBox = new System.Windows.Forms.ComboBox();
            this.enterButton = new System.Windows.Forms.Button();
            this.currentInfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            //
            // usersLabel
            //
            this.usersLabel.AutoSize = true;
            this.usersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usersLabel.Location = new System.Drawing.Point(9, 9);
            this.usersLabel.Name = "usersLabel";
            this.usersLabel.Size = new System.Drawing.Size(43, 13);
            this.usersLabel.TabIndex = 0;
            this.usersLabel.Text = "Users:";
            //
            // usersComboBox
            //
            this.usersComboBox.FormattingEnabled = true;
            this.usersComboBox.Location = new System.Drawing.Point(12, 25);
            this.usersComboBox.Name = "usersComboBox";
            this.usersComboBox.Size = new System.Drawing.Size(141, 21);
            this.usersComboBox.TabIndex = 1;
            // convLabel
            //
            this.convLabel.AutoSize = true;
            this.convLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.convLabel.Location = new System.Drawing.Point(9, 63);
            this.convLabel.Name = "convLabel";
            this.convLabel.Size = new System.Drawing.Size(91, 13);
            this.convLabel.TabIndex = 2;
            this.convLabel.Text = "Conversations:";
            //
            // convComboBox
            //
            this.convComboBox.FormattingEnabled = true;
            this.convComboBox.Location = new System.Drawing.Point(12, 79);
            this.convComboBox.Name = "convComboBox";
            this.convComboBox.Size = new System.Drawing.Size(141, 21);
            this.convComboBox.TabIndex = 3;
            //
            // enterButton
            //
            this.enterButton.Location = new System.Drawing.Point(41, 116);
            this.enterButton.Name = "enterButton";
            this.enterButton.Size = new System.Drawing.Size(75, 23);
            this.enterButton.TabIndex = 4;
            this.enterButton.Text = "Enter";
            this.enterButton.UseVisualStyleBackColor = true;
            //
            // currentInfo
            //
            this.currentInfo.Location = new System.Drawing.Point(159, 9);
            this.currentInfo.Name = "currentInfo";
            this.currentInfo.Size = new System.Drawing.Size(354, 298);
            this.currentInfo.TabIndex = 5;
            this.currentInfo.Text = "";
            //
            // ServerView
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 319);
            this.Controls.Add(this.currentInfo);
            this.Controls.Add(this.enterButton);
            this.Controls.Add(this.convComboBox);
            this.Controls.Add(this.convLabel);
            this.Controls.Add(this.contactsComboBox);
            this.Controls.Add(this.contactsLabel);
            this.Name = "ServerView";
            this.Text = "Server Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label usersLabel;
        private System.Windows.Forms.ComboBox usersComboBox;
        private System.Windows.Forms.Label convLabel;
        private System.Windows.Forms.ComboBox convComboBox;
        private System.Windows.Forms.Button enterButton;
        private System.Windows.Forms.RichTextBox currentInfo;
    }
}

