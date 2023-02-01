using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uss_client_gui_v2.ViewModels;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui_v2.Commands
{
    internal class ConnectCommand : BaseCommand
    {
        BaseViewModel vm;
        public ConnectCommand(BaseViewModel vm)
        {
            this.vm = vm;
        }

        public override void Execute(object parameter)
        {
            NetworkManager.Connect();

            if (!NetworkManager.ConnectionError)
            {
                // try getting modes list 
                NetworkManager.Send(new BCMessage(BCCommand.GetModes, "null", 0));
                Thread.Sleep(2000);
                NetworkManager.Send(new BCMessage(BCCommand.GetSwitches, "null", 0));
            }
            vm.UpdateEntireUI();
        }
    }
}
