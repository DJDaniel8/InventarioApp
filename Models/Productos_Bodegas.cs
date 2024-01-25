using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventarioApp.Models
{
    public class Productos_Bodegas : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public Producto Producto { get; set; } = new();
        public Bodega Bodega { get; set; } = new();
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }

        private int _Cantidad;
        public int Cantidad
        {
            get
            {
                return _Cantidad;
            }
            set
            {
                _Cantidad = value;
                OnPropertyChanged(nameof(Cantidad));
            }
        }

        public Productos_Bodegas()
        {

        }

        public Productos_Bodegas(Producto producto, Bodega bodega, ICommand deleteCommand, ICommand editCommand)
        {
            Producto = producto;
            Bodega = bodega;
            DeleteCommand = deleteCommand;
            EditCommand = editCommand;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
