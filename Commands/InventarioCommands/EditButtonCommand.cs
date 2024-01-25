using InventarioApp.Models;
using InventarioApp.Services.Alerts;
using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.InventarioCommands
{
    public class EditButtonCommand : CommandBase
    {
        private InventarioViewModel _ViewModel;

        public EditButtonCommand(InventarioViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            if(parameter != null)
            {
                string respuesta = await App.Current.MainPage.DisplayPromptAsync("Editar", "Ingrese la cantidad", "Guardar", "Cancelar", "", -1, Keyboard.Numeric, ((Productos_Bodegas)parameter).Cantidad.ToString());
                int cantidad = 0;
                if (!String.IsNullOrEmpty(respuesta) 
                    && Int32.TryParse(respuesta, out cantidad)
                    && cantidad > 0)
                {
                    _ViewModel.UpdateCantidadProductoBodega((Productos_Bodegas)parameter, cantidad);
                }
                else if(respuesta != null)
                {
                    ThrowMessage.ShowErrorMessage("Ingrese un numero entero positivo");
                }
            }
        }
    }
}
