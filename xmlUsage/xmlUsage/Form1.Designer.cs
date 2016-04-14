namespace xmlUsage
{
    partial class Form1
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
            this.ux_menuStrip = new System.Windows.Forms.MenuStrip();
            this.ux_fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ux_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ux_tabPanel = new System.Windows.Forms.TabControl();
            this.ux_tabPage0 = new System.Windows.Forms.TabPage();
            this.ux_tabPage1 = new System.Windows.Forms.TabPage();
            this.ux_label0 = new System.Windows.Forms.Label();
            this.ux_label1 = new System.Windows.Forms.Label();
            this.ux_listBox = new System.Windows.Forms.ListBox();
            this.ux_menuStrip.SuspendLayout();
            this.ux_tabPanel.SuspendLayout();
            this.ux_tabPage0.SuspendLayout();
            this.ux_tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ux_menuStrip
            // 
            this.ux_menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ux_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ux_fileMenuItem});
            this.ux_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.ux_menuStrip.Name = "ux_menuStrip";
            this.ux_menuStrip.Size = new System.Drawing.Size(485, 33);
            this.ux_menuStrip.TabIndex = 1;
            this.ux_menuStrip.Text = "menuStrip1";
            // 
            // ux_fileMenuItem
            // 
            this.ux_fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ux_openFileMenuItem});
            this.ux_fileMenuItem.Name = "ux_fileMenuItem";
            this.ux_fileMenuItem.Size = new System.Drawing.Size(50, 29);
            this.ux_fileMenuItem.Text = "File";
            // 
            // ux_openFileMenuItem
            // 
            this.ux_openFileMenuItem.Name = "ux_openFileMenuItem";
            this.ux_openFileMenuItem.Size = new System.Drawing.Size(211, 30);
            this.ux_openFileMenuItem.Text = "Open...";
            this.ux_openFileMenuItem.Click += new System.EventHandler(this.ux_openFileMenuItem_Click);
            // 
            // ux_openFileDialog
            // 
            this.ux_openFileDialog.FileName = "Input.txt";
            this.ux_openFileDialog.InitialDirectory = "Environment.CurrentDirectory";
            // 
            // ux_tabPanel
            // 
            this.ux_tabPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_tabPanel.Controls.Add(this.ux_tabPage0);
            this.ux_tabPanel.Controls.Add(this.ux_tabPage1);
            this.ux_tabPanel.Location = new System.Drawing.Point(138, 36);
            this.ux_tabPanel.Name = "ux_tabPanel";
            this.ux_tabPanel.SelectedIndex = 0;
            this.ux_tabPanel.Size = new System.Drawing.Size(335, 316);
            this.ux_tabPanel.TabIndex = 2;
            // 
            // ux_tabPage0
            // 
            this.ux_tabPage0.Controls.Add(this.ux_label0);
            this.ux_tabPage0.Location = new System.Drawing.Point(4, 29);
            this.ux_tabPage0.Name = "ux_tabPage0";
            this.ux_tabPage0.Padding = new System.Windows.Forms.Padding(3);
            this.ux_tabPage0.Size = new System.Drawing.Size(327, 283);
            this.ux_tabPage0.TabIndex = 0;
            this.ux_tabPage0.Text = "Page 0";
            this.ux_tabPage0.UseVisualStyleBackColor = true;
            // 
            // ux_tabPage1
            // 
            this.ux_tabPage1.Controls.Add(this.ux_label1);
            this.ux_tabPage1.Location = new System.Drawing.Point(4, 29);
            this.ux_tabPage1.Name = "ux_tabPage1";
            this.ux_tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.ux_tabPage1.Size = new System.Drawing.Size(327, 283);
            this.ux_tabPage1.TabIndex = 1;
            this.ux_tabPage1.Text = "Page 1";
            this.ux_tabPage1.UseVisualStyleBackColor = true;
            // 
            // ux_label0
            // 
            this.ux_label0.AutoSize = true;
            this.ux_label0.Location = new System.Drawing.Point(6, 3);
            this.ux_label0.Name = "ux_label0";
            this.ux_label0.Size = new System.Drawing.Size(93, 20);
            this.ux_label0.TabIndex = 0;
            this.ux_label0.Text = "Page 0 Text";
            // 
            // ux_label1
            // 
            this.ux_label1.AutoSize = true;
            this.ux_label1.Location = new System.Drawing.Point(6, 3);
            this.ux_label1.Name = "ux_label1";
            this.ux_label1.Size = new System.Drawing.Size(93, 20);
            this.ux_label1.TabIndex = 0;
            this.ux_label1.Text = "Page 1 Text";
            // 
            // ux_listBox
            // 
            this.ux_listBox.FormattingEnabled = true;
            this.ux_listBox.ItemHeight = 20;
            this.ux_listBox.Location = new System.Drawing.Point(12, 44);
            this.ux_listBox.Name = "ux_listBox";
            this.ux_listBox.Size = new System.Drawing.Size(120, 304);
            this.ux_listBox.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 364);
            this.Controls.Add(this.ux_listBox);
            this.Controls.Add(this.ux_tabPanel);
            this.Controls.Add(this.ux_menuStrip);
            this.MainMenuStrip = this.ux_menuStrip;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ux_menuStrip.ResumeLayout(false);
            this.ux_menuStrip.PerformLayout();
            this.ux_tabPanel.ResumeLayout(false);
            this.ux_tabPage0.ResumeLayout(false);
            this.ux_tabPage0.PerformLayout();
            this.ux_tabPage1.ResumeLayout(false);
            this.ux_tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip ux_menuStrip;
        private System.Windows.Forms.ToolStripMenuItem ux_fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ux_openFileMenuItem;
        private System.Windows.Forms.OpenFileDialog ux_openFileDialog;
        private System.Windows.Forms.TabControl ux_tabPanel;
        private System.Windows.Forms.TabPage ux_tabPage0;
        private System.Windows.Forms.Label ux_label0;
        private System.Windows.Forms.TabPage ux_tabPage1;
        private System.Windows.Forms.Label ux_label1;
        private System.Windows.Forms.ListBox ux_listBox;
    }
}

