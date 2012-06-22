using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using CommandLine.Utility;

namespace hipchat.notification
{
    public class Bootstrap
    {
        public string HelpMessage =
                                    @"Valid arguments are:
                                    /message=[put your message here]
                                    ";

        private readonly Arguments _args;
        
        public string Message { get; set; }
        public int RoomId { get; set; }
        public string From { get; set; }
        public string Token { get; set; }

        public Bootstrap(string[] args)
        {
            this._args = new Arguments(args);

            RoomId = Convert.ToInt32(ConfigurationManager.AppSettings["roomid"]);
            From = ConfigurationManager.AppSettings["from"];
            Token = ConfigurationManager.AppSettings["token"];
        }

        public bool IsValid()
        {
            Message = _args["message"];

            return (!string.IsNullOrEmpty(Message)) ;
        }
    }
}
