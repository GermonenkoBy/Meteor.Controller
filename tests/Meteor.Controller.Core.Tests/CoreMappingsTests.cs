using Mapster;
using MapsterMapper;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Mapping;
using Meteor.Controller.Core.Models;

namespace Meteor.Controller.Core.Tests;

[TestClass]
public class CoreMappingsTests
{
    private readonly IMapper _mapper;

    public CoreMappingsTests()
    {
        var config = new TypeAdapterConfig();
        config.Apply(new CoreMappingsRegister());
        _mapper = new Mapper(config);
    }

    [TestMethod]
    public void MapCustomerSettingsDtoToCustomerSettings_Should_MapFields()
    {
        var dto = new SetCustomerSettingsDto
        {
            CoreConnectionString = "Test2"
        };

        var settings = new CustomerSettings
        {
            CoreDatabaseConnectionString = "Test",
        };

        _mapper.Map(dto, settings);
        
        Assert.AreEqual(dto.CoreConnectionString, settings.CoreDatabaseConnectionString);
    }
    
    [TestMethod]
    public void MapCustomerSettingsDtoToCustomerSettings_Should_IgnoreNullFields()
    {
        var dto = new SetCustomerSettingsDto
        {
            CoreConnectionString = null
        };

        var settings = new CustomerSettings
        {
            CoreDatabaseConnectionString = "Test",
        };

        _mapper.Map(dto, settings);

        Assert.AreEqual("Test", settings.CoreDatabaseConnectionString);
    }
}