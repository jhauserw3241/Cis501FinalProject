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
        private List<ServerConversation> conversationList = new List<ServerConversation>();
        private Dictionary<string, List<string>> contactRelationshipDict = new Dictionary<string, List<string>>();
        private List<ServerUser> userList = new List<ServerUser>();

        public ServerController ServerController
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        // Get Conversation List
        //
        // Get the list of all conversations that have been created
        // @return the list of server conversations
        public List<ServerConversation> GetConversationList()
        {
            return conversationList;
        }

        // Get Contact Relationship Dictionary
        //
        // Get the list of all contact relationships that have been created
        // @return the list of contact relationships
        public Dictionary<string, List<string>> GetContactRelationshipDict()
        {
            return contactRelationshipDict;
        }

        // Get User List
        //
        // Get the list of all users that have been created
        // @return the list of server users
        public List<ServerUser> GetUserList()
        {
            return userList;
        }

        // Set Conversation List
        //
        // Set the list of all conversations that have been created
        // @arg list A list of conversations created since the server started
        public void SetConversationList(List<ServerConversation> list)
        {
            conversationList = list;
        }

        // Set Contact Relationship List
        //
        // Set the list of all contact relationships that have been created
        // @arg list A list of contact relationships created since the user started
        public void SetContactRelationshipList(Dictionary<string, List<string>> dict)
        {
            contactRelationshipDict = dict;
        }

        // Set User List
        //
        // Set the list of all users that have been created
        // @arg list A list of users created since the user started
        public void SetUserList(List<ServerUser> list)
        {
            userList = list;
        }
    }
}
