using MasterService.Dto.DataTable;
using MasterService.Dto.Request;
using MasterService.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace MasterService
{
    public partial class MasterServiceController : IMasterService
    {
        private readonly PublicService publicService = new PublicService();
        public List<Users> GetUsersByKeyword(GetUsersByKeyword req)
        {
            List<Users> result = new List<Users>();
            using (var conn = DbConnectionFactory.GetSqlConnection())
            {
                try
                {
                    // Create an SQL command
                    string sqlQuery = @"SELECT * FROM Users WHERE first_name LIKE @Keyword OR last_name LIKE @Keyword OR email LIKE @Keyword";
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.Add(new SqlParameter("@Keyword", "%" + req.Keyword + "%"));
                    // Execute the SQL command and retrieve the results
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Users user = publicService.MapDataReader<Users>(reader);
                            result.Add(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }
    }
}
