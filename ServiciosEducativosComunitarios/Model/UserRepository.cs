using ServiciosEducativosComunitarios.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Model
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser = false;

            using (var connection = GetConnection()) 
                using (var command = new SqlCommand())
            { 
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] WHERE [username] = @username AND [password] = @password";
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = credential.SecurePassword.ToString();
                validUser = command.ExecuteScalar() != null;
            }

            return validUser;
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public UserModel GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void Remove(string username)
        {
            throw new NotImplementedException();
        }
    }
}
