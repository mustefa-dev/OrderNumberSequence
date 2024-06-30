using OrderNumberSequence.Entities;

namespace OrderNumberSequence.Interface
{
    public interface IProductRepository : IGenericRepository<Product , Guid>
    {
         
    }
}
