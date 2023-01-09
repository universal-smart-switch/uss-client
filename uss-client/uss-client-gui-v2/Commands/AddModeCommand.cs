using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uss_client_gui_v2.ViewModels;
using uss_client_sandbox.Models;

namespace uss_client_gui_v2.Commands
{
    internal class AddModeCommand : BaseCommand
    {
        private ModesDashboardViewModel vm;

        public AddModeCommand(ModesDashboardViewModel vm)
        {
            this.vm = vm;
        }

        public override void Execute(object parameter)
        {
            LocalBridge.ModeList.Add(new ussclientsandbox.Models.Mode("Mode " + (LocalBridge.ModeList.Count + 1).ToString()));
            vm.UpdateEntireUI();
        }
    }
}
