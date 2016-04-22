using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public delegate void ClientInputHandler(FormInput action);

    public delegate void ClientOutputHandler(FormOutput action);
    
    public enum DispState { Offline, Online };

    public enum FlowState { Disconnected, LoggingIn, SigningUp, Connected, LoggingOut };

    public enum FormInput { };

    public enum FormOutput { };
}
