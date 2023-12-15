using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class NewEducation
    {
        public string token { get; set; }
        public int EducationId { get; set; }
        public string Title { get; set; }
        public string Institute { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Description { get; set; }
    }
}