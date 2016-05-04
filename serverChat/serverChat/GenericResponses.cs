using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    class GenericResponses
    {
        // Get the Add Contact Message
        //
        // Get the output message that is for adding a new contact to the user's profile
        // @param contact The user object for the new contact
        // @return a string that creates a new contact on the client side
        public string GetAddContMessage(ServerUser contact)
        {
            Message output = new Message();
            output.AddElement("action", "addCont");
            output.AddElement("username", contact.GetUsername());
            output.AddElement("dispName", contact.GetName());
            output.AddElement("state", contact.GetStatus().ToString());
            return output.Serialize();
        }

        // Get Conversation Message
        //
        // Get the output message that is for the new information for the conversation
        // @param convObj The object for the conversation
        // @return a string that creates a new conversation on the client side
        public string GetConvMessage(ServerConversation convObj)
        {
            // Convert the participants list to a single string
            string parUNames = string.Join(",", convObj.GetParticipantListUsernames());

            Message output = new Message();
            output.AddElement("action", "udConv");
            output.AddElement("conv", convObj.GetConversationName());
            output.AddElement("par", parUNames);
            return output.Serialize();
        }

        // Get Serialized Add Contact Information
        //
        // Get the serialized string of the contact in the format of an added contact
        // @param user The user to get the information from
        // @return the XML string for the contact information
        public string GetSerAddContInfo(ServerUser user)
        {
            Message output = new Message();
            output.AddElement("action", "addCont");
            output.AddElement("username", user.GetUsername());
            output.AddElement("dispName", user.GetName());
            output.AddElement("state", user.GetStatus().ToString());
            return output.Serialize();
        }

        // Get User Obj Message
        //
        // Get the output message that is from the new or previously
        // existing user object
        // @param userObj The object for the user
        // @return a string that creates a new user on the client side
        public string GetUserObjMessage(ServerUser user)
        {
            // Convert the contact list to a single string
            string contUNames = string.Join(",", user.GetContactListUsernames());
            string contDNames = string.Join(",", user.GetContactListNames());

            Message output = new Message();
            output.AddElement("action", "login");
            output.AddElement("username", user.GetUsername());
            output.AddElement("dispName", user.GetName());
            output.AddElement("password", user.GetPassword());
            output.AddElement("contUsername", contUNames);
            output.AddElement("contDisplName", contDNames);
            output.AddElement("state", "Online");
            return output.Serialize();
        }
    }
}
