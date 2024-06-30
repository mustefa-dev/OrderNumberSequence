using AutoMapper;
using OneSignalApi.Model;
using OrderNumberSequence.DATA.DTOs;
using OrderNumberSequence.DATA.DTOs.OrderProduct;
using OrderNumberSequence.DATA.DTOs.User;
using OrderNumberSequence.Entities;

namespace OrderNumberSequence.Helpers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        var baseUrl = "http://localhost:5051/";

        CreateMap<Order, Order>();

        CreateMap<AppUser, UserDto>();
        CreateMap<UpdateUserForm, AppUser>();
        CreateMap<AppUser, TokenDTO>();

        CreateMap<RegisterForm, App>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

      
        CreateMap<AppUser, AppUser>();


        // here to add
CreateMap<OrderProduct, OrderProductDto>();
CreateMap<OrderProductForm,OrderProduct>();
CreateMap<OrderProductUpdate,OrderProduct>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
CreateMap<Order, OrderDto>();
CreateMap<OrderForm,Order>();
CreateMap<OrderUpdate,Order>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
CreateMap<Product, ProductDto>();
CreateMap<ProductForm,Product>();
CreateMap<ProductUpdate,Product>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
CreateMap<Message, MessageDto>();
CreateMap<MessageForm,Message>();
CreateMap<MessageUpdate,Message>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
   
}
