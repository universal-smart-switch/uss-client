using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uss_client_gui_v2.ViewModels;
using uss_client_sandbox.Models;

namespace uss_client_gui_v2.Commands
{
    internal class RemoveModeCommand : BaseCommand
    {
        private ModesDashboardViewModel vm;

        public RemoveModeCommand(ModesDashboardViewModel vm)
        {
            this.vm = vm;
        }

        public override void Execute(object parameter)
        {
            if (vm.SelectedMode != null && !vm.SelectedMode.Name.Contains("Default"))
            {
                int index = LocalBridge.ModeList.IndexOf(vm.SelectedMode);
                index--;
                LocalBridge.ModeList.Remove(vm.SelectedMode);
                vm.SelectedMode = LocalBridge.ModeList[index];
                vm.UpdateEntireUI();
            }
        }
    }
}
