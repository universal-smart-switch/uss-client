using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uss_client_gui_v2.Stores;
using uss_client_gui_v2.ViewModels;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui_v2.Commands
{
    public class NavigateToCommand : BaseCommand
    {
        private BaseViewModel vm;

        public NavigateToCommand(BaseViewModel vm)
        {
            this.vm = vm;
        }

        public override void Execute(object parameter)
        {
            NavigationStore.CurrentViewModel = vm;
        }
    }
}
