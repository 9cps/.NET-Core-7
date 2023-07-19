using MasterService.Dto.DataTable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Services
{
    public class PublicService
    {
        private static IConfiguration configuration;

        public PublicService()
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

        public T MapDataReader<T>(SqlDataReader reader) where T : new()
        {
            T obj = new T();

            // Loop through the properties of T using reflection
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                var value = reader.GetValue(i);

                var prop = typeof(T).GetProperty(fieldName);
                if (prop != null && value != DBNull.Value)
                {
                    prop.SetValue(obj, value);
                }
            }

            return obj;
        }
    }
}
