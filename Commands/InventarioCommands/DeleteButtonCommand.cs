using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.InventarioCommands
{
    public class DeleteButtonCommand : CommandBase
    {
        private InventarioViewModel _ViewModel;

        public DeleteButtonCommand(InventarioViewModel viewModel)
        {
            _ViewModel = viewModel;
        }


        public override async void Execute(object? parameter)
        {
            bool respuesta = await App.Current.MainPage.DisplayAlert("Eliminar", "¿Está seguro de eliminar el producto?", "Si", "No");
            if (respuesta)
            {
                //busca en la lista de productos bodegas el producto que se va a eliminar segun el id
                var productoBodega = _ViewModel.ProductosBodegasFiltrado.FirstOrDefault(x => x.Id == (int)parameter);
                _ViewModel.EliminarProductoBodega(productoBodega);
            }
        }
    }
}
