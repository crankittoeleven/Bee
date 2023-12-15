using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class Settings
    {
        public string token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ReEmail { get; set; }
        public bool IsInvisible { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}