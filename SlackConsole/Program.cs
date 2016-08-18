using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Pook.SlackAPI;
using Pook.SlackAPI.RTMMessages;

namespace SlackConsole
{

    public class PokeResponder : IMessageResponder
    {
        public bool CanRespond(Message message, SlackUser user)
        {
            return
                message.text.StartsWith("level?", StringComparison.OrdinalIgnoreCase) ||
                message.text.StartsWith("whats my level?", StringComparison.OrdinalIgnoreCase) ||
                message.text.StartsWith("level", StringComparison.OrdinalIgnoreCase) ||
                message.text.StartsWith("xp", StringComparison.OrdinalIgnoreCase) ||
                message.text.Contains("xp");
        }

        public Task Respond(ISlackSocket socket, Message message, SlackUser user)
        {
            return socket.Send(message.Reply("this is your pokemon level \n fuck you"));
        }
    }

    /// <summary>
    /// Very simple host for Slack bot.
    /// <para>
    /// Invoke from a command line with: slackconsole.exe &lt;bot token&gt;
    /// </para>
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("A Slack token MUST be provided");
                return;
            }

            var token = args[0];
            if (string.IsNullOrEmpty(token))
                throw new Exception("a Slack RTM token MUST be supplied");

            Trace.Listeners.Add(new ConsoleTraceListener());

            var socket = new SlackSocket(token)
                .AddAllEventHandlers()
                .AddAllResponders();
            socket.Login().Wait();
            string channel = "";
            foreach (var item in socket.State.Channels)
            {
                if (item.name == "webhooktester")
                {
                    channel = item.id;
                }
            }
            //socket.SendChannel("Another day another test", channel);
            Console.WriteLine(socket.State.Url);
            Console.Write("Press enter to quit...");
            Console.ReadLine();
        }
    }
}