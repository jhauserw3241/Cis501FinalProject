using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public delegate void ClientInputHandler(string action, params object[] vars);

    public delegate void ClientOutputHandler(string action, string param1 = "", string param2 = "");
    
    public enum FlowState { Entry, Access, Main, Exit };
}
