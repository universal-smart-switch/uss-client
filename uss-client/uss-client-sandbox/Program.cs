using System;
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
        }
    }
}