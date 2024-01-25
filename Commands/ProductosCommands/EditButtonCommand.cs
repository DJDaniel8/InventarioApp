using InventarioApp.Models;
using InventarioApp.Services.Alerts;
using InventarioApp.Services.DAO;
using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.ProductosCommands
{
    public class EditButtonCommand : CommandBase
    {
        private ProductosViewModel _viewModel;

        public EditButtonCommand(ProductosViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            if (parameter != null)
            {
                string respuesta = await App.Current.MainPage.DisplayPromptAsync("Editar", "Ingrese el Nombre", "Guardar", "Cancelar", "", -1, Keyboard.Text, ((Producto)parameter).Nombre.ToString());
                if (!String.IsNullOrWhiteSpace(respuesta)
                    && respuesta.Length > 0)
                {
                    respuesta = respuesta.Trim();
                    bool resultado = await ProductoDAO.Update((Producto)parameter, respuesta);
                    if (resultado) ((Producto)parameter).Nombre = respuesta;
                }
                else if (respuesta != null) ThrowMessage.ShowErrorMessage("Inserte un nombre valido");
            }
        }
    }
}
