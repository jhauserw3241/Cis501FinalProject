namespace FinalProjectChatClient
{
    partial class ConvCreatePopUp
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
            this.convNameLabel = new System.Windows.Forms.Label();
            this.convNameTextBox = new System.Windows.Forms.TextBox();
            this.contactListBox = new System.Windows.Forms.ListBox();
            this.participantListBox = new System.Windows.Forms.ListBox();
            this.addPartButton = new System.Windows.Forms.Button();
            this.removePartButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // convNameLabel
            // 
            this.convNameLabel.AutoSize = true;
            this.convNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.convNameLabel.Location = new System.Drawing.Point(12, 23);
            this.convNameLabel.Name = "convNameLabel";
            this.convNameLabel.Size = new System.Drawing.Size(231, 29);
            this.convNameLabel.TabIndex = 0;
            this.convNameLabel.Text = "Conversation Name:";
            // 
            // convNameTextBox
            // 
            this.convNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.convNameTextBox.Location = new System.Drawing.Point(249, 20);
            this.convNameTextBox.Name = "convNameTextBox";
            this.convNameTextBox.Size = new System.Drawing.Size(307, 35);
            this.convNameTextBox.TabIndex = 1;
            // 
            // contactListBox
            // 
            this.contactListBox.ColumnWidth = 20;
            this.contactListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactListBox.FormattingEnabled = true;
            this.contactListBox.ItemHeight = 26;
            this.contactListBox.Location = new System.Drawing.Point(17, 74);
            this.contactListBox.Name = "contactListBox";
            this.contactListBox.Size = new System.Drawing.Size(182, 160);
            this.contactListBox.TabIndex = 2;
            // 
            // participantListBox
            // 
            this.participantListBox.ColumnWidth = 20;
            this.participantListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.participantListBox.FormattingEnabled = true;
            this.participantListBox.ItemHeight = 26;
            this.participantListBox.Location = new System.Drawing.Point(363, 74);
            this.participantListBox.Name = "participantListBox";
            this.participantListBox.Size = new System.Drawing.Size(182, 160);
            this.participantListBox.TabIndex = 3;
            // 
            // addPartButton
            // 
            this.addPartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addPartButton.Location = new System.Drawing.Point(215, 97);
            this.addPartButton.Name = "addPartButton";
            this.addPartButton.Size = new System.Drawing.Size(130, 45);
            this.addPartButton.TabIndex = 4;
            this.addPartButton.Text = "Add →";
            this.addPartButton.UseVisualStyleBackColor = true;
            this.addPartButton.Click += new System.EventHandler(this.addPartButton_Click);
            // 
            // removePartButton
            // 
            this.removePartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removePartButton.Location = new System.Drawing.Point(215, 165);
            this.removePartButton.Name = "removePartButton";
            this.removePartButton.Size = new System.Drawing.Size(130, 45);
            this.removePartButton.TabIndex = 5;
            this.removePartButton.Text = "←Remove";
            this.removePartButton.UseVisualStyleBackColor = true;
            this.removePartButton.Click += new System.EventHandler(this.removePartButton_Click);
            // 
            // okButton
            // 
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(49, 250);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(130, 45);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(385, 250);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(130, 45);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ConvCreatePopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(568, 307);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.removePartButton);
            this.Controls.Add(this.addPartButton);
            this.Controls.Add(this.participantListBox);
            this.Controls.Add(this.contactListBox);
            this.Controls.Add(this.convNameTextBox);
            this.Controls.Add(this.convNameLabel);
            this.Name = "ConvCreatePopUp";
            this.Text = "Create Conversation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label convNameLabel;
        private System.Windows.Forms.TextBox convNameTextBox;
        private System.Windows.Forms.ListBox contactListBox;
        private System.Windows.Forms.ListBox participantListBox;
        private System.Windows.Forms.Button addPartButton;
        private System.Windows.Forms.Button removePartButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}