using OrderNumberSequence.DATA.DTOs;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Interface;

namespace OrderNumberSequence.Helpers;

public class OrderNumberGenerator
{
    private int _currentOrderNumber = 0;
    private readonly object _lock = new object();
    private readonly IRepositoryWrapper _repositoryWrapper;

    public OrderNumberGenerator(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<int> GetNextOrderNumber()
    {
        var allOrders = await _repositoryWrapper.Order.GetAll<Order>(x => true);
        if (allOrders.data == null || allOrders.data.Count == 0) { return 1; }
        var lastOrderNumber = allOrders.data.OrderByDescending(x => x.CreationDate).First().OrderNumber;
        return lastOrderNumber == 10 ? 1 : lastOrderNumber + 1;
    }



}