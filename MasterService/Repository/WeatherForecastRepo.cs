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
        public ResponseWeatherForecast GetWeatherForecast(GetWeatherForecast req)
        {
            ResponseWeatherForecast result = new ResponseWeatherForecast();
            using (var conn = DbConnectionFactory.GetSqlConnection())
            {
                try
                {
                    if (string.IsNullOrEmpty(req.Region))
                        throw new Exception("Require data");

                    result.Region = "THA";
                    result.Datetime = DateTime.Now;
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

        public async Task WeatherForecastUploadFile(string base64File, string fileName)
        {
            try
            {
                await S3FileExtensionAWS.UploadBase64FileToS3(base64File, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
