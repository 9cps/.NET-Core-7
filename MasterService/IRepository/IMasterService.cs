using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterService.Dto.DataTable;
using MasterService.Dto.Request;

namespace MasterService
{
    public interface IMasterService
    {
        #region WeatherForecast
        bool HelpCheckConnection();
        #endregion

        #region WeatherForecast
        ResponseWeatherForecast GetWeatherForecast(GetWeatherForecast req);
        #endregion

        #region Users
        List<Users> GetUsersByKeyword(GetUsersByKeyword req);
        Users InsertUser(InsertUser req);
        ResLogin LoginByUser(LoginByUser req);
        #endregion
    }
}
