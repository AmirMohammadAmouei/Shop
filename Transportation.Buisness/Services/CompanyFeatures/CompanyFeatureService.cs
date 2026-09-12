using Transportation.Buisness._0.Common;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.CompanyFeatures.Dtos;
using Transportation.Buisness.Services.CompanyFeatures.Mapping;
using Transportation.Entities._0.Common;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.CompanyFeatures
{
    public class CompanyFeatureService
    {
        private readonly ICompanyFeatureMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CompanyFeature> _companyFeatureRepository;

        public CompanyFeatureService(ICompanyFeatureMapper mapper, IUnitOfWork unitOfWork, IRepository<CompanyFeature> companyFeatureRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _companyFeatureRepository = companyFeatureRepository;
        }

        public async Task<Result<SPFOutPutDto<CompanyFeatureListResponseDto>>> List(CompanyFeatureListRequestDto request)
        {
            if (request == null)
                request = new CompanyFeatureListRequestDto();

            var query = _companyFeatureRepository.GetQuery().Where(x => !x.IsDeleted);


            var entity = await query.OrderByDescending(x => x.CreatedAt).ToPaginatedListAsync(request);

            if (!entity.Items.Any())
                return Result<SPFOutPutDto<CompanyFeatureListResponseDto>>.Failed();

            var result = _mapper.ToList(entity);

            return Result<SPFOutPutDto<CompanyFeatureListResponseDto>>.Success(result);
        }

        public async Task<Result<CompanyFeatureDetailsDto>> GetDetails(long id)
        {
            var feature = await _companyFeatureRepository.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

            if (feature == null)
                return Result<CompanyFeatureDetailsDto>.Failed("خطا در بازیابی داده");

            var result = _mapper.ToDto(feature);

            return Result<CompanyFeatureDetailsDto>.Success(result);
        }


        public async Task<Result> Create(CreateCompanyFeatureDto request)
        {
            var entity = _mapper.CreateEntity(request);
            await _companyFeatureRepository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
        public async Task<Result> Update(UpdateCompanyFeatureDto request)
        {
            if (request == null)
                return Result.Failed("داده های ارسالی نامعتبر است");

            var feature = await _companyFeatureRepository.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == request.Id);

            if (feature == null)
                return Result.Failed("خطا در یافتن داده");

            _mapper.UpdateEntity(request, feature);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result> Delete(long id)
        {
            var feature = await _companyFeatureRepository.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

            if (feature == null)
                return Result.Failed("خطا در یافتن داده");

            feature.IsDeleted = true;
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }
}
