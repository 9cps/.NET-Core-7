using MasterService.Dto.DataTable;

namespace MasterService.Dto.Request
{
    public class GetUsersByKeyword
    {
        public string keyword { get; set; }
        public string user_token { get; set; }
    }

    public class InsertUser
    {
        public Users users { get; set; }
        public string user_token { get; set; }
    }

    public class LoginByUser
    {
        public string user_name { get; set; }
        public string password { get; set; }
    }
}
