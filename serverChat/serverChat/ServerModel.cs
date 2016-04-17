using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    public class ServerModel
    {
        // Declare the server lists
        private List<serverConversation> conversationList = new List<serverConversation>();
        private List<serverUser> userList = new List<serverUser>();

        // Get Conversation List
        //
        // Get the list of all conversations that have been created
        // @return the list of server conversations
        List<serverConversation> GetConversationList()
        {
            return conversationList;
        }

        // Get User List
        //
        // Get the list of all users that have been created
        // @return the list of server users
        List<serverUser> GetUserList()
        {
            return userList;
        }

        // Set Conversation List
        //
        // Set the list of all conversations that have been created
        // @arg list A list of conversations created since the server started
        void SetConversationList(List<serverConversation> list)
        {
            conversationList = list;
        }

        // Set User List
        //
        // Set the list of all users that have been created
        // @arg list A list of users created since the user started
        void SetUserList(List<serverUser> list)
        {
            userList = list;
        }
    }
}
