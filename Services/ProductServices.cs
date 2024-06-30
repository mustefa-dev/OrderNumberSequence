
using AutoMapper;
using OrderNumberSequence.Repository;
using OrderNumberSequence.Services;
using OrderNumberSequence.DATA.DTOs;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Interface;

namespace OrderNumberSequence.Services;


public interface IProductServices
{
Task<(Product? product, string? error)> Create(ProductForm productForm );
Task<(List<ProductDto> products, int? totalCount, string? error)> GetAll(ProductFilter filter);
Task<(Product? product, string? error)> Update(Guid id , ProductUpdate productUpdate);
Task<(Product? product, string? error)> Delete(Guid id);
}

public class ProductServices : IProductServices
{
private readonly IMapper _mapper;
private readonly IRepositoryWrapper _repositoryWrapper;

public ProductServices(
    IMapper mapper ,
    IRepositoryWrapper repositoryWrapper
    )
{
    _mapper = mapper;
    _repositoryWrapper = repositoryWrapper;
}
   
   
public async Task<(Product? product, string? error)> Create(ProductForm productForm )
{
    var product = _mapper.Map<Product>(productForm);
    var result = await _repositoryWrapper.Product.Add(product);
    return result == null ? (null, "product could not add") : (product, null);
}

public async Task<(List<ProductDto> products, int? totalCount, string? error)> GetAll(ProductFilter filter)
    {
        var (products, totalCount) = await _repositoryWrapper.Product.GetAll<ProductDto>(
            x=> (filter.Name == null || x.Name.Contains(filter.Name)) &&
                (filter.Price == null || x.Price == filter.Price)
                , filter.PageNumber, filter.PageSize
        );
        var productDto = _mapper.Map<List<ProductDto>>(products);
        return (productDto, totalCount, null);
        
    }

public async Task<(Product? product, string? error)> Update(Guid id ,ProductUpdate productUpdate)
    {
        var product = await _repositoryWrapper.Product.GetById(id);
        if (product == null)
        {
            return (null, "product not found");
        }
        var updatedProduct = _mapper.Map(productUpdate, product);
        var result = await _repositoryWrapper.Product.Update(updatedProduct);
        return result == null ? (null, "product could not update") : (updatedProduct, null);
    }

public async Task<(Product? product, string? error)> Delete(Guid id)
    {
        var product = await _repositoryWrapper.Product.GetById(id);
        if (product == null)
        {
            return (null, "product not found");
        }
        var result = await _repositoryWrapper.Product.SoftDelete(id);
        return result == null ? (null, "product could not delete") : (product, null);
    }

}
