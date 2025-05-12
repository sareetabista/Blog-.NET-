namespace Talktoyeat.Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Author> Members { get; set; } = new List<Author>();
    }
}