using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.InventarioCommands
{
    public class NewInventarioCommand : CommandBase
    {
        private NuevoInventarioViewModel _viewModel;

        public NewInventarioCommand(NuevoInventarioViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            bool respuesta = await App.Current.MainPage.DisplayAlert("Agregar", "¿Está seguro de agregar el nuevo Inventario?", "Si", "No");
            if (respuesta)
            {
                await _viewModel.AddInventarioAsync();
            }
        }
    }
}
