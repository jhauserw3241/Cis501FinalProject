using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public delegate void ClientInputHandler(FormInput action, params object[] vars);

    public delegate void ClientOutputHandler(FormOutput action, params object[] vars);
    
    public enum DispState { Offline, Online };

    public enum FlowState { Entry, Access, Main, Exit };

    public enum FormInput { AddCont, RemoveCont, CreateConv, LeaveCont, AddPart, Message };

    public enum FormOutput { Message };
}
