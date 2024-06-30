using OrderNumberSequence.Entities;

namespace OrderNumberSequence.Interface
{
    public interface IMessageRepository : IGenericRepository<Message , Guid>
    {
         
    }
}
