    using Riok.Mapperly.Abstractions;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.ProductCategories.Dto;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.ProductCategories.Mapping
{
    public interface IProductCategoryMapper
    {
        ProductCategory ToEntity(CreateProductCategoryDto request);
        SPFOutPutDto<ProductCategoryListResponseDto> ToDtoList(SPFOutPutDto<ProductCategory> source);
        ProductCategoryDetailsDto ToDtoDetails(ProductCategory entity);
        void UpdateEntity(UpdateProductCategoryDto request, ProductCategory entity);
    }

    [Mapper]
    public partial class ProductCategoryMapper : IProductCategoryMapper
    {
        public partial ProductCategoryDetailsDto ToDtoDetails(ProductCategory entity);
        public partial SPFOutPutDto<ProductCategoryListResponseDto> ToDtoList(SPFOutPutDto<ProductCategory> source);
        public partial ProductCategory ToEntity(CreateProductCategoryDto request);
        
        [MapperIgnoreSource(nameof(UpdateProductCategoryDto.Id))]
        public partial void UpdateEntity(UpdateProductCategoryDto request, ProductCategory entity);
    }

}
