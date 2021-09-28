using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace OnlineQuee.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        [Key]
        public int Id { get; set; }
        [JsonProperty(PropertyName= "first_name")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName= "last_name")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName= "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName= "activated")]
        public int Activated { get; set; }
        [JsonProperty(PropertyName= "forbidden")]
        public int Forbidden { get; set; }
        [JsonProperty(PropertyName= "language")]
        public string Language { get; set; }
        [JsonProperty(PropertyName = "category_id")]
        public int CategoryId { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "resource_url")]
        public string ResourceURL { get; set; }
    }
}
