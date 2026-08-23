using Microsoft.EntityFrameworkCore;
using Transportation.Buisness._0.Common;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.ProductCategories.Dto;
using Transportation.Buisness.Services.ProductCategories.Mapping;
using Transportation.Entities._0.Common;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.ProductCategories
{
    public class ProductCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IProductCategoryMapper _mapper;

        public ProductCategoryService(IUnitOfWork unitOfWork,
            IRepository<ProductCategory> productCategoryRepository, IProductCategoryMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<SPFOutPutDto<ProductCategoryListResponseDto>>> List(ProductCategoryListRequestDto request)
        {
            if (request == null)
                return Result<SPFOutPutDto<ProductCategoryListResponseDto>>.Failed("داده های ارسالی نامعتبر است");

            var query = _productCategoryRepository.GetQuery().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchTerm))
                query = query.Where(x => x.Name.Contains(request.SearchTerm));

            var result = await query.OrderByDescending(x => x.CreatedAt).ToPaginatedListAsync(request);

            var map = _mapper.ToDtoList(result);


            if (!map.Items.Any())
                return Result<SPFOutPutDto<ProductCategoryListResponseDto>>.Failed();

            return Result<SPFOutPutDto<ProductCategoryListResponseDto>>.Success(map);
        }

        public async Task<Result<ProductCategoryDetailsDto>> GetDetails(long id)
        {
            var productCategory = await _productCategoryRepository.GetByIdAsync(x => !x.IsDeleted && x.Id == id);

            if (productCategory == null)
                return Result<ProductCategoryDetailsDto>.Failed("دسته بندی با شناسه ارسالی یافت نشد");

            var result = _mapper.ToDtoDetails(productCategory);

            return Result<ProductCategoryDetailsDto>.Success(result);
        }

        public async Task<Result<long>> Create(CreateProductCategoryDto request)
        {
            if (request == null)
                return Result<long>.Failed("داده های ارسالی نامعتبر است");

            if (await _productCategoryRepository.AnyAsync(x => !x.IsDeleted && x.Name == request.Name))
                return Result<long>.Failed("دسته بندی با نام وارد شده تکراری است");

            var entity = _mapper.ToEntity(request);
            await _productCategoryRepository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Result<long>.Success(entity.Id);
        }

        public async Task<Result> Update(UpdateProductCategoryDto request)
        {
            if (request == null)
                return Result.Failed("داده های ارسالی نامعتبر است");

            var productCategory = await _productCategoryRepository.GetQuery()
                .Where(x => !x.IsDeleted && x.Id == request.Id).FirstOrDefaultAsync();

            if (productCategory == null)
                return Result.Failed("دسته بندی با شماسه ارسالی یافت نشد");

            if (await _productCategoryRepository.AnyAsync(x => !x.IsDeleted && x.Id != request.Id && x.Name == request.Name))
                return Result.Failed("دسته بندی با نام وارد شده تکراری است");

            _mapper.UpdateEntity(request, productCategory);

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }


        public async Task<Result> DeleteProductCategory(long id)
        {
            var productCategory = await _productCategoryRepository.GetQuery().Include(x => x.Products)
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (productCategory == null)
                return Result.Failed("دسته با شناسه ارسالی یافت نشد");

            if (productCategory.Products.Any())
                return Result.Failed("دسته بندی مورد نظر در حال استفاده برای کالا است،مجاز به پاک کردن نمی باشید.");


            productCategory.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

    }
}
