using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioApp.Services.DAO
{
    public static class DBConection
    {
        public static string CadenaDeConexion { get; set; } = "Server=pgservervidor.postgres.database.azure.com;Database=inventario;Port=5432;User Id=myadmin;Password=Password1;Ssl Mode=Require;";
    }
}
