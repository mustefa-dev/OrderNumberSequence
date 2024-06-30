using AutoMapper;
using OrderNumberSequence.DATA;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Interface;

namespace OrderNumberSequence.Repository
{

    public class ProductRepository : GenericRepository<Product , Guid> , IProductRepository
    {
        public ProductRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
