using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class NewLanguage
    {
        public string token { get; set; }
        public int LanguageId { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
    }
}