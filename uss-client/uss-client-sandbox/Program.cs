using System;
using System.Net.Sockets;
using System.Net;
using uss_client_sandbox.Model;
using ussclientsandbox.Model;

namespace ussclientsandbox
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //create testmessage
            var testSendMsg = new BCMessage(BCCommand.GetModeSwitches, "hiWelt", 34);
            var testReceivedMsg = new BCMessage(testSendMsg.FullMessage.ToArray());

            var firstChar = new Characteristic(CharacteristicType.Temperature, 22, false);
            var firstMode = new Mode("hiii", firstChar);

            string test = firstMode.ToXAML();
            var secondMode = new Mode(test);

            byte switchId = 0b00010110;

            var modeMessage = new BCMessage(BCCommand.SetModeSwitch, secondMode.ToXAML(), switchId);
            var echoReq = new BCMessage(BCCommand.EchoRequest, "0", 0);


            var source = new CancellationTokenSource();
            NetworkManager.ServerName = "localhost";
            NetworkManager.Connect(source);

            NetworkManager.Send(echoReq.FullMessage.ToArray());

            while (true)
            {
                if (NetworkManager.DataToRead)
                {
                    foreach (var item in NetworkManager.ReceivedData)
                    {
                        Console.WriteLine(item);
                    }
                    NetworkManager.DataToRead = false;
                }
            }

            

            Thread.Sleep(1000 * 60 * 5);    // 5min
            source.Cancel();
        }
    }
}