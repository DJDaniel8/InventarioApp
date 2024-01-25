using InventarioApp.Models;
using InventarioApp.Services.Alerts;
using InventarioApp.Services.DAO;
using InventarioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Commands.BodegasCommands
{
    public class EditButtonCommand : CommandBase
    {
        private BodegasViewModel _viewModel;
        public EditButtonCommand(BodegasViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            if (parameter != null)
            {
                string respuesta = await App.Current.MainPage.DisplayPromptAsync("Editar", "Ingrese el Nombre", "Guardar", "Cancelar", "", -1, Keyboard.Text, ((Bodega)parameter).Nombre.ToString());
                if (!String.IsNullOrWhiteSpace(respuesta)
                    && respuesta.Length > 0)
                {
                    respuesta = respuesta.Trim();
                    bool resultado = await BodegaDAO.Update((Bodega)parameter, respuesta);
                    if (resultado) ((Bodega)parameter).Nombre = respuesta;
                }
                else if (respuesta != null) ThrowMessage.ShowErrorMessage("Inserte un nombre valido");
            }
        }
    }
}
