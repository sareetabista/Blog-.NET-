﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talktoyeat.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Description { get; set; }

        //password

        // public string Email { get; set; }
        public string Password { get; set; }
        //Nevigation props
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
