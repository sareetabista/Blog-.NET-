using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talktoyeat.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int ReadCount { get; set; }

        public bool Published { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
       // public bool IsPublished { get; set; } = false;
        
        
        //credebility score
        // Add this property to your Post entity
        public int CredibilityScore { get; set; } = 100; // Default to 100
        //Navigation props
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public List<ImageUri> Images { get; set; } = new List<ImageUri>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
