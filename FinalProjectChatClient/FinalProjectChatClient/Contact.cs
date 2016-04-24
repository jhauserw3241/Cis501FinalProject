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
            Status = "Online";
        }
        public override string ToString()
        {
            return String.Format("{0} : {1} : {2}", DisplayName, Username, (Status == "Online" ? "Online" : "Offline"));
        }
    }
}
