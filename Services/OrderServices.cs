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
    Task<(OrderDto? order, string? error)> GetById(Guid id);
    Task<(Order? order, string? error)> Update(Guid id, OrderUpdate orderUpdate);
    Task<(string? done, string? error)> Approve(Guid id, Guid userId);
    Task<(string? done, string? error)> Delivered(Guid id, Guid userId);
    Task<(string? done, string? error)> Cancel(Guid id, Guid userId);
    Task<(string? done, string? error)> Reject(Guid id, Guid userId);

    Task<(Order? order, string? error)> Delete(Guid id);
    Task<(List<Order>? orders, string? error)> CreateBulk(OrderForm orderForm, Guid UserId);
}

public class OrderServices : IOrderServices
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly OrderNumberGenerator _orderNumberGenerator;

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
            var order = _mapper.Map<Order>(orderForm);
            order.OrderNumber = await _orderNumberGenerator.GetNextOrderNumber();

            var car = await _repositoryWrapper.Product.Get(x =>
                x.Id == orderForm.OrderProducts.FirstOrDefault().ProductId);
            if (car == null) return (null, "Car not found");

            var addedOrder = await _repositoryWrapper.Order.Add(order);
            if (addedOrder == null) return (null, "Order couldn't be created");

            return (order, null);
        }


        public async Task<(List<OrderDto> orders, int? totalCount, string? error)> GetAll(OrderFilter filter)
        {
            var orders = await _repositoryWrapper.Order.GetAll<OrderDto>(
                x =>
                    (filter.OrderNumber == null || x.OrderNumber == filter.OrderNumber)
                , filter.PageNumber, filter.PageSize);
            return (orders.data, orders.totalCount, null);
        }

        public async Task<(OrderDto? order, string? error)> GetById(Guid id)
        {
            var order = await _repositoryWrapper.Order.GetAll<OrderDto>(x => x.Id == id);
            if (order.data == null)
                return (null, "Order not found");
            return (order.data.FirstOrDefault(), null);
        }

        public async Task<(Order? order, string? error)> Update(Guid id, OrderUpdate orderUpdate)
        {
            throw new NotImplementedException();

        }

        public async Task<(string? done, string? error)> Approve(Guid id, Guid userId)
        {
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);
            if (order == null) return (null, "الطلب غير موجود");
            var update = await _repositoryWrapper.Order.Update(order);
            if (update == null) return (null, "لا يمكن الموافقة على الطلب");



            return ("تمت الموافقة على الطلب", null);
        }

        public async Task<(string? done, string? error)> Delivered(Guid id, Guid userId)
        {
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);

            if (order == null) return (null, "الطلب غير موجود");
            var update = await _repositoryWrapper.Order.Update(order);

            if (update == null) return (null, "لا يمكن تسليم الطلب");
            return ("تم تسليم الطلب", null);
        }

        public async Task<(string? done, string? error)> Cancel(Guid id, Guid userId)
        {
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);
            if (order == null) return (null, "الطلب غير موجود");
            var update = await _repositoryWrapper.Order.Update(order);

            if (update == null) return (null, "لا يمكن الغاء الطلب");
            return ("تم الغاء الطلب", null);

        }

        public async Task<(string? done, string? error)> Reject(Guid id, Guid userId)
        {
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);
            if (order == null) return (null, "الطلب غير موجود");

            var update = await _repositoryWrapper.Order.Update(order);
            if (update == null) return (null, "لا يمكن رفض الطلب");
            return ("تم رفض الطلب", null);

        }



        public async Task<(Order? order, string? error)> Delete(Guid id)
        {
            var order = await _repositoryWrapper.Order.Get(x => x.Id == id);
            if (order == null) return (null, "الطلب غير موجود");
            var delete = await _repositoryWrapper.Order.SoftDelete(id);
            if (delete == null) return (null, "لا يمكن حذف الطلب");
            return (order, null);
        }


        public async Task<(List<Order>? orders, string? error)> CreateBulk(OrderForm orderForm, Guid userId)
        {
            List<Order> createdOrders = new List<Order>();

            for (int i = 0; i < 15; i++)
            {
                var result = await Create(orderForm, userId);
                if (result.order != null)
                {
                    createdOrders.Add(result.order);
                }
                else
                {
                    return (null, result.error);
                }
            }

            return (createdOrders, null);
        }
    }