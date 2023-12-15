using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public User Author { get; set; }

        public Post Post { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }
    }
}