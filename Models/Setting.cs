using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineQuee.Models
{
    public class Setting
    {
        [Key]
        public string Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; } // we dont need to save pass
        public string URL { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Enable { get; set; }

    }
}
