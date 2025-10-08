using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;

        public RepositoryBase()
        {
            _connectionString = "Server=LAPTOP-6DCTAQDT\\VSGESTION; Database=servicios-educativos-comunitarios; Integrated Security=True; TrustServerCertificate=True;";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection( _connectionString );
        }
    }
}
