using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uss_client_gui_v2.ViewModels;

namespace uss_client_gui_v2.Commands
{
    internal class TryRemoveCharacteristicCommand : BaseCommand
    {

        private ModesDashboardViewModel vm;

        public TryRemoveCharacteristicCommand(ModesDashboardViewModel vm)
        {
            this.vm = vm;
        }

        public override void Execute(object parameter)
        {
           if (vm.SelectedMode.CharacteristicsToMet.Count > 1)
           {
                var chrList = vm.SelectedMode.CharacteristicsToMet;

                chrList.Remove(chrList[chrList.Count - 1]);
           }
            vm.UpdateEntireUI();
        }
    }
}
