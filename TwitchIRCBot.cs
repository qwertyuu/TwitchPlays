using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meebey.SmartIrc4net;

namespace TwitchPlays
{
    class TwitchIRCBot
    {
        IrcClient lel = new IrcClient();
        public TwitchIRCBot()
        {
            lel.Connect("irc.twitch.tv", 6667);
            lel.Login("twitchplaysmortalkombat4", "", 0, "twitchplaysmortalkombat4", "oauth:l9u905e27ueea0z3gp559wlm83pn91f");
            
        }
    }
}
