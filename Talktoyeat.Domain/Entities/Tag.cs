using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talktoyeat.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        //Navigation props
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
