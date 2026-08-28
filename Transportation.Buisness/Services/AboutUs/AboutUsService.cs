using Microsoft.EntityFrameworkCore;
using Transportation.Buisness._0.Common;
using Transportation.Buisness._0.Common.Constants;
using Transportation.Buisness._0.Common.FileManager;
using Transportation.Buisness.Services.AboutUs.Dtos;
using Transportation.Buisness.Services.AboutUs.Mapping;
using Transportation.Entities._0.Common;

namespace Transportation.Buisness.Services.AboutUs
{
    public class AboutUsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Transportation.Entities.Entities.AboutUs> _aboutUsRepository;
        private readonly IFileService _uploadFileService;
        private readonly IAboutUsMapper _mapper;
        public AboutUsService(IUnitOfWork unitOfWork, IRepository<Transportation.Entities.Entities.AboutUs> aboutUsRepository, IFileService uploadFileService, IAboutUsMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _aboutUsRepository = aboutUsRepository;
            _uploadFileService = uploadFileService;
            _mapper = mapper;
        }

        public async Task<Result<AboutUsResponseDto>> GetDetails()
        {
            var aboutUs = await _aboutUsRepository.GetQuery()
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (aboutUs == null)
                return Result<AboutUsResponseDto>.Failed("اطلاعات شرکت یافت نشد");

            var result = _mapper.ToDtoList(aboutUs);

            return Result<AboutUsResponseDto>.Success(result);
        }

        public async Task<Result> Create(CreateAboutUsDto request)
        {
            var exists = await _aboutUsRepository.GetQuery()
                .AnyAsync(x => !x.IsDeleted);

            if (exists)
                return Result.Failed("اطلاعات شرکت قبلاً ثبت شده است");

            var entity = _mapper.ToEntity(request);

            if (request.Logo != null)
            {
                var upload = await _uploadFileService.UploadAsync(request.Logo , UploadFilesPath.AboutUs);
                if (!upload.IsSucceeded)
                    return Result.Failed(upload.Message);
                entity.LogoPath = upload.Path;
            }

            await _aboutUsRepository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result> Update(UpdateAboutUsDto request)
        {
            var entity = await _aboutUsRepository.GetQuery()
                .Where(x => !x.IsDeleted && x.Id == request.Id)
                .FirstOrDefaultAsync();

            if (entity == null)
                return Result<AboutUsResponseDto>.Failed("اطلاعات شرکت یافت نشد");

            _mapper.UpdateEntity(request, entity);

            if (request.Logo != null)
            {
                var upload = await _uploadFileService.UploadAsync(request.Logo , UploadFilesPath.AboutUs);
                if (!upload.IsSucceeded)
                    return Result.Failed(upload.Message);
                entity.LogoPath = upload.Path;
            }

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }
}
