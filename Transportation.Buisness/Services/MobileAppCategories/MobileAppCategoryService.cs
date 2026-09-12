using Transportation.Buisness._0.Common;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.MobileAppCategories.Dtos;
using Transportation.Buisness.Services.MobileAppCategories.Mapping;
using Transportation.Entities._0.Common;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.MobileAppCategories
{
    public class MobileAppCategoryService
    {
        private readonly IMobileAppCategoryMapper _mapping;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MobileAppCategory> _mobileAppCategoryRepository;

        public MobileAppCategoryService(IUnitOfWork unitOfWork, IRepository<MobileAppCategory> mobileAppCategoryRepository, IMobileAppCategoryMapper mapping)
        {
            _unitOfWork = unitOfWork;
            _mobileAppCategoryRepository = mobileAppCategoryRepository;
            _mapping = mapping;
        }

        public async Task<Result<SPFOutPutDto<MobileAppCategoryListResponseDto>>> List(MobileAppCategoryListRequestDto request)
        {
            if (request == null)
                request = new MobileAppCategoryListRequestDto();

            var query = _mobileAppCategoryRepository.GetQuery().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchTerm))
                query = query.Where(x => x.Name.Contains(request.SearchTerm));

            var entity = await query.OrderByDescending(x => x.CreatedAt).ToPaginatedListAsync(request);

            if (!entity.Items.Any())
                return Result<SPFOutPutDto<MobileAppCategoryListResponseDto>>.Failed();

            var result = _mapping.ToList(entity);


            return Result<SPFOutPutDto<MobileAppCategoryListResponseDto>>.Success(result);
        }

        public async Task<Result<MobileAppCategoryDetailsDto>> GetDetails(long id)
        {
            if (id == 0)
                return Result<MobileAppCategoryDetailsDto>.Failed("شناسه  دسته بندی نامعتبر است");

            var category = await _mobileAppCategoryRepository.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

            if (category == null)
                return Result<MobileAppCategoryDetailsDto>.Failed("دسته بندی مورد نظر یافت نشد");

            var result = _mapping.ToDto(category);

            return Result<MobileAppCategoryDetailsDto>.Success(result);
        }

        public async Task<Result> Create(CreateMobileAppCategoryDto request)
        {
            if (request == null)
                return Result.Failed("داده ارسالی نامعتبر است");

            if (await _mobileAppCategoryRepository.AnyAsync(x => !x.IsDeleted && x.Name.Contains(request.Name)))
                return Result.Failed("نام دسته بندی تکراری است");

            var result = _mapping.CreateEntity(request);

            await _mobileAppCategoryRepository.InsertAsync(result);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result> Update(UpdateMobileAppCategoryDto request)
        {
            if (request == null)
                return Result.Failed("داده ارسالی نامعتبر است");

            var category = await _mobileAppCategoryRepository.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == request.Id);

            if (category == null)
                return Result.Failed("دسته بندی موردنظر یافت نشد");

            _mapping.UpdateEntity(request, category);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result> Delete(long id)
        {
            if (id == 0)
                return Result.Failed("داده ارسالی نامعتبر است");

            var category = await _mobileAppCategoryRepository.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

            if (category == null)
                return Result.Failed("دسته بندی مورد نظر یافت نشد");

            category.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }
}
