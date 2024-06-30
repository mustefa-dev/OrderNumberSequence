namespace OrderNumberSequence.Entities
{
    public class Product : BaseEntity<Guid>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? Image { get; set; }
        
    }
}
