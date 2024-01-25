using InventarioApp.Commands.BodegasCommands;
using InventarioApp.Models;
using InventarioApp.Services.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventarioApp.ViewModels
{
    public class BodegasViewModel : ViewModelBase
    {
        public BodegasViewModel()
        {
            Bodegas = BodegaDAO.Bodegas;
            SetCommands();
        }

        //Comando para el boton de nueva bodega
        public ICommand NuevaButtonCommand => new NuevaButtonCommand(this);
        //Comando para eliminar una bodega
        public ICommand DeleteButtonCommand => new DeleteButtonCommand(this);
        //Comando para editar una bodega
        public ICommand EditButtonCommand => new EditButtonCommand(this);

        //propiedad para listar las bodegas
        private ObservableCollection<Bodega> _Bodegas;
        public ObservableCollection<Bodega> Bodegas
        {
            get
            {
                return _Bodegas;
            }
            set
            {
                _Bodegas = value;
                OnPropertyChanged(nameof(Bodegas));
            }
        }

        //metodo para agregar una nueva bodega
        public async Task AddBodegaAsync(string nombre)
        {
            Bodega bodega = new();
            bodega.Nombre = nombre;
            bodega.EditCommand = new EditButtonCommand(this);
            bodega.DeleteCommand = new DeleteButtonCommand(this);

            int respuesta = await BodegaDAO.Insert(bodega);

            if (respuesta > 0)
            {
                bodega.Id = respuesta;
                Bodegas.Add(bodega);
            }
        }

        //metodo para eliminar una bodega
        public async Task DeleteBodegaAsync(Bodega bodega)
        {
            bool respuesta = await BodegaDAO.Delete(bodega);

            if(respuesta) Bodegas.Remove(bodega);
        }

        //metodo para establecer los comandos de 
        private void SetCommands()
        {
            foreach (var bodega in Bodegas)
            {
                bodega.EditCommand = EditButtonCommand;
                bodega.DeleteCommand = DeleteButtonCommand;
            }
        }
    }
}
