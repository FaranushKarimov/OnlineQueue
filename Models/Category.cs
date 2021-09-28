using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineQuee.Models
{
    public class Category
    {
        [JsonProperty(PropertyName = "id")]
        [Key]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CraetedAt { get; set; }
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdateAt  { get; set; }
        [JsonProperty(PropertyName = "resource_url")]
        public string ResourceUrl { get; set; }
        [JsonProperty(PropertyName = "users")]
        public List<User> Users { get; set; }
    }
}
