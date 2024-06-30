using OrderNumberSequence.Helpers;
using OrderNumberSequence.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OrderNumberSequence.DATA.DTOs;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Services;

namespace OrderNumberSequence.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductServices _productServices;

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll([FromQuery] ProductFilter filter) => Ok(await _productServices.GetAll(filter) , filter.PageNumber , filter.PageSize);

        
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] ProductForm productForm) => Ok(await _productServices.Create(productForm));

        
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update([FromBody] ProductUpdate productUpdate, Guid id) => Ok(await _productServices.Update(id , productUpdate));

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(Guid id) =>  Ok( await _productServices.Delete(id));
        
    }
}
