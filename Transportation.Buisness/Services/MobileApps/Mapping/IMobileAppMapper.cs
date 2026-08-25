using Riok.Mapperly.Abstractions;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.MobileApps.Dtos;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.MobileApps.Mapping
{
    public interface IMobileAppMapper
    {
        MobileApp ToEntity(CreateMobileAppDto request);
        void UpdateEntity(UpdateMobileAppDto request, MobileApp entity);
        MobileAppDetailsDto ToDtoDetails(MobileApp entity);
        SPFOutPutDto<MobileAppListResponseDto> ToDtoList(SPFOutPutDto<MobileApp> entity);
    }

    [Mapper]
    public partial class MobileAppMapper : IMobileAppMapper
    {
        public partial MobileAppDetailsDto ToDtoDetails(MobileApp entity);

        public partial SPFOutPutDto<MobileAppListResponseDto> ToDtoList(SPFOutPutDto<MobileApp> entity);

        [MapperIgnoreSource(nameof(CreateMobileAppDto.File))]
        [MapperIgnoreSource(nameof(CreateMobileAppDto.Icon))]
        public partial MobileApp ToEntity(CreateMobileAppDto request);

        [MapperIgnoreSource(nameof(UpdateMobileAppDto.Id))]
        [MapperIgnoreSource(nameof(UpdateMobileAppDto.Icon))]
        [MapperIgnoreSource(nameof(UpdateMobileAppDto.Icon))]
        public partial void UpdateEntity(UpdateMobileAppDto request, MobileApp entity);
    }
}
