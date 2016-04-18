using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serverChat
{
    public partial class ServerView : Form
    {
        // Declare objects
        private ServerUser user = new ServerUser();

        public ServerView()
        {
            InitializeComponent();
        }
        // Users Combobox Selected Index Changed
        //
        // Handle the when the user chooses a value from the user drop down list
        // that isn't the base value
        private void usersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Disable the conversation drop down box
            convComboBox.AllowDrop = false;
            convComboBox.Enabled = false;

            // TODO: Get the user object from the list of all users
        }
    }
}
