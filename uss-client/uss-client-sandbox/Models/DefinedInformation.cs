using System.IO.Ports;

namespace ussclientsandbox.Models
{
    static class DefinedInformation
    {
        //
        public const bool LocalDebugMode = true;


        // Bridge/Client messages
        public const string BCMarkStart = "$C";
        public const string BCMarkEnd = "$CE";
        public const string BSMark = "$S";

        public const int BCCInvalid = 1;
        public const int BCCEchoReq = 2;
        public const int BCCEchorRep = 3;
        public const int BCCGetSwitches = 4;
        public const int BCCGetSwitchesRep = 5;
        public const int BCCGetModes = 6;
        public const int BCCGetModesRep = 7;
        public const int BCCGetSysInfo = 8;
        public const int BCCGetSysInfoRep = 9;
        public const int BCCSetSwitchState = 10;


        /*public const int BCCGetSwitches = 2;
        public const int BCCGetModeSwitches = 3;
        public const int BCCGetStateSwitch = 4;
        public const int BCCSendSwitches = 5;
        public const int BCCSendModeSwitches = 6;
        public const int BCCSendStateSwitch = 7;
        public const int BCCEchoRequest = 8;
        public const int BCCEchoReply = 9;
        public const int BCCSetModeSwitch = 10;*/

        // bluetotoh communication
        public const string BluetoothBridgePin = "welcome";
        public const bool BluetoothAuthentificate = true;
        public const string BluetoothBridgeAdress = "test";
        public const string BridgeCOM = "COM1";

        public const int BridgeCOMBaud = 9600;
        public const Handshake BridgeCOMHandshake = Handshake.None;
        public const Parity BridgeCOMParity = Parity.None;
        public const StopBits BridgeCOMStopBits = StopBits.One;

        // network communication
        public const int TCPPort = 5000;
        public const string BridgeHostName = "uiw-bridge";


        public static long DateTimeToUnix(DateTime dateTime)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime);
            var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
            return unixDateTime;
        }

        public static DateTime UnixToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
