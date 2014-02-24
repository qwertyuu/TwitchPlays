using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using vJoyInterfaceWrap;

namespace TwitchPlays
{
    class InputHandler
    {
        private Dictionary<String, Delegate> commands = new Dictionary<String, Delegate>();
        private vJoy joystick;
        private uint id;

        public InputHandler(vJoy joy, uint _id)
        {
            joystick = joy;
            id = _id;
            commands["reset"] = new Action(Reset);
            commands["left"] = new Action(Left);
            commands["right"] = new Action(Right);
            commands["up"] = new Action(Up);
            commands["down"] = new Action(Down);
        }

        private void Reset()
        {
            //les HID_USAGES c'est l'enum qui contient tout les AXIS qu'on peut utiliser.
            //X, Y, Z, Rotation Z, Y... Tout

            //cette boucle la met tout les axis à "Neutre"
            foreach (var item in (HID_USAGES[])Enum.GetValues(typeof(HID_USAGES)))
            {
                //16500 c'est la valeur neutre.
                //min = 0
                //max = 33000
                var lel = joystick.SetAxis(16500, id, item);
            }

        }

        private void Left()
        {
            joystick.SetAxis(0, id, HID_USAGES.HID_USAGE_X);
        }

        private void Up()
        {
            joystick.SetAxis(0, id, HID_USAGES.HID_USAGE_Y);
        }

        private void Right()
        {
            joystick.SetAxis(33000, id, HID_USAGES.HID_USAGE_X);
        }

        private void Down()
        {
            joystick.SetAxis(33000, id, HID_USAGES.HID_USAGE_Y);
        }

        public void Handle(String str)
        {
            str.ToLower();
            if (commands.ContainsKey(str))
                commands[str].DynamicInvoke();
        }
    }
}
