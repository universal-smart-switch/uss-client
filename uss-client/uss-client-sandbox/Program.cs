using System.Runtime.CompilerServices;
using uss_client_sandbox.Model;
using ussclientsandbox.Model;

namespace ussclientsandbox
{
    internal class Program
    {
        private static bool toCancel = false;

        static void Main(string[] args)
        {
            //create testmessage
            //var testSendMsg = new BCMessage(BCCommand.GetModeSwitches, "hiWelt", 34);
            //var testReceivedMsg = new BCMessage(testSendMsg.FullMessage.ToArray());

            var firstChar = new Characteristic(CharacteristicType.Temperature, 22, false);
            var firstMode = new Mode("hiii", firstChar);

            string test = firstMode.ToXAML();
            var secondMode = new Mode(test);

            byte switchId = 0b00010110;

            //var modeMessage = new BCMessage(BCCommand.SetModeSwitch, secondMode.ToXAML(), switchId);
            var echoReq = new BCMessage(BCCommand.EchoReq, "0", 0);

            var reEcho = new BCMessage(echoReq.FullMessage.ToArray());

            var getSw = new BCMessage(BCCommand.GetSwitches, "0", 0);

            var source = new CancellationTokenSource();
            NetworkManager.Connect(source);

            NetworkManager.Send(echoReq);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendThread), source.Token);

            
            while (true)
            {

            }
            
        }

        private static void SendThread(object obj)
        {
            var token = (CancellationToken)obj;   // get cancellationtoken

            var getSw = new BCMessage(BCCommand.GetSwitches, "0", 0);
            var echoReq = new BCMessage(BCCommand.EchoReq, "0", 0);

            while (!token.IsCancellationRequested)
            {
                NetworkManager.Send(echoReq);
                Thread.Sleep(3000);
                NetworkManager.Send(getSw);
                Thread.Sleep(4000);
                toCancel = true;
            }
        }
    }
}