using AutoMapper;

using Microsoft.EntityFrameworkCore;
using OneSignalApi.Model;
using OrderNumberSequence.DATA.DTOs;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Helpers;
using OrderNumberSequence.Helpers.OneSignal;
using OrderNumberSequence.Interface;

namespace OrderNumberSequence.Services;


public interface IOrderServices
{
    Task<(Order? order, string? error)> Create(OrderForm orderForm, Guid UserId);
    Task<(List<OrderDto> orders, int? totalCount, string? error)> GetAll(OrderFilter filter);
    
    Task<(List<Order>? orders, string? error)> CreateBulk(OrderForm orderForm, Guid UserId);
}

public class OrderServices : IOrderServices
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly OrderNumberGenerator _orderNumberGenerator;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public OrderServices(
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper, OrderNumberGenerator orderNumberGenerator)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
            _orderNumberGenerator = orderNumberGenerator;
        }


        public async Task<(Order? order, string? error)> Create(OrderForm orderForm, Guid userId)
        {
            await _semaphore.WaitAsync();
            try
            {
                var order = _mapper.Map<Order>(orderForm);
                order.OrderNumber = await _orderNumberGenerator.GetNextOrderNumber();

                var car = await _repositoryWrapper.Product.Get(x =>
                    x.Id == orderForm.OrderProducts.FirstOrDefault().ProductId);
                if (car == null) return (null, "Product not found");
            
                var addedOrder = await _repositoryWrapper.Order.Add(order);
                if (addedOrder == null) return (null, "Order couldn't be created");

                return (order, null);
            }
            finally
            {
                _semaphore.Release();
            }
            // public async Task<(Order? order, string? error)> Create(OrderForm orderForm, Guid userId)
            // {
            //     var order = _mapper.Map<Order>(orderForm);
            //     order.OrderNumber = await _orderNumberGenerator.GetNextOrderNumber();
            //
            //     var car = await _repositoryWrapper.Product.Get(x => x.Id == orderForm.OrderProducts.FirstOrDefault().ProductId);
            //     if (car == null) return (null, "Car not found");
            //
            //     var addedOrder = await _repositoryWrapper.Order.Add(order);
            //     if (addedOrder == null) return (null, "Order couldn't be created");
            //
            //     return (order, null);
            // }
        }
        public async Task<(List<OrderDto> orders, int? totalCount, string? error)> GetAll(OrderFilter filter)
        {
            var orders = await _repositoryWrapper.Order.GetAll<OrderDto>(
                x =>
                    (filter.OrderNumber == null || x.OrderNumber == filter.OrderNumber)
                , filter.PageNumber, filter.PageSize);
            return (orders.data, orders.totalCount, null);
        }
        
        public async Task<(List<Order>? orders, string? error)> CreateBulk(OrderForm orderForm, Guid userId)
         {
             var tasks = Enumerable.Range(0, 15)
                 .Select(_ => Create(orderForm, userId))
                 .ToArray();
         
             var results = await Task.WhenAll(tasks);
         
             var createdOrders = results
                 .Where(result => result.order != null)
                 .Select(result => result.order)
                 .ToList();
         
             var errors = results
                 .Where(result => result.error != null)
                 .Select(result => result.error)
                 .ToList();
         
             
         
             return (createdOrders, null);
         }
    }
