using InventarioApp.Models;
using InventarioApp.Services.Alerts;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Services.DAO
{
    public class Productos_BodegasDAO
    {
        public static NpgsqlConnection conexion { get; } = new NpgsqlConnection(DBConection.CadenaDeConexion);

        public static ObservableCollection<Productos_Bodegas> Inventarios { get; set; }

        public static void CargarInventarios()
        {
            var resultado = GetAll();
            Inventarios = new ObservableCollection<Productos_Bodegas>(resultado);
        }

        public static List<Productos_Bodegas> GetAll()
        {
            List<Productos_Bodegas> inventarios = new();
            try
            {
                conexion.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM productos_bodegas", conexion);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Productos_Bodegas inventario = new();
                    inventario.Id = reader.GetInt32(0);
                    int productoId = reader.GetInt32(1);
                    inventario.Producto =  ProductoDAO.Productos.Where(p => p.Id == productoId).FirstOrDefault();
                    int bodegaId = reader.GetInt32(2);
                    inventario.Bodega = BodegaDAO.Bodegas.Where(b => b.Id == bodegaId).FirstOrDefault();
                    inventario.Cantidad = reader.GetInt32(3);
                    inventarios.Add(inventario);
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                ThrowMessage.ShowErrorMessage(ex.Message);
                if (conexion.State == System.Data.ConnectionState.Open) conexion.Close();
            }
            return inventarios;
        }

        public static async Task<bool> Insert(Producto producto, Bodega bodega, int cantidad)
        {
            bool respuesta = false;
            try
            {
                await conexion.OpenAsync();
                string consulta = "INSERT INTO productos_bodegas(\"productoId\", \"bodegaId\", cantidad) VALUES(@productoId, @bodegaId, @cantidad)";
                NpgsqlCommand command = new NpgsqlCommand(consulta, conexion);
                command.Parameters.AddWithValue("@productoId", producto.Id);
                command.Parameters.AddWithValue("@bodegaId", bodega.Id);
                command.Parameters.AddWithValue("@cantidad", cantidad);
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
                        ThrowMessage.ShowErrorMessage("Ya Existe un Inventario con ese producto y la misma bodega porfavor verifica");
                    }
                }
                else ThrowMessage.ShowErrorMessage(ex.Message);
                if (conexion.State == System.Data.ConnectionState.Open) await conexion.CloseAsync();
            }
            return respuesta;
        }

        public static async Task<bool> Update(Productos_Bodegas inventario, int cantidad)
        {
            bool respuesta = false;
            try
            {
                await conexion.OpenAsync();
                string consulta = "UPDATE productos_bodegas SET cantidad = @cantidad WHERE id = @id";
                NpgsqlCommand command = new NpgsqlCommand(consulta, conexion);
                command.Parameters.AddWithValue("@cantidad", cantidad);
                command.Parameters.AddWithValue("@id", inventario.Id);
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

        public static async Task<bool> Delete(Productos_Bodegas inventario)
        {
            bool respuesta = false;
            try
            {
                await conexion.OpenAsync();
                string consulta = "DELETE FROM productos_bodegas WHERE id = @id";
                NpgsqlCommand command = new NpgsqlCommand(consulta, conexion);
                command.Parameters.AddWithValue("@id", inventario.Id);
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
