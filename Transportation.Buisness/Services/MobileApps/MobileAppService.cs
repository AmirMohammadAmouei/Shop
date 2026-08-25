using Transportation.Buisness._0.Common;
using Transportation.Buisness._0.Common.Constants;
using Transportation.Buisness._0.Common.FileManager;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.MobileApps.Dtos;
using Transportation.Buisness.Services.MobileApps.Mapping;
using Transportation.Entities._0.Common;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.MobileApps
{
    public class MobileAppService
    {
        private readonly IRepository<MobileApp> _mobileAppRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMobileAppMapper _mapper;
        private readonly IUploadFileService _fileService;
        public MobileAppService(IRepository<MobileApp> mobileAppRepository,
            IUnitOfWork unitOfWork, IMobileAppMapper mapper, IUploadFileService fileService)
        {
            _mobileAppRepository = mobileAppRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }


        public async Task<Result<SPFOutPutDto<MobileAppListResponseDto>>> List(MobileAppListRequestDto request)
        {
            if (request == null)
                return Result<SPFOutPutDto<MobileAppListResponseDto>>.Failed("داده های ارسالی نامعتبر است");

            var query = _mobileAppRepository.GetQuery().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchTerm))
                query = query.Where(x => x.Title.Contains(request.SearchTerm));

            var result = await query.OrderByDescending(x => x.CreatedAt)
                .ToPaginatedListAsync(request ?? new MobileAppListRequestDto());

            if (!result.Items.Any())
                return Result<SPFOutPutDto<MobileAppListResponseDto>>.Failed();

            var mapping = _mapper.ToDtoList(result);

            return Result<SPFOutPutDto<MobileAppListResponseDto>>.Success(mapping);
        }


        public async Task<Result<MobileAppDetailsDto>> GetDetails(long id)
        {
            if (id == 0)
                return Result<MobileAppDetailsDto>.Failed("شناسه ارسالی نامعتبر است");

            var mobile = await _mobileAppRepository.GetByIdAsync(x => !x.IsDeleted && x.Id == id);

            if (mobile == null)
                return Result<MobileAppDetailsDto>.Failed("اپ موبایل با شناسه ارسالی یافت نشد");

            var result = _mapper.ToDtoDetails(mobile);

            return Result<MobileAppDetailsDto>.Success(result);
        }


        public async Task<Result<long>> Create(CreateMobileAppDto request)
        {
            if (request == null)
                return Result<long>.Failed("داده های ارسالی نامعتبر است");

            if (await _mobileAppRepository.AnyAsync(x => !x.IsDeleted && x.Title == request.Title && x.Version == request.Version))
                return Result<long>.Failed("برنامه ایی با نام و ورژن وارد شده قبلا ثبت شده است");

            var mobileApp = _mapper.ToEntity(request);

            if (request.Icon != null && request.Icon.Length > 0)
            {
                var iconPath = await _fileService.UploadAsync(request.Icon, UploadFilesPath.Apps);

                if (!iconPath.IsSucceeded)
                    return Result<long>.Failed("خطا در بارگزاری آیکون اپ");

                mobileApp.IconPath = iconPath.Path;
            }

            if (request.File != null && request.File.Length > 0)
            {
                var appPath = await _fileService.UploadAsync(request.File, UploadFilesPath.Apps);

                if (!appPath.IsSucceeded)
                    return Result<long>.Failed("خطا در بارگزاری اپ");

                mobileApp.FilePath = appPath.Path;
                mobileApp.FileSize = appPath.FileSize;
            }

            await _mobileAppRepository.InsertAsync(mobileApp);
            await _unitOfWork.CommitAsync();

            return Result<long>.Success(mobileApp.Id);
        }

        public async Task<Result> UpdateMobileApp(UpdateMobileAppDto request)
        {
            if (request == null)
                return Result.Failed("داده های ارسالی نامعتبر است");

            var mobile = await _mobileAppRepository.GetByIdAsync(x => !x.IsDeleted && x.Id == request.Id);

            if (mobile == null)
                return Result.Failed("برنامه ایی با شناسه ارسالی یافت نشد یافت نشد");


            if (request.Icon != null && request.Icon.Length > 0)
            {
                var iconPath = await _fileService.UploadAsync(request.Icon, UploadFilesPath.Apps);

                if (!iconPath.IsSucceeded)
                    return Result<long>.Failed("خطا در بارگزاری آیکون اپ");

                mobile.IconPath = iconPath.Path;
            }


            if (request.File != null && request.File.Length > 0)
            {
                var appPath = await _fileService.UploadAsync(request.File, UploadFilesPath.Apps);

                if (appPath.IsSucceeded)
                    return Result<long>.Failed("خطا در بارگزاری اپ");

                mobile.FilePath = appPath.Path;
                mobile.FileSize = appPath.FileSize;
            }

            _mapper.UpdateEntity(request, mobile);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

    }
}
