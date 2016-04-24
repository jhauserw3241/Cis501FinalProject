using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public class ConvCreateController
    {
        private ConvCreatePopUp csPopUp;
        private ChatClientModel clientModel;

        public ConvCreatePopUp PopUp
        {
            get { return csPopUp; }
            set { csPopUp = value; }
        }

        /// <summary>
        /// Creates a new instance of a conversation creation controller.
        /// </summary>
        /// <param name="model">The model to work from to populate the list of contacts.</param>
        public ConvCreateController(ChatClientModel model, ConvCreatePopUp popup)
        {
            clientModel = model;
            csPopUp = popup;
        }
        /// <summary>
        /// Handles the inner-form actions such as add, remove, and reset.
        /// </summary>
        /// <param name="action">Whether a participant is being added or removed, ot the form is being reset.</param>
        /// <param name="vars">The item from the lists.</param>
        public void HandleFormInput(string action, params object[] vars)
        {
            switch (action)
            {
                case "Add":
                    csPopUp.ParticipantListBox.Add((Contact)vars[0]);
                    csPopUp.ContactListBox.Remove((Contact)vars[0]);
                    break;
                case "Remove":
                    csPopUp.ContactListBox.Add((Contact)vars[0]);
                    csPopUp.ParticipantListBox.Remove((Contact)vars[0]);
                    break;
                case "Reset":
                    csPopUp.ContactListBox.AddRange(clientModel.ContactList.ToArray());
                    break;
            }
        }
    }
}
