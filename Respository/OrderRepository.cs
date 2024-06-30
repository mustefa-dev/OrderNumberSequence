using AutoMapper;
using OrderNumberSequence.DATA;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Interface;

namespace OrderNumberSequence.Repository
{

    public class OrderRepository : GenericRepository<Order , Guid> , IOrderRepository
    {
        public OrderRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
