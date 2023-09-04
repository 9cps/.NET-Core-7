using MasterService.Dto.DataTable;
using MasterService.Dto.Request;
using MasterService.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MasterService
{
    public partial class MasterServiceController : IMasterService
    {
        private readonly GlobalService publicService = new GlobalService();
        public List<Users> GetUsersByKeyword(GetUsersByKeyword req)
        {
            List<Users> result = new List<Users>();
            if (!JwtTokenAuthen.IsTokenExpired(req.user_token))
            {
                using (var conn = DbConnectionFactory.GetSqlConnection())
                {
                    try
                    {
                        // Create an SQL command
                        string sqlQuery = @"SELECT * FROM Users WHERE first_name LIKE @KEYWORD OR last_name LIKE @KEYWORD OR email LIKE @KEYWORD";
                        SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                        cmd.Parameters.Add(new SqlParameter("@KEYWORD", "%" + req.keyword + "%"));
                        // Execute the SQL command and retrieve the results
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            foreach (DataRow row in dataTable.Rows)
                            {
                                Users obj = new Users();
                                obj = publicService.MapDataRow<Users>(row);
                                result.Add(obj);
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
            }
            return result;
        }

        public Users InsertUser(InsertUser req)
        {
            Users result = new Users();
            using (var conn = DbConnectionFactory.GetSqlConnection())
            {
                try
                {
                    req.users.password = JwtTokenAuthen.HashPassword(req.users.password);

                    // Create an SQL command
                    SqlCommand insertCommand = publicService.InsertCommand(conn, "Users", req.users);

                    // Execute the INSERT command
                    _ = insertCommand.ExecuteNonQuery();
                    result = req.users;
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

        public ResLogin LoginByUser(LoginByUser req)
        {
            ResLogin result = new ResLogin();
            using (var conn = DbConnectionFactory.GetSqlConnection())
            {
                try
                {
                    // Create an SQL command
                    string sqlQuery = @"SELECT * FROM Users WHERE user_name = @USERNAME";
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                    cmd.Parameters.Add(new SqlParameter("@USERNAME", req.user_name));
                    // Execute the SQL command and retrieve the results
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        Users obj = new Users();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            obj = publicService.MapDataRow<Users>(row);
                        }

                        if (JwtTokenAuthen.VerifyPassword(obj.password, req.password))
                        {
                            result.email = obj.email;
                            result.user_name = obj.user_name;
                            result.last_name = obj.last_name;
                            result.first_name = obj.first_name;
                            result.user_token = JwtTokenAuthen.GenerateToken(obj.user_name);
                        }
                        else
                        {
                            throw new Exception("Can't login.");
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
