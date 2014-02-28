using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using vJoyInterfaceWrap;

namespace TwitchPlays
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        List<CustomString> commands;
        Size singleCharSize;
        public static int charWidth = 30;
        int charHeight = 20;
        IrcBot bot;
        public Form1()
        {
            //INSTALLE VJOY
            //http://softlayer-ams.dl.sourceforge.net/project/vjoystick/Beta%202.x/2.0.2%20030114/vJoy_x86x64_I030114.exe
            //les DLL sont supposer se placer tuseul dans le projet mais un DLL fait rien si t'as pas le driver de vJoy installé
            AllocConsole();
            InitializeComponent();
            bot = new IrcBot();
            commands = new List<CustomString>();
            singleCharSize = TextRenderer.MeasureText("a", richTextBox1.Font);
            
            this.BackColor = Color.Black;
            richTextBox1.BackColor = this.BackColor;
            richTextBox1.ForeColor = Color.White;
            charWidth = richTextBox1.Width / singleCharSize.Width;
            charHeight = richTextBox1.Height / singleCharSize.Height;
            bot.IssuedCommand += bot_IssuedCommand;
        }

        void bot_IssuedCommand(IrcBot iB, CommandEventArgs e)
        {
            if (e.Command == "player")
            {
                commands.Add(new CustomString() { Beginning = e.Player + " is now player", End = e.NewPlayer.ToString() });
            }
            else
            {
                commands.Add(new CustomString() { Beginning = e.Player, End = e.Command });
            }
            ShowText();
        }

        private bool toInterface(string toShow)
        {
            richTextBox1.Text = toShow;
            return true;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            charWidth = richTextBox1.Width / singleCharSize.Width;
            charHeight = richTextBox1.Height / singleCharSize.Height;
            ShowText();
        }

        private void ShowText()
        {
            StringBuilder toShow = new StringBuilder();
            if (commands.Count > charHeight)
            {
                commands.RemoveAt(0);
            }
            for (int i = 0; i < commands.Count; i++)
            {
                if (i != 0)
                {
                    toShow.Append(Environment.NewLine);
                }
                toShow.Append(commands[i]);
            }
            richTextBox1.Invoke(new Func<string, bool>(toInterface), toShow.ToString());
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
