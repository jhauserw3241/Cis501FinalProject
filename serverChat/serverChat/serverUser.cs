using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    public class ServerUser
    {
        private List<ServerUser> contacts = new List<ServerUser> { };
        private int id;
        private string name;
        private string password;
        private STATUS status;
        private string username;

        #region Class Manipulation
        // Constructor
        public ServerUser()
        {
        }

        // Constructor
        //
        // @param uName The username for the user
        public ServerUser(string uName)
        {
            username = uName;
            name = uName;
        }
        #endregion

        #region Getters
        // Get Contact List
        //
        // Get the list of contacts for the current user
        // @return the list of user objects
        public List<ServerUser> GetContactList()
        {
            return contacts;
        }

        // Get Contact List Names
        //
        // Get the list of display names for the contacts for the current user
        // @return the list of display names for the contact list
        public List<string> GetContactListNames()
        {
            List<string> dNames = new List<string>();
            int size = contacts.Count;

            for (int i = 0; i < size; i++)
            {
                dNames.Add(contacts.ElementAt(i).GetName());
            }

            return dNames;
        }

        // Get Contact List Statuses
        //
        // Get the list of statuses for the contacts for the current user
        // @return the list of status for the contact list in string form
        public List<string> GetContactListStatuses()
        {
            List<string> statuses = new List<string>();
            int size = contacts.Count;

            for (int i = 0; i < size; i++)
            {
                statuses.Add(contacts.ElementAt(i).GetStatus().ToString());
            }

            return statuses;
        }

        // Get Contact List Usernames
        //
        // Get the list of usernames for the contacts for the current user
        // @return the list of usernames for the contact list
        public List<string> GetContactListUsernames()
        {
            List<string> usernames = new List<string>();
            int size = contacts.Count;

            for (int i = 0; i < size; i++)
            {
                usernames.Add(contacts.ElementAt(i).GetUsername());
            }

            return usernames;
        }

        // Get ID
        //
        // Get the ID for the current user
        // @return an integer containing the ID
        public int GetId()
        {
            return id;
        }

        // Get ID String
        //
        // Get the ID string for the current user
        // @return a string containing the ID
        public string GetIdString()
        {
            return id.ToString();
        }

        // Get Name
        //
        // Get the display name for the current user
        // @return a string containing the display name
        public string GetName()
        {
            return name;
        }

        // Get Password
        //
        // Get the password for the current user
        // @return a string containing the password
        public string GetPassword()
        {
            return password;
        }

        // Get Status
        //
        // Get the status for the current user
        // @return a boolean that determines whether or not the user is online
        public STATUS GetStatus()
        {
            return status;
        }

        // Get Username
        //
        // Get the username for the current user
        // @return a string containing the username
        public string GetUsername()
        {
            return username;
        }
        #endregion

        #region Modify
        // Add Contact
        //
        // Add a contact to the list of contacts
        // @param contact The user to be added as another contact for the current user
        public void AddContact(ServerUser contact)
        {
            contacts.Add(contact);
        }

        // Remove Contact
        //
        // Remove a contact from the list of contacts
        // @param contact The user to be removed as a contact for the current user
        public void RemoveContact(ServerUser contact)
        {
            contacts.Remove(contact);
        }

        // Remove Contact
        //
        // Remove a contact from the list of contacts
        // @param contact The string that contains the username of the contact to be removed
        public void RemoveContact(string username)
        {
            int size = contacts.Count;

            for (int i = 0; i < size; i++)
            {
                // Get the current element information
                ServerUser contact = contacts.ElementAt(i);

                // Check if the current contact is the specfied contact
                if (username == contact.GetUsername())
                {
                    contacts.Remove(contact);
                    break;
                }
            }
        }
        #endregion

        #region Setters
        // Set ID
        //
        // Set the ID for the current user
        // @param i The ID for the current user
        public void SetID(int i)
        {
            id = i;
        }

        // Set Name
        //
        // Set the display name of the current user
        // @param dName A string containing the display name
        public void SetName(string dName)
        {
            name = dName;
        }

        // Set Password
        //
        // Set the password for the current user
        // @param pass A string containing the password for the current user
        public void SetPassword(string pass)
        {
            password = pass;
        }

        // Set Status
        //
        // Set the status of the current user
        // @param status Whether the user is online, away, or offline
        public void SetStatus(STATUS s)
        {
            status = s;
        }

        // Set Username
        //
        // Set the username for the current user
        // @param name A string containing the username for the current user
        public void SetUsername(string name)
        {
            username = name;
        }
        #endregion
    }
}
