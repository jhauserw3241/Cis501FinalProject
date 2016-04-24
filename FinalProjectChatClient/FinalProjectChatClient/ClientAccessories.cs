using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public delegate void ClientInputHandler(string action, params object[] vars);

    public delegate void ClientOutputHandler(string action, params object[] vars);
    
    public enum FlowState { Entry, Access, Main, Exit };
}
