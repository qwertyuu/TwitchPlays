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
            int total = Form1.charWidth * 2;
            int minimum = End.Length + 3;
            int totalMinusMinimum = total - minimum;
            if (total - minimum >= Beginning.Length)
            {
                return Beginning + End.PadLeft((Form1.charWidth * 2 - 1) - Beginning.Length, '.');
            }
            else
            {
                string tempBeginning = Beginning.Substring(0, total - minimum);
                return tempBeginning + End.PadLeft((Form1.charWidth * 2 - 1) - tempBeginning.Length, '.');
            }
        }
    }
}
