using Google.Protobuf.WellKnownTypes;
using Mapster;
using MapsterMapper;
using Meteor.Controller.Api.Grpc;
using Meteor.Controller.Api.Mapping;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Models.Enums;

namespace Meteor.Controller.Api.Tests;

[TestClass]
public class ApiMappingsTests
{
    private readonly IMapper _mapper;

    public ApiMappingsTests()
    {
        var config = new TypeAdapterConfig();
        config.Apply(new ApiMappingsRegister());
        _mapper = new Mapper(config);
    }

    [TestMethod]
    public void MapCustomerToGrpcCustomer_Should_MapFieldsCorrectly()
    {
        var customer = new Core.Models.Customer
        {
            Id = 1,
            Name = "Test",
            Domain = "test.com",
            Created = DateTimeOffset.UtcNow,
            Status = CustomerStatuses.Active,
        };

        var createdTimestamp = Timestamp.FromDateTimeOffset(customer.Created);

        var grpcCustomer = _mapper.Map<Customer>(customer);

        Assert.AreEqual(customer.Id, grpcCustomer.Id);
        Assert.AreEqual(customer.Name, grpcCustomer.Name);
        Assert.AreEqual(customer.Domain, grpcCustomer.Domain);
        Assert.AreEqual(CUSTOMER_STATUS.Active, grpcCustomer.Status);
        Assert.AreEqual(createdTimestamp, grpcCustomer.Created);
    }

    [TestMethod]
    public void MapGetCustomersPageRequestToCustomersFilter_Should_MapAllFields()
    {
        var customersRequest = new GetCustomersPageRequest
        {
            Offset = 25,
            Limit = 100,
            Statuses =
            {
                CUSTOMER_STATUS.Active,
                CUSTOMER_STATUS.Suspended,
            }
        };

        var filter = _mapper.Map<CustomersFilter>(customersRequest);

        Assert.AreEqual(customersRequest.Limit, filter.Paging.Limit);
        Assert.AreEqual(customersRequest.Offset, filter.Paging.Offset);
        Assert.AreEqual(2, filter.Statuses.Count);
        Assert.IsTrue(filter.Statuses.Any(s => s == CustomerStatuses.Active));
        Assert.IsTrue(filter.Statuses.Any(s => s == CustomerStatuses.Suspended));
    }
}