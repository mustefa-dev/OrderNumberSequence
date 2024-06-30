using AutoMapper;
using OrderNumberSequence.DATA;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Interface;

namespace OrderNumberSequence.Repository
{

    public class OrderProductRepository : GenericRepository<OrderProduct , Guid> , IOrderProductRepository
    {
        public OrderProductRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
