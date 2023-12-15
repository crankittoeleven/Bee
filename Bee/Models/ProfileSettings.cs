using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class ProfileSettings
    {
        public string token { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CityOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public DateTime Birthdate { get; set; }
        public string Occupation { get; set; }
        public string Work { get; set; }
        public string College { get; set; }
        public string School { get; set; }
        public string Relationship { get; set; }
        public string Website { get; set; }
    }
}