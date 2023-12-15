using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        public User User { get; set; }

        public Post Post { get; set; }
    }
}