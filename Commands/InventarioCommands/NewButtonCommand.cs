using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.InventarioCommands
{
    public class NewButtonCommand : CommandBase
    {
        private InventarioViewModel _viewModel;
        public NewButtonCommand(InventarioViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object> 
            {
                { "InventarioViewModel", _viewModel }
            };
            await Shell.Current.GoToAsync("//NuevoInventarioView", parameters);
        }
    }
}
