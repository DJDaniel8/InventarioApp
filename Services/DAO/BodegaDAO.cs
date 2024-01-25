using InventarioApp.Models;
using InventarioApp.ViewModels;
using InventarioApp.Commands.BodegasCommands;
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
    public class BodegaDAO
    {
        public static NpgsqlConnection conexion { get; } = new NpgsqlConnection(DBConection.CadenaDeConexion);

        public static ObservableCollection<Bodega> Bodegas { get; set; }

        public static void CargarBodegas()
        {
            var resultado = GetAll();
            Bodegas = new ObservableCollection<Bodega>(resultado);
        }

        public static List<Bodega> GetAll()
        {
            List<Bodega> bodegas = new();
            try
            {
                conexion.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM bodegas", conexion);
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Bodega bodega = new();
                    bodega.Id = reader.GetInt32(0);
                    bodega.Nombre = reader.GetString(1);
                    bodegas.Add(bodega);
                }
                conexion.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return bodegas;
        }

        public static async Task<int> Insert(Bodega bodega)
        {
            int idBodega = 0;
            try
            {
                await conexion.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO bodegas(nombre) VALUES(@nombre) RETURNING id", conexion);
                command.Parameters.AddWithValue("@nombre", bodega.Nombre);
                object id = await command.ExecuteScalarAsync();
                idBodega = Convert.ToInt32(id);
                await conexion.CloseAsync();
                return idBodega;
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

        public static async Task<bool> Update(Bodega bodega, string nuevoNombre)
        {
            bool respuesta = false;
            try
            {
                await conexion.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand("UPDATE bodegas SET nombre = @nombre WHERE id = @id", conexion);
                command.Parameters.AddWithValue("@nombre", nuevoNombre);
                command.Parameters.AddWithValue("@id", bodega.Id);
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

        public static async Task<bool> Delete(Bodega bodega)
        {
            bool respuesta = false;
            try
            {
                await conexion.OpenAsync();
                NpgsqlCommand command = new NpgsqlCommand("DELETE FROM bodegas WHERE id = @id", conexion);
                command.Parameters.AddWithValue("@id", bodega.Id);
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
