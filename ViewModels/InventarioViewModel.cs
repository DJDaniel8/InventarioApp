using InventarioApp.Commands.InventarioCommands;
using InventarioApp.Models;
using InventarioApp.Services.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventarioApp.ViewModels
{
    public class InventarioViewModel : ViewModelBase
    {
        public ICommand LimpiarClick { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand NewInventarioCommand { get; }
        private Timer _timer;

        private ObservableCollection<Productos_Bodegas> _ProductosBodegas = Productos_BodegasDAO.Inventarios;

        public InventarioViewModel()
        {
            NewInventarioCommand = new NewButtonCommand(this);
            DeleteCommand = new DeleteButtonCommand(this);
            EditCommand = new EditButtonCommand(this);
            Bodegas = BodegaDAO.Bodegas;
            Productos = ProductoDAO.Productos;
            ProductosBodegasFiltrado = _ProductosBodegas;
            LimpiarClick = new LimpiarButtonCommand(this);
            SetCommands();
            _timer = new Timer(OnTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
        }

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

        private string _ValorBusqueda = string.Empty;
        public string ValorBusqueda
        {
            get { return _ValorBusqueda; }
            set
            {
                _ValorBusqueda = value;
                OnPropertyChanged(nameof(ValorBusqueda));
                _timer?.Change(500, Timeout.Infinite);
            }
        }

        

        private ObservableCollection<Productos_Bodegas> _ProductosBodegasFiltrado;
        public ObservableCollection<Productos_Bodegas> ProductosBodegasFiltrado
        {
            get { return _ProductosBodegasFiltrado; }
            set
            {
                _ProductosBodegasFiltrado = value;
                OnPropertyChanged(nameof(ProductosBodegasFiltrado));
            }
        }

        private Bodega? _SelectedBodega = null;
        public Bodega? SelectedBodega
        {
            get { return _SelectedBodega; }
            set
            {
                _SelectedBodega = value;
                OnPropertyChanged(nameof(SelectedBodega));
                FiltrarProductosBodegasAsync().ConfigureAwait(false);
            }
        }

        private void OnTimerElapsed(object state)
        {
            FiltrarProductosBodegasAsync().ConfigureAwait(false);
        }

        public async Task FiltrarProductosBodegasAsync()
        { 
            if (string.IsNullOrEmpty(ValorBusqueda))
            {
                    
                ProductosBodegasFiltrado = new ObservableCollection<Productos_Bodegas>(_ProductosBodegas);
            }
            else
            {
                    
                ProductosBodegasFiltrado = await Task.Run(() =>
                {
                    return new ObservableCollection<Productos_Bodegas>(_ProductosBodegas
                        .Where(x => x.Producto.Nombre.ToLower().Contains(ValorBusqueda.ToLower()))
                        .OrderBy(x => x.Producto.Nombre)
                        .ToList());
                }).ConfigureAwait(false);
            }

            if (SelectedBodega != null)
            {
                    
                ProductosBodegasFiltrado = await Task.Run(() =>
                {
                    return new ObservableCollection<Productos_Bodegas>(_ProductosBodegas
                        .Where(x => x.Bodega.Id == SelectedBodega.Id)
                        .ToList());
                }).ConfigureAwait(false);
            }
        }

        //public async Task FiltrarProductosBodegasAsync()
        //{
        //    if (string.IsNullOrEmpty(ValorBusqueda))
        //    {
        //        ProductosBodegasFiltrado = new ObservableCollection<Productos_Bodegas>(_ProductosBodegas);
        //    }
        //    else
        //    {
        //        var filteredList = await Task.Run(() =>
        //        {
        //            return _ProductosBodegas
        //                .Where(x => x.Producto.Nombre.ToLower().Contains(ValorBusqueda.ToLower()))
        //                .OrderBy(x => x.Producto.Nombre)
        //                .ToList();
        //        }).ConfigureAwait(false);

        //        // Actualizar la colección en el hilo de la interfaz de usuario
        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            ProductosBodegasFiltrado = new ObservableCollection<Productos_Bodegas>(filteredList);
        //        });
        //    }

        //    if (SelectedBodega != null)
        //    {
        //        var filteredList = await Task.Run(() =>
        //        {
        //            return _ProductosBodegas
        //                .Where(x => x.Bodega.Id == SelectedBodega.Id)
        //                .ToList();
        //        }).ConfigureAwait(false);

        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            ProductosBodegasFiltrado = new ObservableCollection<Productos_Bodegas>(filteredList);
        //        });
        //    }
        //}

        public async Task EliminarProductoBodega(Productos_Bodegas productoBodega)
        {
            bool respuesta = await Productos_BodegasDAO.Delete(productoBodega);
            if(respuesta)
            {
                _ProductosBodegas.Remove(productoBodega);
                ProductosBodegasFiltrado.Remove(productoBodega);
            }
        }

        public async void UpdateCantidadProductoBodega(Productos_Bodegas productoBodega, int cantidad)
        {
            bool respuesta = await Productos_BodegasDAO.Update(productoBodega, cantidad);
            if(respuesta)
            {
                productoBodega.Cantidad = cantidad;
            }
        }

        private void SetCommands()
        {
            foreach (var inventario in _ProductosBodegas)
            {
                inventario.EditCommand = EditCommand;
                inventario.DeleteCommand = DeleteCommand;
            }
        }
        
    }
}
