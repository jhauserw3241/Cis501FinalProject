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
    public partial class EntryPopUp : Form
    {
        public event EntryInputHandler Input;

        public EntryPopUp()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (Input != null)
                Input(EntryOption.Login);
        }

        private void signupButton_Click(object sender, EventArgs e)
        {
            if (Input != null)
                Input(EntryOption.Signup);
        }
    }
}
