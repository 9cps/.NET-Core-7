using MasterService.Dto.Request;
using MasterService.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MasterService
{   
    public partial class MasterServiceController : IMasterService
    {
        public bool HelpCheckConnection()
        {
            bool result = false;
            using (var conn = DbConnectionFactory.GetSqlConnection())
            {
                try
                {
                    result = true;
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
