using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineQuee.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public LoginRepsonseData Data { get; set; }
    }
    public class LoginRepsonseData
    {
        public string Token { get; set; }
    }

    public class GetCatigoriesResponse
    {
        public int Code { get; set; }
        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }
        public List<Category> Categories { get; set; }
    }

    public class AddToQueue
    {
        public int Code { get; set; }
        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }
        public Ticket Ticket {get;set;}
    }

}
