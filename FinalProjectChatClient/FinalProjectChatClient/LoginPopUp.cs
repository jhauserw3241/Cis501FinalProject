using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProjectChatClient
{
    public partial class LoginPopUp : Form
    {
        public string Username
        {
            get { return usernameTextBox.Text; }
        }
        public string Password
        {
            get { return passwordTextBox.Text; }
        }

        public LoginPopUp()
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
        }

        /// <summary>
        /// Clears the text fields.
        /// </summary>
        public void ClearTextFields()
        {
            usernameTextBox.Text = String.Empty;
            passwordTextBox.Text = String.Empty;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
