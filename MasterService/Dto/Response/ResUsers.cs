﻿using MasterService.Dto.DataTable;

namespace MasterService.Dto.Request
{
    public class ResLogin
    {
        public string user_name { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_token { get; set; }
    }
}
