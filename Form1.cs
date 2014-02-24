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
    public partial class Form1 : Form
    {
        public Form1()
        {
            //INSTALLE VJOY
            //http://softlayer-ams.dl.sourceforge.net/project/vjoystick/Beta%202.x/2.0.2%20030114/vJoy_x86x64_I030114.exe
            //les DLL sont supposer se placer tuseul dans le projet mais un DLL fait rien si t'as pas le driver de vJoy installé
            InitializeComponent();
            joystick = new vJoy();
            id = 1;
            ///// Write access to vJoy Device - Basic
            VjdStat status;
            status = joystick.GetVJDStatus(id);
            // Acquire the target
            string prt;
            if ((status == VjdStat.VJD_STAT_OWN) ||
            ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
                prt = String.Format("Failed to acquire vJoy device number {0}.", id);
            else
                prt = String.Format("Acquired: vJoy device number {0}.", id);
            Reset();

            MessageBox.Show(prt);
        }
        vJoy joystick;
        uint id;

        private void Reset()
        {
            //les HID_USAGES c'est l'enum qui contient tout les AXIS qu'on peut utiliser.
            //X, Y, Z, Rotation Z, Y... Tout

            //cette boucle la met tout les axis à "Neutre"
            foreach (var item in (HID_USAGES[]) Enum.GetValues(typeof(HID_USAGES)))
            {
                //16500 c'est la valeur neutre.
                //min = 0
                //max = 33000
                var lel = joystick.SetAxis(16500, id, item);
            }

        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            Reset();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            //Left
            joystick.SetAxis(0, id, HID_USAGES.HID_USAGE_X);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            //Up
            joystick.SetAxis(0, id, HID_USAGES.HID_USAGE_Y);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            //Right
            joystick.SetAxis(33000, id, HID_USAGES.HID_USAGE_X);
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            //Down
            joystick.SetAxis(33000, id, HID_USAGES.HID_USAGE_Y);
        }
    }
}
