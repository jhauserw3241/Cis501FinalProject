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
    public partial class ConvCreatePopUp : Form
    {
        public ListBox.ObjectCollection ContactListBox
        {
            get { return contactListBox.Items; }
        }
        public string ConvName
        {
            get { return convNameTextBox.Text; }
        }
        public ListBox.ObjectCollection ParticipantListBox
        {
            get { return participantListBox.Items; }
        }

        public event ClientInputHandler CreationInput;

        /// <summary>
        /// Creates a new instance of a conversation creation pop up.
        /// </summary>
        public ConvCreatePopUp()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Adds a contact to the list of participants.
        /// </summary>
        private void addPartButton_Click(object sender, EventArgs e)
        {
            if (CreationInput != null && contactListBox.SelectedItem != null)
                CreationInput("Add", contactListBox.SelectedItem);
        }
        /// <summary>
        /// Cancel creating this conversation.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        /// <summary>
        /// Resets the form every time it is shown.
        /// </summary>
        private void ConvCreatePopUp_Shown(object sender, EventArgs e)
        {
            convNameTextBox.Text = "";
            participantListBox.Items.Clear();
            if (CreationInput != null) CreationInput("Reset");
        }
        /// <summary>
        /// Accept the current state of the form.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (Name.Equals(String.Empty))
                ChatClientForm.ShowError("Conversation name cannot be emtpy.");
            else DialogResult = DialogResult.OK;
        }
        /// <summary>
        /// Removes a contact form the list of participants.
        /// </summary>
        private void removePartButton_Click(object sender, EventArgs e)
        {
            if (CreationInput != null && participantListBox.SelectedItem != null)
                CreationInput("Remove", participantListBox.SelectedItem);
        }
    }
}
