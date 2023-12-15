using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Author"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? Author_Id { get; set; }

        [ForeignKey("Owner"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? Owner_Id { get; set; }

        public virtual User Author { get; set; }

        public virtual User Owner { get; set; }

        public DateTime Created { get; set; }

        public string Group { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public string Type { get; set; }
    }
}