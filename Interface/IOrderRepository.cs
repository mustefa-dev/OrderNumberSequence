using OrderNumberSequence.Entities;

namespace OrderNumberSequence.Interface
{
    public interface IOrderRepository : IGenericRepository<Order , Guid>
    {
         
    }
}
