using Transportation.Buisness._0.Common;
using Transportation.Buisness._0.Common.Constants;
using Transportation.Buisness._0.Common.FileManager;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.Customers.Dtos;
using Transportation.Buisness.Services.Customers.Mappings;
using Transportation.Entities._0.Common;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.Customers
{
    public class CustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ICustomerMapper _mapper;
        private readonly IFileService _fileService;

        public CustomerService(IUnitOfWork unitOfWork, IRepository<Customer> customerRepository, ICustomerMapper mapper, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<Result<SPFOutPutDto<CustomerListResponseDto>>> List(CustomerListRequestDto request)
        {
            if (request == null)
                return Result<SPFOutPutDto<CustomerListResponseDto>>.Failed("داده ارسالی نامعتبر است");

            var query = _customerRepository.GetQuery().Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchTerm))
                query = query.Where(x => x.Name.Contains(request.SearchTerm));

            var result = await query.OrderByDescending(x => x.CreatedAt).ToPaginatedListAsync(request ?? new CustomerListRequestDto());

            if (!result.Items.Any())
                return Result<SPFOutPutDto<CustomerListResponseDto>>.Failed("داده ایی از سرور یافت نشد");

            var mapping = _mapper.ToList(result);

            return Result<SPFOutPutDto<CustomerListResponseDto>>.Success(mapping);
        }


        public async Task<Result<CustomerDetailsDto>> GetDetails(long id)
        {
            if (id == 0)
                return Result<CustomerDetailsDto>.Failed("شناسه اارسالی نامعتبر است");

            var customer = await _customerRepository.GetByIdAsync(x => !x.IsDeleted && x.Id == id);

            if (customer == null)
                return Result<CustomerDetailsDto>.Failed("خطا در پیدا شدن مشتری");

            var result = _mapper.ToDetails(customer);

            return Result<CustomerDetailsDto>.Success(result);
        }

        public async Task<Result<long>> Create(CreateCustomerDto request)
        {
            if (request == null)
                return Result<long>.Failed("داده های ارسالی نامعتبر است");

            var customer = await _customerRepository.GetByIdAsync(x => !x.IsDeleted && x.Name.Contains(request.Name));

            if (customer != null)
                return Result<long>.Failed("نام مشتری وارده شده تکراری است");

            var entity = _mapper.ToEntity(request);

            if (request.LogoPath != null)
            {
                var logoPath = await _fileService.UploadAsync(request.LogoPath, UploadFilesPath.Customers);

                if (!logoPath.IsSucceeded)
                    return Result<long>.Failed(logoPath.Message);

                entity.LogoPath = logoPath.Path;
            }

            await _customerRepository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return Result<long>.Success(entity.Id);
        }


        public async Task<Result> Update(UpdateCustomerDto request)
        {
            if (request == null)
                return Result.Failed("داده های ارسالی نامعتبر است");

            var customer = await _customerRepository.GetByIdAsync(x => !x.IsDeleted && x.Id == request.Id);

            if (customer == null)
                return Result.Failed("مشتری با شناسه ارسالی یافت نشد");

            _mapper.UpdateEntity(request, customer);

            if (request.LogoPath != null)
            {
                var logoPath = await _fileService.UploadAsync(request.LogoPath, UploadFilesPath.Customers);

                if (!logoPath.IsSucceeded)
                    return Result<long>.Failed(logoPath.Message);

                customer.LogoPath = logoPath.Path;
            }

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }

        public async Task<Result> Delete(long id)
        {
            if (id == 0)
                return Result.Failed("شناسه ارسالی نامعتبر است");

            var customer = await _customerRepository.GetByIdAsync(x => !x.IsDeleted && x.Id == id);

            if (customer == null)
                return Result.Failed("مشتری با شناسه ارسالی یافت نشد");

            customer.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }
}
