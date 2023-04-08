using Mapster;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Models;

namespace Meteor.Controller.Core.Mapping;

public class CoreMappingsRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<SetCustomerSettingsDto, CustomerSettings>()
            .IgnoreIf(
                (dto, settings) => dto.CoreConnectionString == null, 
                settings => settings.CoreDatabaseConnectionString
            )
            .Map(cs => cs.CoreDatabaseConnectionString, dto => dto.CoreConnectionString);
    }
}