using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ussclientsandbox.Model
{
    static class DefinedInformation
    {
        // Bridge/Client messages
        public const string BCMark = "$C";
        public const string BSMark = "$S";

        public const int BCCInvalid = 1;
        public const int BCCGetSwitches = 2;
        public const int BCCGetModeSwitches = 3;
        public const int BCCGetStateSwitch = 4;
        public const int BCCSendSwitches = 5;
        public const int BCCSendModeSwitches = 6;
        public const int BCCSendStateSwitch = 7;
        public const int BCCEchoRequest = 8;
        public const int BCCSetModeSwitch = 9;

        // bluetotoh communication
        public const string BluetoothBridgePin = "welcome";
        public const bool BluetoothAuthentificate = true;
        public const string BluetoothBridgeAdress = "test";
        public const string BridgeCOM = "COM1";

        public const int BridgeCOMBaud = 9600;
        public const Handshake BridgeCOMHandshake = Handshake.None;
        public const Parity BridgeCOMParity = Parity.None;
        public const StopBits BridgeCOMStopBits = StopBits.One;
    }
}
