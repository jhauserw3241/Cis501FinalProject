using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    public class ServerController
    {
        private ServerModel data = new ServerModel();

        // Add Contact To Model List
        //
        // Add the contact to the list of all of the users in the model
        // @arg user The current user
        public void AddContactToModelList(ServerUser user)
        {
            // Get the list of all contacts from the model
            List<ServerUser> userList = data.GetUserList();

            // Add the current user object
            userList.Add(user);

            // Set the current user list to the model user list
            data.SetUserList(userList);
        }

        // Get Conversation Participants
        //
        // Get the list of participants from the specified conversation
        // @arg convName The name of the conversation
        public List<string> GetConvParticipants(string convName)
        {
            // Get list of all conversation relationships
            Dictionary<string, List<string>> convRelationships = data.GetContactRelationshipDict();

            // Get the list of participants in the current conversation
            List<string> participants = new List<string>();
            convRelationships.TryGetValue(convName, out participants);

            return participants;
        }
    }
}
