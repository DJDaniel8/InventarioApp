using InventarioApp.Commands.InventarioCommands;
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
	[QueryProperty(nameof(InventarioViewModel), "InventarioViewModel")]
    public class NuevoInventarioViewModel : ViewModelBase
    {
		public ICommand NewInventarioCommand => new NewInventarioCommand(this);
		public ICommand CancelNewInventarioCommad => new CancelNewInventarioCommand(this);

        public NuevoInventarioViewModel()
        {
            Productos = ProductoDAO.Productos;
            Bodegas = BodegaDAO.Bodegas;
        }

		private InventarioViewModel _InventarioViewModel;
		public InventarioViewModel InventarioViewModel
		{
			get
			{
				return _InventarioViewModel;
			}
			set
			{
				_InventarioViewModel = value;
				OnPropertyChanged(nameof(InventarioViewModel));
			}
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

		private Producto _SelectedProducto;
		public Producto SelectedProducto
		{
			get
			{
				return _SelectedProducto;
			}
			set
			{
				_SelectedProducto = value;
				OnPropertyChanged(nameof(SelectedProducto));
			}
		}

		private Bodega _SelectedBodega;
		public Bodega SelectedBodega
		{
			get
			{
				return _SelectedBodega;
			}
			set
			{
				_SelectedBodega = value;
				OnPropertyChanged(nameof(SelectedBodega));
			}
		}

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

		//Metodo para agregar un nuevo inventario
		public async Task AddInventarioAsync()
		{
            bool respuesta = await Productos_BodegasDAO.Insert(SelectedProducto, SelectedBodega, Cantidad);
			if (respuesta)
			{
				Productos_Bodegas pb = new Productos_Bodegas();
				pb.Producto = SelectedProducto;
				pb.Bodega = SelectedBodega;
				pb.Cantidad = Cantidad;
				pb.EditCommand = _InventarioViewModel.EditCommand;
				pb.DeleteCommand = _InventarioViewModel.DeleteCommand;
                Productos_BodegasDAO.Inventarios.Add(pb);
				_InventarioViewModel.FiltrarProductosBodegasAsync();
                await Shell.Current.GoToAsync("//InventarioView");
            }
		}
	}
}
