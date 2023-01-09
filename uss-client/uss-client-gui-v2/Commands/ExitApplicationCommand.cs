
using System.Threading;
using uss_client_gui_v2.Commands;

namespace uss_client_gui_v2.ViewModels.Commands
{
    /// <summary>
    /// Exit entire application -> Will call cancellationtokens in the future
    /// </summary>

    public class ExitApplicationCommand : BaseCommand
    {


        public ExitApplicationCommand()
        {

        }

        public override void Execute(object parameter)
        {
            //Notification.Dispose(); //dispose notification manager
            Thread.Sleep(1000);
            System.Windows.Application.Current.Shutdown();  //complete shutdown
        }
    }
}
