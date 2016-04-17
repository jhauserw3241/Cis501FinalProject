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
        public event AccountInputHandler Input;

        public SignupPopUp()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (Input != null)
                Input(DialogOption.Ok);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (Input != null)
                Input(DialogOption.Cancel);
        }
    }
}
