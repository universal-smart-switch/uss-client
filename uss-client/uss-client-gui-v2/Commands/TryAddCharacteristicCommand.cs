using HandyControl.Tools.Command;
using REghZyFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uss_client_gui_v2.ViewModels;

namespace uss_client_gui_v2.Commands
{
    internal class TryAddCharacteristicCommand : CommandBase
    {
        private ModesDashboardViewModel vm;

        public TryAddCharacteristicCommand(ModesDashboardViewModel vm)
        {
            this.vm = vm;
        }

        protected override void OnExecute(object parameter)
        {
            vm.SelectedMode.CharacteristicsToMet.Add(new ussclientsandbox.Models.Characteristic(ussclientsandbox.Models.CharacteristicType.Blank,0,false));
            vm.UpdateEntireUI();
        }
    }
}
