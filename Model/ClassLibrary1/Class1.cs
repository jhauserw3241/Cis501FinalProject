using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class ClientConversation
    {
        private string convoName;
        private List<string> participants = new List<string>();

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
