using ServiciosEducativosComunitarios.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEducativosComunitarios.Repositories
{
    public class ServiceRepository : RepositoryBase, IServiceRepository
    {
        public void Add(ServiceModel serviceModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO [Service] ([code], [localityId], [period], [program], [status]) VALUES(@Code, @LocalityId, @Period, @Program, @Status)";
                command.Parameters.AddWithValue("@Code", serviceModel.Code);
                command.Parameters.AddWithValue("@LocalityId", serviceModel.LocalityId);
                command.Parameters.AddWithValue("@Period", serviceModel.Period);
                command.Parameters.AddWithValue("@Program", serviceModel.Program);
                command.Parameters.AddWithValue("@Status", serviceModel.Status);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(ServiceModel serviceModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM [Service] WHERE [id] = @Id";
                command.Parameters.AddWithValue("@Id", serviceModel.Id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Update(ServiceModel serviceModel)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [Service] SET [code]=@Code, [localityId]=@LocalityId, [period]=@Period, [program]=@Program, [status]=@Status WHERE [id]=@Id";
                command.Parameters.AddWithValue("@Code", serviceModel.Code);
                command.Parameters.AddWithValue("@LocalityId", serviceModel.LocalityId);
                command.Parameters.AddWithValue("@Period", serviceModel.Period);
                command.Parameters.AddWithValue("@Program", serviceModel.Program);
                command.Parameters.AddWithValue("@Status", serviceModel.Status);
                command.Parameters.AddWithValue("@Id", serviceModel.Id);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public IEnumerable<ServiceModel> GetAll()
        {
            var services = new List<ServiceModel>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT [Service].[id], [Service].[code], [Service].[localityId], [Locality].[code] AS 'locality', [Service].[period], [Service].[program], [Service].[status] FROM [Service] LEFT JOIN [Locality] ON [Locality].[id] = [Service].[localityId]";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var service = new ServiceModel
                        {
                            Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            Code = reader.IsDBNull(1) ? null : reader.GetString(1),
                            LocalityId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            Locality = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Period = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            Program = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                            Status = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                            IsDirty = false
                        };

                        services.Add(service);
                    }
                }

                connection.Close();
            }

            return services;
        }

        public bool CodeExists(ServiceModel serviceModel)
        {
            bool exists = false;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM  [Service] WHERE [Id] != @id AND [Code] = @code";
                command.Parameters.AddWithValue("@id", serviceModel.Id);
                command.Parameters.AddWithValue("@code", serviceModel.Code);
                exists = command.ExecuteScalar() != null;
            }

            return exists;
        }
    }
}
