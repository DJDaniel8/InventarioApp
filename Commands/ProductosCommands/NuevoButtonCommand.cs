using InventarioApp.Services.Alerts;
using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.ProductosCommands
{
    public class NuevoButtonCommand : CommandBase
    {
        private ProductosViewModel _viewModel;

        public NuevoButtonCommand(ProductosViewModel viewModel)
        {
            _viewModel = viewModel;
        }


        public override async void Execute(object? parameter)
        {
            string respuesta = await App.Current.MainPage.DisplayPromptAsync("Editar", "Ingrese el Nombre del Producto", "Guardar", "Cancelar", "Nombre del Producto", -1, Keyboard.Text);
            if (respuesta != null
                && !String.IsNullOrWhiteSpace(respuesta)
                && respuesta.Length > 0)
            {
                respuesta = respuesta.Trim();
                await _viewModel.AddProductoAsyng(respuesta);
            }
            else if (respuesta != null) ThrowMessage.ShowErrorMessage("Inserte un nombre valido");

        }
    }
}
