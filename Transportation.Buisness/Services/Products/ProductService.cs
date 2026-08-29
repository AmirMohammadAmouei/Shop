using Microsoft.EntityFrameworkCore;
using Transportation.Buisness._0.Common;
using Transportation.Buisness._0.Common.Constants;
using Transportation.Buisness._0.Common.FileManager;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.ProductCategories.Dto;
using Transportation.Buisness.Services.Products.Dto;
using Transportation.Buisness.Services.Products.Mapping;
using Transportation.Entities._0.Common;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.Products
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product> _productRepository;
        private readonly IProductMapper _mapper;
        private readonly IFileService _uploadFileService;

        public ProductService(
            IUnitOfWork unitOfWork,
            IRepository<Product> productRepository,
            IProductMapper mapper,
            IFileService uploadFileService)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _mapper = mapper;
            _uploadFileService = uploadFileService;
        }

        public async Task<Result<SPFOutPutDto<ProductListResponseDto>>> List(ProductListRequestDto request)
        {
            var query = _productRepository.GetQuery()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(request?.SearchTerm))
                query = query.Where(x => x.Name.Contains(request.SearchTerm));

            var result = await query.Include(x => x.ProductCategory).Include(x => x.ProductImages)
                .OrderByDescending(x => x.CreatedAt)
                .ToPaginatedListAsync(request ?? new ProductListRequestDto());

            var mapping = _mapper.ToDtoList(result);
            return Result<SPFOutPutDto<ProductListResponseDto>>.Success(mapping);
        }

        public async Task<Result<ProductDetailsDto>> GetProductBy(long id)
        {
            if (id == 0)
                return Result<ProductDetailsDto>.Failed("شناسه ارسالی نامعتبر است");

            var product = await _productRepository.GetQuery().Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.ProductCategory).Include(x => x.ProductImages).FirstOrDefaultAsync();

            if (product == null)
                return Result<ProductDetailsDto>.Failed("کالایی با شناسه ارسالی یافت نشد");

            var result = _mapper.ToDetialsDto(product);

            if (result == null)
                return Result<ProductDetailsDto>.Failed("خطا در دریافت اطلاعات کالا");

            return Result<ProductDetailsDto>.Success(result);
        }

        public async Task<Result<SPFOutPutDto<ProductListResponseDto>>> GetProductsByCategoryId(long categoryId, ProductListRequestDto request)
        {
            var products = _productRepository.GetQuery().Include(x => x.ProductCategory).Include(x => x.ProductImages)
                .Where(x => !x.IsDeleted && x.ProductCategoryId == categoryId).OrderByDescending(x => x.CreatedAt);

            var result = await products.ToPaginatedListAsync(request);

            var mapping = _mapper.ToDtoList(result);

            if (!mapping.Items.Any())
                return Result<SPFOutPutDto<ProductListResponseDto>>.Failed();

            return Result<SPFOutPutDto<ProductListResponseDto>>.Success(mapping);
        }



        public async Task<Result<long>> Create(CreateProductDto request)
        {
            if (request == null)
                return Result<long>.Failed("داده های ارسالی نامعتبر است");

            var entity = _mapper.ToEntity(request);

            foreach (var img in request.Images)
            {
                var upload = await _uploadFileService.UploadAsync(img, UploadFilesPath.Products);
                entity.ProductImages.Add(new ProductImages { Path = upload.Path });
            }

            await _productRepository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Result<long>.Success(entity.Id);
        }

        public async Task<Result> Update(UpdateProductDto request)
        {
            if (request == null)
                return Result.Failed("داده های ارسالی نامعتبر است");

            var product = await _productRepository.GetQuery()
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == request.Id);

            if (product == null)
                return Result.Failed("محصول یافت نشد");

            _mapper.UpdateEntity(request, product);

            // حذف عکس‌های انتخاب شده
            if (request.DeletedImageIds?.Any() == true)
            {
                var toDelete = product.ProductImages
                    .Where(i => request.DeletedImageIds.Contains(i.Id))
                    .ToList();

                _uploadFileService.DeleteMany(toDelete.Select(i => i.Path).ToList());

                foreach (var img in toDelete)
                    product.ProductImages.Remove(img);
            }

            // اضافه کردن عکس‌های جدید
            foreach (var img in request.NewImages)
            {
                var upload = await _uploadFileService.UploadAsync(img, UploadFilesPath.Products);
                product.ProductImages.Add(new ProductImages { Path = upload.Path });
            }

            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result> Delete(long id)
        {
            var product = await _productRepository.GetQuery()
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);

            if (product == null)
                return Result.Failed("محصول یافت نشد");

            _uploadFileService.DeleteMany(product.ProductImages
                .Select(i => i.Path)
                .ToList());

            product.IsDeleted = true;
            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result<string>> GetImagePath(long imageId)
        {
            var product = await _productRepository.GetQuery()
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => !x.IsDeleted &&
                    x.ProductImages.Any(i => i.Id == imageId));

            if (product == null)
                return Result<string>.Failed("تصویر یافت نشد");

            var image = product.ProductImages.First(i => i.Id == imageId);

            return Result<string>.Success(image.Path);
        }
    }
}
