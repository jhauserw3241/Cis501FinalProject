using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace serverChat
{
    class Message
    {
        private string from = "";
        private List<string> to = new List<string>();
        private string serMsg = "";
        private Dictionary<string, string> msgEles = new Dictionary<string, string>();

        #region Class Manipulation
        // Constructor
        public Message()
        {
        }

        // Constructor
        //
        // @param xmlMsg A message that is already serialized
        public Message(string xmlMsg)
        {
            serMsg = xmlMsg;
        }

        // Constructor
        //
        // @param msgParts The parts of a message that needs to be serialized
        public Message(Dictionary<string, string> parts)
        {
            msgEles = parts;
        }
        #endregion

        #region Getters
        // Get XML Message Parts
        //
        // Get the parts for the deserailized XML message
        public Dictionary<string, string> GetMsgParts()
        {
            return msgEles;
        }

        // Get Recipients
        //
        // Get the list of IDs for the recipients for the message
        public List<string> GetRecipientIds()
        {
            return to;
        }

        // Get XML String
        //
        // Get the string containing the serialized version of the message
        // @return a string containing the serialized message
        public string GetXmlString()
        {
            return serMsg;
        }
        #endregion

        #region Handle Serialization
        // Deserialize XML
        //
        // Deserialize the string defined by the constructor
        // @return the dictionary containing the keywords and their values
        public Dictionary<string, string> Deserialize()
        {
            // Convert the message string to a XML object
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(serMsg);

            // Initialize the dictionary
            Dictionary<string, string> output = new Dictionary<string, string>();

            // Add the action element to the dictionary
            output.Add("action", xml.DocumentElement.Name);

            // Go through all the nodes in the provided XML object
            foreach (XmlElement node in xml)
            {
                // Go through all the attributes for the provided node
                XmlAttributeCollection attList = node.Attributes;
                foreach (XmlAttribute at in attList)
                {
                    // Add the current attribute and its value to the dictionary
                    output.Add(at.LocalName, at.Value);
                }
            }

            msgEles = output;
            return output;
        }

        // Serialize XML
        //
        // Serialize the parts of the XML message
        // @return a string containing the message with the unparsed XML string
        public string Serialize()
        {
            string output = "<";

            // Add the correct action to the XML string
            if (msgEles.ContainsKey("action"))
            {
                output += (msgEles["action"] + " ");
            }
            else
            {
                output += "error ";
            }

            // Add each of the attributes to the XML string
            foreach (KeyValuePair<string, string> curEle in msgEles) {
                if (curEle.Key != "action")
                {
                    output += string.Format("{0}=\"{1}\" ", curEle.Key, curEle.Value);
                }
            }

            output += "/>";

            serMsg = output;
            return output;
        }
        #endregion

        #region Modify Elements
        // Add Message Element
        //
        // Add message element to the message part list
        // @param key A string containing the key to be added
        // @param val A string containing the value to be added
        public void AddElement(string key, string val)
        {
            msgEles.Add(key, val);
        }

        // Add Recipient
        //
        // Add the specified recipient to the list of recipients
        // @param id The ID of the recipient
        public void AddRecipient(string id)
        {
            to.Add(id);
        }

        // Contains Key
        //
        // Whether or not the message elements list contains the specified key
        // @param key A string containing the key that may or may not be in the list
        // @return whether or not the specified key could be found
        public bool ContainsKey(string key)
        {
            return msgEles.ContainsKey(key);
        }

        // Get Value
        //
        // Get the value for the element with the specified key
        // @param key A string containing the key in the list
        // @return the string value; will be empty if the key doesn't exist
        public string GetValue(string key)
        {
            if (!ContainsKey(key))
            {
                return "";
            }

            return msgEles[key];
        }


        // Remove Message Element
        //
        // Remove message element based on the key
        // @param key A string containing the key to be added
        public void RemoveElement(string key)
        {
            msgEles.Remove(key);
        }
        #endregion

        #region Setters
        // Set Message Parts
        //
        // Set the parts of the message
        // @param parts The parts of the message
        public void SetMsgParts(Dictionary<string, string> parts)
        {
            msgEles = parts;
        }

        // Set Recipients
        //
        // Set the list of IDs for the recipients
        // @param rec The list of recipient IDs
        public void SetRecipients(List<string> rec)
        {
            to = rec;
        }

        // Set Serialized Message
        //
        // Set the serialized message string
        // @param s The serialized string
        public void SetSerMsg(string s)
        {
            serMsg = s;
        }
        #endregion
    }
}
