﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talktoyeat.Domain.Entities
{
    public class ImageUri
    {
        public int Id { get; set; }
        public string Uri { get; set; }

        //Navigation props
        public Post Post { get; set; }
    }
}
