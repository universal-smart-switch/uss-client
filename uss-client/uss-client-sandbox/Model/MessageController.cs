using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ussclientsandbox.Model;

namespace uss_client_sandbox.Model
{
    public static class MessageController
    {
        public static void ReceiveController(BCMessage message)
        {
            //Console.WriteLine("[Bridge]: " + message.DataString);

            switch (message.Command)
            {
                case BCCommand.Invalid:
                    break;
                case BCCommand.EchoReq:
                    // save current date (Is tis really the only way to do it ?!)
                    NetworkManager.LastEchoRequest = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second,DateTime.Now.Millisecond);
                    NetworkManager.Send(new BCMessage(BCCommand.EchorRep, DefinedInformation.DateTimeToUnix(NetworkManager.LastEchoRequest).ToString(), 0));
                    break;
                case BCCommand.EchorRep:
                    long unixTime = long.Parse(message.DataString); // convert to long
                    DefinedInformation.UnixToDateTime(unixTime);    // convert to datetime
                    var timeSpanPing = DateTime.Now.Subtract(NetworkManager.LastEchoRequest);   // get timespan between response and reequest
                    NetworkManager.PingHistory.Add(DateTime.Now, timeSpanPing); // add to history

                    Console.WriteLine("[Brige PING]" + timeSpanPing.ToString());

                    break;
                case BCCommand.GetSwitchesRep:
                    break;
                case BCCommand.GetModesRep:
                    break;
                case BCCommand.GetSysInfo:
                    break;
                default:
                    break;
            }
        }
    }
}
