using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uss_client_gui_v2.Commands;
using uss_client_gui_v2.Stores;
using uss_client_sandbox.Models;

namespace uss_client_gui_v2.Commands
{
    public class SetSwitchStateCommand : BaseCommand
    {
        private Switch sw;

        public SetSwitchStateCommand(Switch sw)
        {
            this.sw = sw;
        }

        public override void Execute(object parameter)
        {
            sw.StateOn = !sw.StateOn;
            NavigationStore.CurrentViewModel.UpdateEntireUI();
        }
    }
}
