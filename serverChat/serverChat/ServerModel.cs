using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using System.Xml.Serialization;

namespace serverChat
{
    public class ServerModel
    {
        // Instance of the singleton
        private static ServerModel inst;
        // Declare the server lists
        private List<ServerConversation> conversationList = new List<ServerConversation>();;
        private List<ServerUser> userList = new List<ServerUser>();
        
        // One-time instantiation constructor
        public static ServerModel Instance
        {
            get {
                if (inst == null)
                {
                    inst = new ServerModel();
                }

                return inst;
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

        // Set User List
        //
        // Set the list of all users that have been created
        // @arg list A list of users created since the user started
        public void SetUserList(List<ServerUser> list)
        {
            userList = list;
        }

        #region Save/Load Data

        public void SerializeUserList(string path)
        {
            XmlSerializer dataSerializer = new XmlSerializer(typeof(List<ServerUser>));

            using (Stream stream = File.Open(path, FileMode.Create))
            {
                dataSerializer.Serialize(stream, GetUserList());
                stream.Close();
            }
        }

        public void DeserializeUserList(string path)
        {
            XmlSerializer dataSerializer = new XmlSerializer(typeof(List<ServerUser>));

            using (Stream stream = File.Open(path, FileMode.Open))
            {
                SetUserList((List<ServerUser>)dataSerializer.Deserialize(stream));
                stream.Close();
            }
        }

        #endregion
    }
}
