using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineQuee.Models
{
    public class Ticket
    {

        [JsonProperty(PropertyName = "category_id")]
        public int CategoryId { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty(PropertyName = "status_id")]
        public int StatusId { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "client_id")]
        public int ClientId { get; set; }

        public string Comment { get; set; }
        public string Number { get; set; }
        public int Id { get; set; }
    }
}
