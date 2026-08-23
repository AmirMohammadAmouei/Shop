using Riok.Mapperly.Abstractions;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.Products.Dto;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.Products.Mapping
{
    public interface IProductMapper
    {
        Product ToEntity(CreateProductDto request);
        SPFOutPutDto<ProductListResponseDto> ToDtoList(SPFOutPutDto<Product> source);

        void UpdateEntity(UpdateProductDto request, Product entity);
    }

    [Mapper]
    public partial class ProductMapper : IProductMapper
    {
        [MapProperty("ProductCategory.Name", nameof(ProductListResponseDto.CategoryName))]
        [MapProperty(nameof(Product.ProductImages), nameof(ProductListResponseDto.Images))]
        private partial ProductListResponseDto ToDto(Product source);

        public partial SPFOutPutDto<ProductListResponseDto> ToDtoList(SPFOutPutDto<Product> source);
        public partial Product ToEntity(CreateProductDto request);

        [MapperIgnoreSource(nameof(UpdateProductDto.Id))]
        [MapperIgnoreSource(nameof(UpdateProductDto.NewImages))]
        [MapperIgnoreSource(nameof(UpdateProductDto.DeletedImageIds))]
        public partial void UpdateEntity(UpdateProductDto request, Product entity);
    }
}
