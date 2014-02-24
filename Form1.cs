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
            handler = new InputHandler(joystick, id);
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

            handler.Handle(String.Format("reset"));

            MessageBox.Show(prt);
        }
        vJoy joystick;
        uint id;
        InputHandler handler;


        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                handler.Handle(textBox1.Text);
                textBox1.Clear();
            }
        }
    }
}
