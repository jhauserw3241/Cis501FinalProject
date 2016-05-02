using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientConversation
{
    public class ClientConversation
    {
        private string convoName;
        private List<string> participants = new List<string>();

        public ClientConversation()
        {
            convoName = "";
            participants = null;
        }

        public ClientConversation(string cN ,List<string> p)
        {
            convoName = cN;
            participants.AddRange(p);
        }

        public string ConvoName
        {
            get
            {
                return convoName;
            }
            set
            {
                convoName = value;
            }
        }

        public List<string> Participants
        {
            get
            {
                return participants;
            }
            set
            {
                participants = value;
            }
        }

    }
}
