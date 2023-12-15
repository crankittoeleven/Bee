using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [JsonIgnore]
        public string Password { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string CityOfBirth { get; set; }

        public string CountryOfBirth { get; set; }

        public DateTime? Birthdate { get; set; }

        public string Occupation { get; set; }

        public string Work { get; set; }

        public string College { get; set; }

        public string School { get; set; }

        public string Relationship { get; set; }

        public bool IsInvisible { get; set; }

        public bool IsOnline { get; set; }

        public bool PrivatePosts { get; set; }

        public bool PrivateFriends { get; set; }

        public bool PrivatePictures { get; set; }

        public bool PrivateCV { get; set; }

        public bool PrivateEmail { get; set; }

        public string Website { get; set; }
    }
}