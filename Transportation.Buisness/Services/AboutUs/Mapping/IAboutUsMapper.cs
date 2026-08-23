using Riok.Mapperly.Abstractions;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.AboutUs.Dtos;


namespace Transportation.Buisness.Services.AboutUs.Mapping
{
    public interface IAboutUsMapper
    {
        Transportation.Entities.Entities.AboutUs ToEntity(CreateAboutUsDto source);
        AboutUsResponseDto ToDtoList(Transportation.Entities.Entities.AboutUs source);
        void UpdateEntity(UpdateAboutUsDto source, Transportation.Entities.Entities.AboutUs target);
    }

    [Mapper]
    public partial class AboutUsMapper : IAboutUsMapper
    {
        private partial AboutUsResponseDto ToDto(Transportation.Entities.Entities.AboutUs source);
        public partial Transportation.Entities.Entities.AboutUs ToEntity(CreateAboutUsDto source);
        [MapperIgnoreSource(nameof(UpdateAboutUsDto.Id))]
        [MapperIgnoreSource(nameof(UpdateAboutUsDto.Logo))]
        public partial void UpdateEntity(UpdateAboutUsDto source, Transportation.Entities.Entities.AboutUs target);
        public AboutUsResponseDto ToDtoList(Transportation.Entities.Entities.AboutUs source) => ToDto(source);
    }
}