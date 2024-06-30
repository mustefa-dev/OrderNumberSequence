
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using OrderNumberSequence.DATA.DTOs;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Services;

namespace OrderNumberSequence.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAll([FromQuery] OrderFilter filter) => Ok(await _orderServices.GetAll(filter) , filter.PageNumber , filter.PageSize);
        
        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] OrderForm orderForm) => Ok(await _orderServices.Create(orderForm,Id));
        [HttpPost("bulk")]
        public async Task<ActionResult<List<Order>>> CreateBulk([FromBody] OrderForm orderForm) => Ok(await _orderServices.CreateBulk(orderForm, Id));    }
}
