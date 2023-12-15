﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bee.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public User User { get; set; }
    }
}