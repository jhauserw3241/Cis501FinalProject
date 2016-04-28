using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    public delegate void ServerOutputHandler(string action, params object[] vars);

    public enum STATUS {online, away, offline};
}
