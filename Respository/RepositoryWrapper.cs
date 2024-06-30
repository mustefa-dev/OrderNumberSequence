using AutoMapper;
using OrderNumberSequence.DATA;
using OrderNumberSequence.Interface;
using OrderNumberSequence.Repository;

namespace OrderNumberSequence.Respository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;


    // here to add
private IOrderProductRepository _OrderProduct;

public IOrderProductRepository OrderProduct {
    get {
        if(_OrderProduct == null) {
            _OrderProduct = new OrderProductRepository(_context, _mapper);
        }
        return _OrderProduct;
    }
}
private IOrderRepository _Order;

public IOrderRepository Order {
    get {
        if(_Order == null) {
            _Order = new OrderRepository(_context, _mapper);
        }
        return _Order;
    }
}
private IProductRepository _Product;

public IProductRepository Product {
    get {
        if(_Product == null) {
            _Product = new ProductRepository(_context, _mapper);
        }
        return _Product;
    }
}
private IMessageRepository _Message;

public IMessageRepository Message {
    get {
        if(_Message == null) {
            _Message = new MessageRepository(_context, _mapper);
        }
        return _Message;
    }
}



    private IUserRepository _user;


    public RepositoryWrapper(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    

    public IUserRepository User
    {
        get
        {
            if (_user == null) _user = new UserRepository(_context, _mapper);
            return _user;
        }
    }

}
