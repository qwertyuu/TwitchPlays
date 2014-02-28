using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace TwitchPlays
{
    public class IrcBot
    {
        private InputHandler handler;

        private NetworkStream stream;
        private TcpClient irc;
        private string inputLine;
        private StreamReader reader;

        public static string SERVER = "irc.twitch.tv";
        public static int PORT = 6667;
        private string USER = "USER twitchplaysmortalkombat4 0 * :twitchplaysmortalkombat4";
        private string NICK = "twitchplaysmortalkombat4";
        private string CHANNEL = "#twitchplaysmortalkombat4";
        public static StreamWriter writer;
        Thread botThread;
        public event CommandHandler IssuedCommand;
        public delegate void CommandHandler(IrcBot iB, CommandEventArgs e);
        private PingSender ping;

        public IrcBot()
        {
            try
            {
                handler = new InputHandler();
                irc = new TcpClient(SERVER, PORT);
                stream = irc.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream) { AutoFlush = true };
                ping = new PingSender();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            botThread = new Thread(Run);
            botThread.Start();
        }
        public void Run()
        {
            string commandToRemove = "PRIVMSG #twitchplaysmortalkombat4 :";
            writer.WriteLine("PASS oauth:l9u905e27ueea0z3gp559wlm83pn91f");
            ping.Start();
            writer.WriteLine("NICK " + NICK);
            writer.WriteLine(USER);
            writer.WriteLine("JOIN " + CHANNEL);
            while (true)
            {
                while ((inputLine = reader.ReadLine()) != null)
                {
                    Console.WriteLine(inputLine);
                    int index;
                    if ((index = inputLine.IndexOf(commandToRemove)) != -1)
                    {
                        string nickname = inputLine.Substring(1, inputLine.IndexOf('!') - 1);
                        string command = inputLine.Substring(index + commandToRemove.Length);
                        var successfulCommand = handler.Handle(command, nickname);
                        if (IssuedCommand != null && successfulCommand != null)
                        {
                            IssuedCommand(this, successfulCommand);
                        }
                        Console.WriteLine("{0} a dit: {1}", nickname, command);
                    }
                }
                // Close all streams
                writer.Close();
                reader.Close();
                irc.Close();
            }
        }
    }

    class PingSender
    {
        static string PING = "PING :";
        private Thread pingSender;
        // Empty constructor makes instance of Thread
        public PingSender()
        {
            pingSender = new Thread(this.Run);
        }
        // Starts the thread
        public void Start()
        {
            pingSender.Start();
        }
        // Send PING to irc server every 15 seconds
        public void Run()
        {
            while (true)
            {
                IrcBot.writer.WriteLine(PING + IrcBot.SERVER);
                Thread.Sleep(20000);
            }
        }
    }
}