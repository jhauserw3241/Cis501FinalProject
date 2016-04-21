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
    public partial class SignupPopUp : Form
    {
        public string Username
        {
            get { return usernameTextBox.Text; }
        }
        public string Password1
        {
            get { return passwordTextBox.Text; }
        }
        public string Password2
        {
            get { return reEnterTextBox.Text; }
        }

        public SignupPopUp()
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
