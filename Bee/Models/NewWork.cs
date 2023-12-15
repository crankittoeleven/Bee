using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class NewWork
    {
        public string token { get; set; }
        public int WorkId { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Size { get; set; }
    }
}