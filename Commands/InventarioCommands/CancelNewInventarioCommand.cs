using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.InventarioCommands
{
    public class CancelNewInventarioCommand : CommandBase
    {
        private NuevoInventarioViewModel _viewModel;

        public CancelNewInventarioCommand(NuevoInventarioViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            await Shell.Current.GoToAsync("//InventarioView");
        }
    }
}
