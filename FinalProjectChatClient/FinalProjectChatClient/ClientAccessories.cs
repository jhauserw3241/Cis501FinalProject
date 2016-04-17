using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectChatClient
{
    public delegate void ClientInputHandler(string action);

    public delegate void EntryInputHandler(EntryOption action);

    public delegate void AccountInputHandler(DialogOption action);

    public delegate void ClientOutputHandler(string action);

    public enum EntryOption { Login, Signup };
    public enum DialogOption { Cancel, Ok };
    public enum States { Disconnected, LoggingIn, SigningUp, Connected, LoggingOut };
}
