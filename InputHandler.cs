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
        private vJoy player1;
        private vJoy player2;
        private string lastInput;
        private string currentInput;

        public InputHandler()
        {
            player1 = new vJoy();
            player2 = new vJoy();
            ///// Write access to vJoy Device - Basic
            VjdStat status = player1.GetVJDStatus(1);
            
            // Acquire the target
            string prt;
            if ((status == VjdStat.VJD_STAT_OWN) ||
            ((status == VjdStat.VJD_STAT_FREE) && (!player1.AcquireVJD(1))))
                prt = String.Format("Failed to acquire vJoy device number {0}.", 1);
            else
                prt = String.Format("Acquired: vJoy device number {0}.", 1);
            MessageBox.Show(prt);


            status = player2.GetVJDStatus(2);

            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) ||
            ((status == VjdStat.VJD_STAT_FREE) && (!player2.AcquireVJD(2))))
                prt = String.Format("Failed to acquire vJoy device number {0}.", 2);
            else
                prt = String.Format("Acquired: vJoy device number {0}.", 2);
            MessageBox.Show(prt);
            lastInput = "";
            commands["left"] = new Func<uint, bool>(Left);
            commands["right"] = new Func<uint, bool>(Right);
            commands["up"] = new Func<uint, bool>(Up);
            commands["down"] = new Func<uint, bool>(Down);
            commands["a"] = new Func<uint, bool>(A);
            commands["b"] = new Func<uint, bool>(B);
            commands["start"] = new Func<uint, bool>(Start);
        }

        private bool Start(uint player)
        {
            vJoy playa = player == 1 ? player1 : player2;
            return playa.SetBtn(true, player, 5);
        }

        private bool Right(uint player)
        {
            vJoy playa = player == 1 ? player1 : player2;
            return playa.SetDiscPov(1, player, 1);
        }

        private bool Left(uint player)
        {
            vJoy playa = player == 1 ? player1 : player2;
            return playa.SetDiscPov(3, player, 1);
        }

        private bool Up(uint player)
        {
            vJoy playa = player == 1 ? player1 : player2;
            return playa.SetDiscPov(0, player, 1);
        }

        private bool Down(uint player)
        {
            vJoy playa = player == 1 ? player1 : player2;
            return playa.SetDiscPov(2, player, 1);
        }

        private bool B(uint player)
        {
            vJoy playa = player == 1 ? player1 : player2;
            return playa.SetBtn(true, player, 2);
        }

        private bool A(uint player)
        {
            vJoy playa = player == 1 ? player1 : player2;
            return playa.SetBtn(true, player, 1);
        }

        public void Handle(string str)
        {
            uint player;
            currentInput = ParseInput(str, out player);
            if (player == 1)
            {
                Reset(player);
            }
            else if(player == 2)
            {
                Reset(player);
            }
            if (commands.ContainsKey(currentInput))
                commands[currentInput].DynamicInvoke(player);
        }

        private string ParseInput(string str, out uint player)
        {
            string[] parts = str.Trim().Split(' ');
            if (parts.Length != 2 || parts[0].Length != 1)
            {
                player = 0;
                return "";
            }
            else
            {
                if (uint.TryParse(parts[0], out player))
                {
                    return parts[1];
                }
                else
                {
                    player = 0;
                    return "";
                }
            }
        }

        //les HID_USAGES c'est l'enum qui contient tout les AXIS qu'on peut utiliser.
        //X, Y, Z, Rotation Z, Y... Tout

        //cette boucle la met tout les axis à "Neutre"
        //foreach (var item in (HID_USAGES[])Enum.GetValues(typeof(HID_USAGES)))
        //{
        //    //16500 c'est la valeur neutre.
        //    //min = 0
        //    //max = 33000
        //    var lel = joystick.SetAxis(16500, player, item);
        //}
        /*
         *   0
         * 3   1
         *   2
         */
        private void Reset(uint player)
        {
            vJoy playa = player == 1 ? player1 : player2;
            bool code = false;
            for (uint i = 1; i <= 2; i++)
            {
                code = playa.SetDiscPov(-1, player, i);
                code = playa.SetBtn(false, player, i);
            }
            for (uint i = 3; i <= 5; i++)
            {
                code = playa.SetBtn(false, player, i);
            }
            System.Threading.Thread.Sleep(50);
            lastInput = currentInput;
        }
    }
}
