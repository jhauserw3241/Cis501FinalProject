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
        public LoginPopUp()
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

        public void ShowError(string text)
        {
            MessageBox.Show(text, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
