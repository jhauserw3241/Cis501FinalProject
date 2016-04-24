using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public class ChatClientModel
    {
        #region Fields

        private List<Contact> contactList;
        private Dictionary<string, List<string>> conversationList;
        private string displayName;
        private bool errorFlag;
        private string userName;
        private FlowState state;
        private string status;
        private bool waitFlag;

        #endregion

        #region Properties

        public List<Contact> ContactList
        {
            get { return contactList; }
            set { contactList = value; }
        }
        public Dictionary<string, List<string>> ConversationList
        {
            get { return conversationList; }
            private set { conversationList = value; }
        }
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        public bool ErrorFlag
        {
            get { return errorFlag; }
            set { errorFlag = value; }
        }
        public string Username
        {
            get { return userName; }
            set { userName = value; }
        }
        public FlowState State
        {
            get { return state; }
            set { state = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public bool WaitFlag
        {
            get { return waitFlag; }
            set { waitFlag = value; }
        }

        #endregion

        #region Public Methods

        public ChatClientModel()
        {
            contactList = new List<Contact>() { new Contact("admin", "Admin") };
            conversationList = new Dictionary<string, List<string>>();
            displayName = "";
            userName = "";
            state = FlowState.Entry;
            status = "Offline";
            waitFlag = false;
        }

        #endregion
    }
}
