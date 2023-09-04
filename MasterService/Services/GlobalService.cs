using MasterService.Dto.DataTable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Services
{
    public class GlobalService
    {
        private static IConfiguration configuration;

        public GlobalService()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public string GetConfiguration(string key)
        {
            string result = configuration.GetSection(key).Value;
            return result;
        }

        #region Supprot Database

        public T MapDataRow<T>(DataRow row) where T : new()
        {
            T obj = new T();
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (row.Table.Columns.Contains(property.Name) && row[property.Name] != DBNull.Value)
                {
                    property.SetValue(obj, Convert.ChangeType(row[property.Name], property.PropertyType));
                }
            }

            return obj;
        }

        public SqlCommand InsertCommand<T>(SqlConnection connection, string tableName, T obj)
        {
            var properties = obj.GetType().GetProperties();
            string columns = string.Join(", ", properties.Select(p => p.Name));
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            string insertSql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            SqlCommand command = new SqlCommand(insertSql, connection);

            foreach (var property in properties)
            {
                command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(obj));
            }

            return command;
        }

        #endregion
    }
}
