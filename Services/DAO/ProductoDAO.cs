using InventarioApp.Models;
using InventarioApp.ViewModels;
using InventarioApp.Commands.ProductosCommands;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using InventarioApp.Services.Alerts;

namespace InventarioApp.Services.DAO
{
    public class ProductoDAO
    {
        public static NpgsqlConnection conexion { get; } = new NpgsqlConnection(DBConection.CadenaDeConexion);

        public static ObservableCollection<Producto> Productos { get; set; }

        public static void CargarProductos()
        {
            var resultado = GetAll();
            Productos = new ObservableCollection<Producto>(resultado);
        }

        public static List<Producto> GetAll()
        {
            List<Producto> productos = new();
            try
            {
                conexion.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM productos", conexion);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Producto producto = new();
                    producto.Id = reader.GetInt32(0);
                    producto.Nombre = reader.GetString(1);
                    productos.Add(producto);
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                ThrowMessage.ShowErrorMessage(ex.Message);
                if (conexion.State == System.Data.ConnectionState.Open) conexion.Close();
            }
            return productos;
        }

        public static async Task<int> Insert(Producto producto)
        {
            int idProducto = 0;
            try
            {
                await conexion.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO productos(nombre) VALUES(@nombre) RETURNING id", conexion);
                command.Parameters.AddWithValue("@nombre", producto.Nombre);
                object id = await command.ExecuteScalarAsync();
                idProducto = Convert.ToInt32(id);
                await conexion.CloseAsync();
                return idProducto;
            }
            catch (Exception ex)
            {
                if (ex is NpgsqlException)
                {
                    string errorCode = ((NpgsqlException)ex).SqlState;
                    if (errorCode == "23505")
                    {
                        ThrowMessage.ShowErrorMessage("Ya existe una bodega con ese nombre");
                    }
                }
                else ThrowMessage.ShowErrorMessage(ex.Message);
                if (conexion.State == System.Data.ConnectionState.Open) await conexion.CloseAsync();
                return 0;
            }
        }

        public static async Task<bool> Update(Producto producto, string nuevoNombre)
        {
            bool respuesta = false;
            try
            {
                await conexion.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand("UPDATE productos SET nombre = @nombre WHERE id = @id", conexion);
                command.Parameters.AddWithValue("@nombre", nuevoNombre);
                command.Parameters.AddWithValue("@id", producto.Id);
                int filasAfectadas = await command.ExecuteNonQueryAsync();
                if (filasAfectadas > 0)
                {
                    respuesta = true;
                }
                await conexion.CloseAsync();
            }
            catch (Exception ex)
            {
                if (ex is NpgsqlException)
                {
                    string errorCode = ((NpgsqlException)ex).SqlState;
                    if (errorCode == "23505")
                    {
                        ThrowMessage.ShowErrorMessage("Ya existe una bodega con ese nombre");
                    }
                }
                else ThrowMessage.ShowErrorMessage(ex.Message);
                if (conexion.State == System.Data.ConnectionState.Open) await conexion.CloseAsync();
            }
            return respuesta;
        }

        public static async Task<bool> Delete(Producto producto)
        {
            bool respuesta = false;
            try
            {
                await conexion.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand("DELETE FROM productos WHERE id = @id", conexion);
                command.Parameters.AddWithValue("@id", producto.Id);
                int filasAfectadas = await command.ExecuteNonQueryAsync();
                if (filasAfectadas > 0)
                {
                    respuesta = true;
                }
                await conexion.CloseAsync();
            }
            catch (Exception ex)
            {
                ThrowMessage.ShowErrorMessage(ex.Message);
                if (conexion.State == System.Data.ConnectionState.Open) await conexion.CloseAsync();
            }
            return respuesta;
        }
    }
}
