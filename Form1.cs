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
using IrcDotNet;

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
            handler = new InputHandler();
            states = new Stack<string>();
        }
        InputHandler handler;
        Stack<string> states;

        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string text = textBox1.Text;
                textBox1.Clear();
                handler.Handle(text);
            }
        }
    }
}
