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
        #region Fields
        private static ChatClientModel instance;
        #endregion

        #region Properties

        public List<Contact> ContactList { get; set; }
        public Dictionary<string, List<string>> ConversationList { get; set; }
        public string DisplayName { get; set; }
        public bool ErrorFlag { get; set; }
        public static ChatClientModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChatClientModel();
                }

                return instance;
            }
        }
        public string Username { get; set; }
        public FlowState State { get; set; }
        public string Status { get; set; }
        public bool WaitFlag { get; set; }

        #endregion
        
        public ChatClientModel()
        {
            ContactList = new List<Contact>();
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
