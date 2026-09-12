using Microsoft.EntityFrameworkCore;
using Transportation.Buisness._0.Common;
using Transportation.Buisness._0.Common.Constants;
using Transportation.Buisness._0.Common.FileManager;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.AboutUs.Dtos;
using Transportation.Buisness.Services.AboutUs.Mapping;
using Transportation.Entities._0.Common;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.AboutUs
{
    public class AboutUsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Entities.Entities.AboutUs> _aboutUsRepository;
        private readonly IFileService _fileService;
        private readonly IAboutUsMapper _mapper;
        private readonly IRepository<AboutUsGallery> _aboutUsGalleryRepository;

        public AboutUsService(IUnitOfWork unitOfWork, IRepository<Entities.Entities.AboutUs> aboutUsRepository,
            IFileService uploadFileService, IAboutUsMapper mapper, IRepository<AboutUsGallery> aboutUsGalleryRepository)
        {
            _unitOfWork = unitOfWork;
            _aboutUsRepository = aboutUsRepository;
            _fileService = uploadFileService;
            _mapper = mapper;
            _aboutUsGalleryRepository = aboutUsGalleryRepository;
        }

        public async Task<Result<AboutUsResponseDto>> GetDetails()
        {
            var aboutUs = await _aboutUsRepository.GetQuery()
                .Where(x => !x.IsDeleted)
                .Include(x=>x.Galleries)
                .FirstOrDefaultAsync();

            if (aboutUs == null)
                return Result<AboutUsResponseDto>.Failed("اطلاعات شرکت یافت نشد");

            var result = _mapper.ToDto(aboutUs);

            return Result<AboutUsResponseDto>.Success(result);
        }

        public async Task<Result> Create(CreateAboutUsDto request)
        {


            if (await _aboutUsRepository.GetQuery().AnyAsync(x => !x.IsDeleted))
                return Result.Failed("اطلاعات شرکت قبلاً ثبت شده است");

            var entity = _mapper.ToEntity(request);

            if (request.Logo != null)
            {
                var upload = await _fileService.UploadAsync(request.Logo, UploadFilesPath.AboutUs);
                if (!upload.IsSucceeded)
                    return Result.Failed(upload.Message);
                entity.LogoPath = upload.Path;
            }

            if (request.Galleries != null && request.Galleries.Any())
            {
                foreach (var gallery in request.Galleries)
                {
                    var upload = await _fileService.UploadAsync(gallery, UploadFilesPath.AboutUs);

                    if (!upload.IsSucceeded)
                        return Result.Failed(upload.Message);

                    entity.Galleries.Add(new AboutUsGallery
                    {
                        AboutUsId = entity.Id,
                        FileName = upload.FileName,
                        Path = upload.Path,
                        OriginalFileName = upload.OriginalFileName
                    });
                }
            }

            await _aboutUsRepository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result> Update(UpdateAboutUsDto request)
        {
            var entity = await _aboutUsRepository.GetQuery()
                .Where(x => !x.IsDeleted && x.Id == request.Id)
                .Include(x=>x.Galleries)
                .FirstOrDefaultAsync();

            if (entity == null)
                return Result<AboutUsResponseDto>.Failed("اطلاعات شرکت یافت نشد");

            _mapper.UpdateEntity(request, entity);

            if (request.Logo != null)
            {
                var upload = await _fileService.UploadAsync(request.Logo, UploadFilesPath.AboutUs);
                if (!upload.IsSucceeded)
                    return Result.Failed(upload.Message);
                entity.LogoPath = upload.Path;
            }

            // حذف تصاویر
            if (request.DeletedGalleryIds?.Any() == true)
            {
                var toDelete = entity.Galleries
                    .Where(x => request.DeletedGalleryIds.Contains(x.Id))
                    .ToList();
                foreach (var img in toDelete)
                    entity.Galleries.Remove(img);
            }

            // آپلود تصاویر جدید
            if (request.NewImages?.Any() == true)
            {
                foreach (var file in request.NewImages)
                {
                    var upload = await _fileService.UploadAsync(file, UploadFilesPath.AboutUs);
                    if (!upload.IsSucceeded) continue;
                    entity.Galleries.Add(new AboutUsGallery
                    {
                        Path = upload.Path,
                        FileName = upload.FileName,
                        OriginalFileName = upload.OriginalFileName,
                        AboutUsId = entity.Id
                    });
                }
            }

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result<string>> GetGalleryImagePath(long id)
        {
            var image = await _aboutUsGalleryRepository.GetQuery()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (image == null)
                return Result<string>.Failed("تصویر یافت نشد");

            return Result<string>.Success(image.Path);
        }
    }
}
