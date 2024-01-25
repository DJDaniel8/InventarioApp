using InventarioApp.Models;
using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.ProductosCommands
{
    public class DeleteButtonCommand : CommandBase
    {
        private ProductosViewModel _viewModel;

        public DeleteButtonCommand(ProductosViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Está seguro de eliminar el Producto?", "Si", "No");
            if (respuesta)
            {
                var bodega = (Producto)parameter;
                await _viewModel.DeleteProductoAsync(bodega);
            }
        }
    }
}
