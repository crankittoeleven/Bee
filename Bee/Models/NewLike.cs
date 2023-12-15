using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class NewLike
    {
        public string token { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}