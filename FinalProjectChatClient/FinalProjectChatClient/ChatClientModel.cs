/* ChatClientModel.cs
 * Author: Ryan Huber
 * 
 * A class representing the users information and the state of the program.
 */
using System.Collections.Generic;

namespace FinalProjectChatClient
{
    public class ChatClientModel
    {

        #region Properties

        public List<Contact> ContactList { get; set; }
        public Dictionary<string, List<string>> ConversationList { get; set; }
        public string DisplayName { get; set; }
        public bool ErrorFlag { get; set; }
        public string Username { get; set; }
        public FlowState State { get; set; }
        public string Status { get; set; }
        public bool WaitFlag { get; set; }

        #endregion
        
        public ChatClientModel()
        {
            ContactList = new List<Contact>() { new Contact("admin", "Admin", "Online") };
            ConversationList = new Dictionary<string, List<string>>();
            DisplayName = "";
            Username = "";
            State = FlowState.Entry;
            Status = "Offline";
            WaitFlag = false;
            ErrorFlag = false;
        }
    }
}
