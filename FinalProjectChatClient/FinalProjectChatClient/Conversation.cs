using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    class Conversation
    {
        public string Name { get; private set; }
        public List<Contact> Participants { get; private set; }

        public Conversation(string name, params Contact[] party)
        {
            Name = name;
            Participants = party.ToList();
        }
    }
}
