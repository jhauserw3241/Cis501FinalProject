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
        private States status;

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

        public States Status
        {
            get { return status; }
            set { status = value; }
        }

        public ChatClientModel()
        {
            contactList = new List<string>();
            conversationList = new List<string>();
            displayName = "";
            ipAddress = "";
            status = States.Disconnected;
        }
    }
}
