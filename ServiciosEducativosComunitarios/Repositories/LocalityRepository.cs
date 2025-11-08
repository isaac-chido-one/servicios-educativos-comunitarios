using ServiciosEducativosComunitarios.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Repositories
{
    public class LocalityRepository : RepositoryBase, ILocalityRepository
    {
        public void Add(LocalityModel localityModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO [Locality] ([code], [municipio], [comunidad], [ambito], [latitud], [longitud], [poblacion]) VALUES (@Code, @Municipio, @Comunidad, @Ambito, @Latitud, @Longitud, @Poblacion)";
                command.Parameters.AddWithValue("@Code", localityModel.Code);
                command.Parameters.AddWithValue("@Municipio", localityModel.Municipio);
                command.Parameters.AddWithValue("@Comunidad", localityModel.Comunidad);
                command.Parameters.AddWithValue("@Ambito", localityModel.Ambito);
                command.Parameters.AddWithValue("@Latitud", localityModel.Latitud);
                command.Parameters.AddWithValue("@Longitud", localityModel.Longitud);
                command.Parameters.AddWithValue("@Poblacion", localityModel.Poblacion);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(LocalityModel localityModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [Locality] WHERE [id] = @Id";
                command.Parameters.AddWithValue("@Id", localityModel.Id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        public void Update(LocalityModel localityModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [Locality] SET [code]=@Code, [municipio]=@Municipio, [comunidad]=@Comunidad, [ambito]=@Ambito, [latitud]=@Latitud, [longitud]=@Longitud, [poblacion]=@Poblacion WHERE [id]=@Id";
                command.Parameters.AddWithValue("@Code", localityModel.Code);
                command.Parameters.AddWithValue("@Municipio", localityModel.Municipio);
                command.Parameters.AddWithValue("@Comunidad", localityModel.Comunidad);
                command.Parameters.AddWithValue("@Ambito", localityModel.Ambito);
                command.Parameters.AddWithValue("@Latitud", localityModel.Latitud);
                command.Parameters.AddWithValue("@Longitud", localityModel.Longitud);
                command.Parameters.AddWithValue("@Poblacion", localityModel.Poblacion);
                command.Parameters.AddWithValue("@Id", localityModel.Id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public IEnumerable<LocalityModel> GetAll()
        {
            var localities = new List<LocalityModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT [id], [code], [municipio], [comunidad], [ambito], [latitud], [longitud], [poblacion] FROM [Locality]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var locality = new LocalityModel
                        {
                            Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            Code = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Municipio = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            Comunidad = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Ambito = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            Latitud = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Longitud = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Poblacion = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                        };

                        localities.Add(locality);
                    }
                }

                connection.Close();
            }

            return localities;
        }
    }
}
