using Grpc.Core;
using Mapster;
using MapsterMapper;
using Meteor.Controller.Api.Grpc;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Services.Contracts;

namespace Meteor.Controller.Api.Services;

public class CustomersGrpcService : CustomersService.CustomersServiceBase
{
    private readonly ICustomersService _customersService;

    private readonly IMapper _mapper;

    public CustomersGrpcService(ICustomersService customersService, IMapper mapper)
    {
        _customersService = customersService;
        _mapper = mapper;
    }

    public override async Task<Customer> GetCustomer(
        GetCustomerRequest request,
        ServerCallContext context
    )
    {
        var customer = await _customersService.GetCustomerAsync(request.Domain);
        return _mapper.Map<Customer>(customer);
    }

    public override async Task<CustomersPage> GetCustomers(
        GetCustomersPageRequest request,
        ServerCallContext context
    )
    {
        var filter = _mapper.Map<CustomersFilter>(request);
        var customersPage = await _customersService.GetCustomersAsync(filter);
        return new()
        {
            Total = customersPage.Total,
            Customers =
            {
                _mapper.Map<List<Customer>>(customersPage.Items)
            }
        };
    }

    public override async Task<CustomerSettings> GetCustomerSettings(
        GetCustomerByIdRequest request,
        ServerCallContext context
    )
    {
        var settings = await _customersService.GetCustomerSettings(request.CustomerId);
        var settingsResponse = _mapper.Map<CustomerSettings>(settings);
        settingsResponse.CustomerId = request.CustomerId;
        return settingsResponse;
    }

    public override async Task<CustomerSettings> SetCustomerSettings(
        SetCustomerSettingsRequest request,
        ServerCallContext context
    )
    {
        var dto = _mapper.Map<SetCustomerSettingsDto>(request);
        var settings = await _customersService.SetCustomerSettings(request.CustomerId, dto);
        var settingsResponse = _mapper.Map<CustomerSettings>(settings);
        settingsResponse.CustomerId = request.CustomerId;
        return settingsResponse;
    }
}