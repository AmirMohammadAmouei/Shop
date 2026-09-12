using Riok.Mapperly.Abstractions;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.MobileAppCategories.Dtos;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.MobileAppCategories.Mapping
{
    public interface IMobileAppCategoryMapper
    {
        MobileAppCategory CreateEntity(CreateMobileAppCategoryDto source);
        SPFOutPutDto<MobileAppCategoryListResponseDto> ToList(SPFOutPutDto<MobileAppCategory> source);
        MobileAppCategoryDetailsDto ToDto(MobileAppCategory source);
        void UpdateEntity(UpdateMobileAppCategoryDto source, MobileAppCategory entity);
    }

    [Mapper]
    public partial class MobileAppCategoryMapper : IMobileAppCategoryMapper
    {
        public partial MobileAppCategoryDetailsDto ToDto(MobileAppCategory source);

        public partial MobileAppCategory CreateEntity(CreateMobileAppCategoryDto source);

        public partial SPFOutPutDto<MobileAppCategoryListResponseDto> ToList(SPFOutPutDto<MobileAppCategory> source);

        [MapperIgnoreSource(nameof(UpdateMobileAppCategoryDto.Id))]
        public partial void UpdateEntity(UpdateMobileAppCategoryDto source, MobileAppCategory entity);
    }
}
