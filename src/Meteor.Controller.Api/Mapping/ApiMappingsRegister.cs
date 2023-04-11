using Google.Protobuf.WellKnownTypes;
using Mapster;
using Meteor.Controller.Api.Grpc;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Models.Enums;

namespace Meteor.Controller.Api.Mapping;

public class ApiMappingsRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Core.Models.Customer, Customer>()
            .Map(c => c.Created, c => Timestamp.FromDateTimeOffset(c.Created));

        config.ForType<SetCustomerSettingsRequest, SetCustomerSettingsDto>()
            .IgnoreIf(
                (settings, dto) => settings.HasCoreConnectionString,
                dto => dto.CoreConnectionString!
            )
            .Map(dto => dto.CoreConnectionString, req => req.CoreConnectionString);

        config.ForType<Core.Models.CustomerSettings, CustomerSettings>()
            .Map(cs => cs.CoreConnectionString, cs => cs.CoreDatabaseConnectionString);

        config.ForType<GetCustomersPageRequest, CustomersFilter>()
            .Map(filter => filter.Paging.Limit, req => req.Limit)
            .Map(filter => filter.Paging.Offset, req => req.Offset)
            .Map(filter => filter.Statuses, req => req.Statuses.Adapt<List<CustomerStatuses>>());
    }
}