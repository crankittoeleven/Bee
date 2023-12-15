using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class NewMessage
    {
        public string token { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}