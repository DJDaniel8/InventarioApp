using InventarioApp.Services.Alerts;
using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.BodegasCommands
{
    public class NuevaButtonCommand : CommandBase
    {
        private BodegasViewModel _viewModel;
        public NuevaButtonCommand(BodegasViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            string respuesta = await App.Current.MainPage.DisplayPromptAsync("Editar", "Ingrese el Nombre de la Bodega", "Guardar", "Cancelar", "Nombre de Bodega", -1, Keyboard.Text);
            if (!String.IsNullOrWhiteSpace(respuesta)
                && respuesta.Length > 0)
            {
                respuesta = respuesta.Trim();
                await _viewModel.AddBodegaAsync(respuesta);
            }
            else if (respuesta != null) ThrowMessage.ShowErrorMessage("Inserte un nombre valido");
        }
    }
}
