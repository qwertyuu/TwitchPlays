using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchPlays
{
    class CustomString
    {
        public string Beginning;
        public string End;
        public override string ToString()
        {
            return Beginning + End.PadLeft(Form1.charWidth * 2 - Beginning.Length, '.');
        }
    }
}
