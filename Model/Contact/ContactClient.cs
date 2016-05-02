using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact
{
    public class ContactClient
    {
        private string displayName;
        private string status;
        private string userName;

        public ContactClient()
        {
            displayName = "";
            userName = "";
            status = "";
        }

        public ContactClient(string d, string u, string s)
        {
            displayName = d;
            userName = u;
            status = s;
        }

        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                displayName = value;
            }
        }

        public bool Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }
    }
}
