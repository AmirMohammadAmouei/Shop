using Riok.Mapperly.Abstractions;
using Transportation.Buisness._0.Common.Paging;
using Transportation.Buisness.Services.Customers.Dtos;
using Transportation.Entities.Entities;

namespace Transportation.Buisness.Services.Customers.Mappings
{
    public interface ICustomerMapper
    {
        Customer ToEntity(CreateCustomerDto request);
        SPFOutPutDto<CustomerListResponseDto> ToList(SPFOutPutDto<Customer> entity);
        CustomerDetailsDto ToDetails(Customer entity);
        void UpdateEntity(UpdateCustomerDto request , Customer entity);
    }

    [Mapper]
    public partial class CustomerMapper : ICustomerMapper
    {
        public partial CustomerDetailsDto ToDetails(Customer entity);

        public partial Customer ToEntity(CreateCustomerDto request);

        public partial SPFOutPutDto<CustomerListResponseDto> ToList(SPFOutPutDto<Customer> entity);

        [MapperIgnoreSource(nameof(UpdateCustomerDto.Id))]
        public partial void UpdateEntity(UpdateCustomerDto request, Customer entity);
    }
}
