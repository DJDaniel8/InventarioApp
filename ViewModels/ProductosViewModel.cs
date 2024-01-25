using InventarioApp.Commands.ProductosCommands;
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
    public class ProductosViewModel : ViewModelBase
    {

        public ProductosViewModel()
        {
            Productos = ProductoDAO.Productos;
            SetCommands();
        }

        //Commandos para los botones
        public ICommand NuevoButtonCommand => new NuevoButtonCommand(this);
        public ICommand DeleteButtonCommand => new DeleteButtonCommand(this);
        public ICommand EditButtonCommand => new EditButtonCommand(this);

        //Propiedad para la lista de productos
        private ObservableCollection<Producto> _Productos;
        public ObservableCollection<Producto> Productos
        {
            get
            {
                return _Productos;
            }
            set
            {
                _Productos = value;
                OnPropertyChanged(nameof(Productos));
            }
        }

        //Metodo para agregar un producto
        public async Task AddProductoAsyng(string nombre)
        {
            Producto producto = new Producto();
            producto.Nombre = nombre;
            producto.EditCommand = new EditButtonCommand(this);
            producto.DeleteCommand = new DeleteButtonCommand(this);

            int resultado = await ProductoDAO.Insert(producto);
            if (resultado > 0)
            {
                producto.Id = resultado;
                Productos.Add(producto);
            }
        }

        //Metodo para eliminar un producto
        public async Task DeleteProductoAsync(Producto producto)
        {
            bool resultado = await ProductoDAO.Delete(producto);
            if(resultado) Productos.Remove(producto);
        }

        //Metodo para establecer los comandos de los botones
        public void SetCommands()
        {
            foreach (var producto in Productos)
            {
                producto.EditCommand = EditButtonCommand;
                producto.DeleteCommand = DeleteButtonCommand;
            }
        }
    }
}
