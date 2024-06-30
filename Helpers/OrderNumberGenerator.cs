using OrderNumberSequence.DATA.DTOs;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Interface;

namespace OrderNumberSequence.Helpers;

public class OrderNumberGenerator
{
    private int _currentOrderNumber;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public OrderNumberGenerator(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
        InitializeCurrentOrderNumber().Wait();
    }

    private async Task InitializeCurrentOrderNumber()
    {
        var lastOrder = await _repositoryWrapper.Order.GetAll<Order>(x => true, 1, 1);
        _currentOrderNumber = lastOrder.data == null || lastOrder.data.Count == 0 ? 0 : lastOrder.data.First().OrderNumber;
    }

    public async Task<int> GetNextOrderNumber()
    {
        await _semaphore.WaitAsync();
        try
        {
            _currentOrderNumber = _currentOrderNumber == 10 ? 1 : _currentOrderNumber + 1;
            return _currentOrderNumber;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}