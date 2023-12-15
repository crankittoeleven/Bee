using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class NewPost
    {
        public string token { get; set; }
        public int AuthorId { get; set; }
        public string Group { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }
}