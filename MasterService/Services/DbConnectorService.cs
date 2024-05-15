using MasterService.Dto.DataTable;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace MasterService.Services
{
    public static class DbConnectionFactory
    {
        public static DbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            string connectionString = GetConnectionString();
            optionsBuilder.UseSqlServer(connectionString);

            try
            {
                var dbContext = new DbContext(optionsBuilder.Options);
                return dbContext;
            }
            catch (Exception ex)
            {
                // Handle any errors that occurred during database connection
                // (e.g., log the error, show an error message, etc.)
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
                throw;
            }
        }

        public static SqlConnection GetSqlConnection()
        {
            string connectionString = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private static string GetConnectionString()
        {
            string connectionString = string.Empty;
            var publicService = new GlobalService();
            try
            {
                string keyName = $"DatabaseConfig:{publicService.GetConfiguration("ApplicationPhase")}";
                string serverName = publicService.GetConfiguration($"{keyName}:Server");
                string databaseName = publicService.GetConfiguration($"{keyName}:Database");
                string username = publicService.GetConfiguration($"{keyName}:Username");
                string password = publicService.GetConfiguration($"{keyName}:Password");

                if (string.IsNullOrEmpty(serverName) || string.IsNullOrEmpty(databaseName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    throw new Exception("Error can't get configuration db.");

                connectionString = $"Server={serverName};Database={databaseName};User Id={username};Password={password};TrustServerCertificate=True;";
            }
            catch
            {
                throw new Exception("Error can't get configuration db.");
            }
            return connectionString;
        }
    }
}
