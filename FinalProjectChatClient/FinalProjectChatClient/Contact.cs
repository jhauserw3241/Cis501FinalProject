using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public class Contact
    {
        public string DisplayName { get; private set; }
        public string Status { get; set; }
        public string Username { get; private set; }

        public Contact(string username, string dispName)
        {
            Username = username;
            DisplayName = dispName;
        }
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
