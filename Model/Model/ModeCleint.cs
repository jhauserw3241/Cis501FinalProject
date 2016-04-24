using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ModelCleint
    {
        private int status;
        private string ipAdd;
        private string dispName;
        private List<string> convs = new List<string>();
        private Dictionary<string, bool> contList = new Dictionary<string, bool>();

        public int Status
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

        public string IpAdd
        {
            get
            {
                return ipAdd;
            }
            set
            {
                ipAdd = value;
            }
        }

        public string DispName
        {
            get
            {
                return dispName;
            }
            set
            {
                dispName = value;
            }
        }

        public List<string> Convs
        {
            get
            {
                return convs;
            }
            set
            {
                convs = value;
            }
        }

        public Dictionary<string, bool> ContList
        {
            get
            {
                return contList;
            }
            set
            {
                contList = value;
            }
        }
    }
}
