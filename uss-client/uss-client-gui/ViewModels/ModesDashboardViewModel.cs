using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uss_client_sandbox.Models;
using ussclientsandbox.Models;

namespace uss_client_gui.ViewModels
{
    public partial class ModesDashboardViewModel : BaseViewModel
    {

        public ModeList ModeList 
        { 
            get 
            {
                NetworkManager.Send(new BCMessage(BCCommand.GetModes, "null", 0));
                Thread.Sleep(1000);
                return LocalBridge.ModeList;
            }
        }
    }
}
