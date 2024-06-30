using AutoMapper;
using OrderNumberSequence.DATA;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Interface;

namespace OrderNumberSequence.Repository
{

    public class MessageRepository : GenericRepository<Message , Guid> , IMessageRepository
    {
        public MessageRepository(DataContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
