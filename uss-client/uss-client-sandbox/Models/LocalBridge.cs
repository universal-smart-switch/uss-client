using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ussclientsandbox.Models;

namespace uss_client_sandbox.Models
{
    public static  class LocalBridge
    {
        private static SwitchList _switchList = new SwitchList();
        private static ModeList _modeList = new ModeList();

        public static SwitchList SwitchList { get => _switchList; set => _switchList = value; }
        public static ModeList ModeList { get => _modeList; set => _modeList = value; }
    }
}
