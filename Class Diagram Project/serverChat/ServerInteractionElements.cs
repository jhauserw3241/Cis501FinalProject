﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    public delegate void ServerInputHandler(string action, params object[] vars);

    public delegate void ServerOutputHandler(string action, params object[] vars);

    public enum STATUS {Online, Away, Offline};
}
