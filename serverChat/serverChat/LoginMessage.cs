using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    class LoginMessage
    {
        private ServerUser user = new ServerUser();
        private Message msg = new Message();

        // Constructor
        //
        // @param u The user that is logging in
        public LoginMessage(ServerUser u)
        {
            user = u;
        }

        // Get Message
        //
        // Get the output message that is from the new or previously
        // existing user object
        // @return a string that contains the login message
        public string GetMessage()
        {
            // Convert the contact list to a single string
            string contUNames = string.Join(",", user.GetContactListUsernames());
            string contDNames = string.Join(",", user.GetContactListNames());
            string contStatuses = string.Join(",", user.GetContactListStatuses());

            // Create message
            msg.AddElement("action", "login");
            msg.AddElement("username", user.GetUsername());
            msg.AddElement("dispName", user.GetName());
            msg.AddElement("password", user.GetPassword());
            msg.AddElement("contUsername", contUNames);
            msg.AddElement("contDisplName", contDNames);
            msg.AddElement("contState", contStatuses);
            msg.AddElement("state", "Online");
            return msg.Serialize();
        }
    }
}
