using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hipchat.notification
{
    enum ExitCode : int
    {
        Success = 0,
        ApplicationError = 1,
        InvalidArgs = 2
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var strap = new Bootstrap(args);

                if (!strap.IsValid())
                {
                    Console.WriteLine(strap.HelpMessage);
                    Environment.Exit((int)ExitCode.InvalidArgs);
                }

                Console.WriteLine("Sending message '{0}' to room {1} from {2}",strap.Message,strap.RoomId,strap.From);
                var client = new HipChat.HipChatClient(strap.Token);
                client.SendMessage(strap.Message, strap.RoomId, strap.From); 

                Environment.Exit((int)ExitCode.Success);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                Environment.Exit((int)ExitCode.ApplicationError);
            }
        }
    }
}
