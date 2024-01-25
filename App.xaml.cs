using InventarioApp.Services.DAO;

namespace InventarioApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            BodegaDAO.CargarBodegas();
            ProductoDAO.CargarProductos();
            Productos_BodegasDAO.CargarInventarios();
            MainPage = new AppShell();
        }
    }
}
