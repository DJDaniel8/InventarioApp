using InventarioApp.Models;
using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.BodegasCommands
{
    public class DeleteButtonCommand : CommandBase
    {

        private BodegasViewModel _viewModel;
        public DeleteButtonCommand(BodegasViewModel viewModel)
        {
            _viewModel = viewModel;
        }



        public override async void Execute(object? parameter)
        {
            bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Está seguro de eliminar la bodega?", "Si", "No");
            if (respuesta)
            {
                var bodega = (Bodega)parameter;
                await _viewModel.DeleteBodegaAsync(bodega);
            }
        }
    }
}
