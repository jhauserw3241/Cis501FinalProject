using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    public class ServerUser
    {
        private List<ServerUser> contacts;
        private string ipAddress;
        private string name;
        private string password;
        private bool status;
        private string username;

        // Add Contact
        //
        // Add a contact to the list of contacts
        // @param contact The user to be added as another contact for the current user
        public void AddContact(ServerUser contact)
        {
            contacts.Add(contact);
        }

        // Get Username
        //
        // Get the username for the current user
        // @return a string containing the username
        public string GetUsername()
        {
            return username;
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






    }
}
