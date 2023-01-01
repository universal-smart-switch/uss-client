using System.Runtime.CompilerServices;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

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

            //var getSw = new BCMessage(BCCommand.GetSwitches, "0", 0);

            var source = new CancellationTokenSource();
            NetworkManager.Connect();

            /*
            var ch1 = new Characteristic(CharacteristicType.Temperature, 22, false);
            var ch2 = new Characteristic(CharacteristicType.Temperature, 21, true);
            var testMode = new Mode("mode1");
            var testMode2 = new Mode("secondMode");
            testMode.CharacteristicsToMet.Add(ch2);
            testMode.CharacteristicsToMet.Add(ch1);
            testMode2.CharacteristicsToMet.Add(ch1);
            LocalBridge.ModeList.Add(testMode);
            LocalBridge.ModeList.Add(testMode2);

            string xml = LocalBridge.ModeList.ToXML();
            LocalBridge.ModeList.FromXML(xml);*/

            SwitchList swiTest = new SwitchList();
            var sw = new Switch();
            sw.Address = "addr";
            sw.Name = "switchy";
            swiTest.Add(sw);
            var swLR = new BCMessage(BCCommand.GetSwitchesRep, swiTest.ToXML(), 0);
            //NetworkManager.Send(swLR);

            Console.WriteLine(LocalBridge.ModeList.ToXML());

            var mdR = new BCMessage(BCCommand.GetModes, "0", 0);
            //NetworkManager.Send(mdR);
            //NetworkManager.Send(echoReq);


            var md = new Mode("einModus", new Characteristic(CharacteristicType.Temperature, 13, false));
            md.OnSingle = true;
            var md1 = new Mode("zweiterModus", new Characteristic(CharacteristicType.Date, 33, false));

            LocalBridge.ModeList.Add(md);
            LocalBridge.ModeList.Add(md1);

            var mdl = new BCMessage(BCCommand.GetModesRep, LocalBridge.ModeList.ToXML(), 0);

            NetworkManager.Send(mdl);

            ThreadPool.QueueUserWorkItem(new WaitCallback(SendThread), source.Token);

            
            while (true)
            {

            }
            
        }

        private static void SendThread(object obj)
        {
            var token = (CancellationToken)obj;   // get cancellationtoken

            var getSw = new BCMessage(BCCommand.GetSwitches, "0", 0);
           // var echoReq = new BCMessage(BCCommand.EchoReq, "0", 0);

            while (!token.IsCancellationRequested)
            {
                //NetworkManager.Send(echoReq);
                Thread.Sleep(3000);
                NetworkManager.Send(getSw);
                Thread.Sleep(4000);
                toCancel = true;
            }
        }
    }
}