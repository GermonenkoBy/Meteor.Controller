using Mapster;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Models;

namespace Meteor.Controller.Core.Mapping;

public class CoreMappingsRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<SetCustomerSettingsDto, CustomerSettings>()
            .Map(
                cs => cs.CoreDatabaseConnectionString,
                dto => dto.CoreConnectionString,
                dto => dto.CoreConnectionString != null
            );
    }
}