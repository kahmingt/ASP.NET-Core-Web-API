using AutoMapper;
using WebApi.Shared.Database.Entity;
using WebApi.Area.Product.Model;

namespace WebApi.Area.Product.Utility;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Database.Products -> Model.Retrieve.ProductList
        CreateMap<Products, ProductListRetrieveModel>()
            .ForMember(dest => dest.CategoryName, opts => opts.MapFrom(src => src.Category.CategoryName));

        //CreateMap<PagedList<Products>, PagedList<ProductListing>>()
        //    .ConvertUsing<PagedListConverter<Products, ProductListing>>();

        // Database.Products -> Model.Retrieve.ProductDetail
        CreateMap<Products, ProductDetailRetrieveModel>()
            .ForMember(dest => dest.CategoryName, opts => opts.MapFrom(src => src.Category.CategoryName))
            .ForMember(dest => dest.SupplierName, opts => opts.MapFrom(src => src.Supplier.CompanyName));

        // Model.Update.Product.Detail -> Database.Products
        CreateMap<ProductDetailUpdateModel, Products>();

        // Model.Create.Product.Detail -> Database.Products
        CreateMap<ProductDetailCreateModel, Products>();


    }
}