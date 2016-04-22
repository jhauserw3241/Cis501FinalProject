using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public class ChatClientModel
    {
        private List<string> contactList;
        private List<string> conversationList;
        private string displayName;
        private string ipAddress;
        private FlowState state;
        private DispState status;
        private bool waitingMsg;

        public List<string> ContactList
        {
            get { return contactList; }
            set { contactList = value; }
        }
        public List<string> ConversationList
        {
            get { return conversationList; }
            set { conversationList = value; }
        }
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        public FlowState State
        {
            get { return state; }
            set { state = value; }
        }
        public DispState Status
        {
            get { return status; }
            set { status = value; }
        }
        public bool WaitingMsg
        {
            get { return waitingMsg; }
            set { waitingMsg = value; }
        }

        public ChatClientModel()
        {
            contactList = new List<string>();
            conversationList = new List<string>();
            displayName = "";
            ipAddress = "";
            state = FlowState.Disconnected;
            status = DispState.Offline;
            waitingMsg = false;
        }
    }
}
