using Riok.Mapperly.Abstractions;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.CompanyFeatures.Dtos;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.CompanyFeatures.Mapping
{
    public interface ICompanyFeatureMapper
    {
        SPFOutPutDto<CompanyFeatureListResponseDto> ToList(SPFOutPutDto<CompanyFeature> entity);
        CompanyFeature CreateEntity(CreateCompanyFeatureDto source);
        CompanyFeatureDetailsDto ToDto(CompanyFeature source);
        void UpdateEntity(UpdateCompanyFeatureDto source, CompanyFeature entity);
    }

    [Mapper]
    public partial class CompanyFeatureMapper : ICompanyFeatureMapper
    {
        public partial SPFOutPutDto<CompanyFeatureListResponseDto> ToList(SPFOutPutDto<CompanyFeature> entity);
        public partial CompanyFeature CreateEntity(CreateCompanyFeatureDto source);

        public partial CompanyFeatureDetailsDto ToDto(CompanyFeature source);


        [MapperIgnoreSource(nameof(UpdateCompanyFeatureDto.Id))]
        public partial void UpdateEntity(UpdateCompanyFeatureDto source, CompanyFeature entity);
    }
}
