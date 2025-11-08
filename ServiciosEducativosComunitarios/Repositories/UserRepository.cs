using ServiciosEducativosComunitarios.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiciosEducativosComunitarios.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public void Add(UserModel userModel)
        {

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText =
                    "INSERT INTO [User] VALUES(@Id, @Username," +
                    " @Password, @Name, @LastName, @Email)";
                command.Parameters.AddWithValue("@Id", userModel.Id);
                command.Parameters.AddWithValue("@Username",
                    userModel.Username);
                command.Parameters.AddWithValue("@Password",
                    userModel.Password);
                command.Parameters.AddWithValue("@Name",
                    userModel.Name);
                command.Parameters.AddWithValue("@LastName",
                    userModel.LastName);
                command.Parameters.AddWithValue("@Email",
                    userModel.Email);
                command.ExecuteNonQuery();
                connection.Close();
            }


        }

        public void Delete(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [User] WHERE Username = @Username";
                command.Parameters.AddWithValue("@Username", userModel.Username);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser = false;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from  [User] where [username] = @username and [password] = @password";
                command.Parameters.Add("@username",
                    System.Data.SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password",
                    System.Data.SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() != null;
            }

            return validUser;
        }

        public void Update(UserModel userModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET  Password=@Password, Name=@Name, LastName=@LastName, Email=@Email WHERE UserName=@UserName";
                //command.Parameters.AddWithValue("@Id", userModel.Id);
                command.Parameters.AddWithValue("@UserName", userModel.Username);
                command.Parameters.AddWithValue("@Name", userModel.Name);
                command.Parameters.AddWithValue("@Password", userModel.Password);
                command.Parameters.AddWithValue("@LastName", userModel.LastName);
                command.Parameters.AddWithValue("@Email", userModel.Email);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select *from [User] where username=@username";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader[0] == null ? 0 : int.Parse(reader[0].ToString()),
                            Username = reader[1].ToString(),
                            Password = string.Empty,
                            Name = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString(),
                        };
                    }
                }
            }
            return user;
        }
    }
}
