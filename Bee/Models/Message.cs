using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public User From { get; set; }

        public User To { get; set; }

        public string Content { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }
    }
}