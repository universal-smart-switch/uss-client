using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static ObservableCollection<string> _possibleCharacteristics = new ObservableCollection<string>();

        public static void LinkSwitchModes()
        {
            foreach (var sw in SwitchList)
            {
                foreach (var mo in ModeList)
                {
                    if (mo.Name == sw.Mode)
                    {
                        sw.VirtMode = mo; break;
                    }
                }
            }
        }

        public static SwitchList SwitchList { get => _switchList; set => _switchList = value; }
        public static ModeList ModeList { get => _modeList; set => _modeList = value; }
        public static ObservableCollection<string> PossibleCharacteristics { get => _possibleCharacteristics; set => _possibleCharacteristics = value; }


    }
}
