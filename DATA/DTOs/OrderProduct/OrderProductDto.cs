namespace OrderNumberSequence.DATA.DTOs
{

    public class OrderProductDto: BaseDto<Guid>
    {
        public int Quantity { get; set; }
        public string Name { get; set; }
        public int? Price { get; set; }
        public string? Image { get; set; }
    }
}
