using AutoMapper;
using WebApi.Database.Entities;
using WebApi.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Products

        CreateMap<Products, ProductListing>()
            .ForMember(dest => dest.CategoryName, opts => opts.MapFrom(src => src.Category.CategoryName));

        CreateMap<Products, ProductDetails>()
            .ForMember(dest => dest.CategoryName, opts => opts.MapFrom(src => src.Category.CategoryName))
            .ForMember(dest => dest.SupplierName, opts => opts.MapFrom(src => src.Supplier.CompanyName));

        CreateMap<ProductDetails, Products>();

        #endregion
    }
}