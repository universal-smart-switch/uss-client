using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui_v2.Commands
{
    public class SendModesCommand : BaseCommand
    {
        public override void Execute(object parameter)
        {
            NetworkManager.Send(new BCMessage(BCCommand.GetModesRep, LocalBridge.ModeList.ToXML(), 0));

            if (LocalBridge.AlsoUpdateSwitchesOnMode)
            {
                Thread.Sleep(2000);
                NetworkManager.Send(new BCMessage(BCCommand.GetSwitchesRep, LocalBridge.SwitchList.ToXML(), 0));
                LocalBridge.AlsoUpdateSwitchesOnMode = false;
            }
        }
    }
}
