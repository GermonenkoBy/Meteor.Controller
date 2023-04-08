using System.Security.Cryptography;
using Mapster;
using MapsterMapper;
using Meteor.Common.Core.Exceptions;
using Meteor.Common.Cryptography.Abstractions;
using Meteor.Controller.Core.Constants;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Mapping;
using Meteor.Controller.Core.Models;
using Meteor.Controller.Core.Models.Enums;
using Meteor.Controller.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Moq;

namespace Meteor.Controller.Core.Tests;

[TestClass]
public class CustomersServiceTests
{
    private const int TestCustomerId = 1;

    private static readonly CustomersService CustomersService;

    private static readonly ControllerContext ControllerContext;

    private static readonly Mock<IEncryptor> EncryptorMock;

    private static readonly Mock<IFeatureManager> FeatureManagerMock;

    static CustomersServiceTests()
    {
        var options = new DbContextOptionsBuilder<ControllerContext>()
            .UseInMemoryDatabase(nameof(CustomersServiceTests))
            .Options;

        ControllerContext = new(options);

        var mapperConfig = new TypeAdapterConfig();
        mapperConfig.Apply(new CoreMappingsRegister());
        var mapper = new Mapper(mapperConfig);

        EncryptorMock = new Mock<IEncryptor>();
        FeatureManagerMock = new Mock<IFeatureManager>();

        CustomersService = new(ControllerContext, mapper, EncryptorMock.Object, FeatureManagerMock.Object);
    }

    [ClassInitialize]
    public static void Initialize(TestContext testContext)
    {
        ControllerContext.Customers.Add(new Customer
        {
            Id = TestCustomerId,
            Name = "Test Customer",
            Domain = "test.customer",
            Created = DateTimeOffset.UtcNow,
            Status = CustomerStatuses.Active,
            Settings = new CustomerSettings
            {
                CoreDatabaseConnectionString = "Test",
                Encrypted = false,
            }
        });

        ControllerContext.SaveChanges();
    }

    [TestCleanup]
    public void Setup()
    {
        ControllerContext.ChangeTracker.Clear();
    }

    [TestMethod]
    public async Task GetCustomer_Should_ReturnCustomerWithNoSettings()
    {
        var customer = await CustomersService.GetCustomerAsync("test.customer");

        Assert.IsNotNull(customer);
        Assert.IsNull(customer.Settings);
    }

    [TestMethod, ExpectedException(typeof(MeteorNotFoundException))]
    public async Task GetCustomerThatDoesNotExist_Should_ThrowException()
    {
        await CustomersService.GetCustomerAsync("error.com");
    }

    [TestMethod]
    public async Task GetCustomerSettings_Should_ReturnCustomerSettings()
    {
        var settings = await CustomersService.GetCustomerSettings(TestCustomerId);

        Assert.IsNotNull(settings);
        Assert.AreEqual("Test", settings.CoreDatabaseConnectionString);
        Assert.AreEqual(false, settings.Encrypted);
    }

    [TestMethod, ExpectedException(typeof(MeteorNotFoundException))]
    public async Task GetSettingsOfCustomerThatDoesNotExist_Should_ThrowNotFoundException()
    {
        await CustomersService.GetCustomerSettings(-1);
    }

    [TestMethod, ExpectedException(typeof(MeteorException))]
    public async Task GetSettingsThatAreNotSetup_Should_ThrowNotFoundException()
    {
        ControllerContext.Customers.Add(new Customer
        {
            Id = TestCustomerId + 1,
            Domain = "Test2.Customer",
            Name = "Test Customer 2",
            Created = DateTimeOffset.UtcNow,
            Status = CustomerStatuses.Active,
        });
        await ControllerContext.SaveChangesAsync();

        await CustomersService.GetCustomerSettings(2);
    }

    [TestMethod, ExpectedException(typeof(MeteorNotFoundException))]
    public async Task SetSettingsForCustomerThatDoesNotExist_Should_ThrowNotFoundError()
    {
        await CustomersService.SetCustomerSettings(-1, new());
    }
    
    [TestMethod]
    public async Task SetCustomerSettings_Should_EncryptSensitiveData()
    {
        var customerId = TestCustomerId + 2;
        ControllerContext.Customers.Add(new()
        {
            Id = customerId,
            Name = "Test Customer 3",
            Domain = "Test3.Customer",
            Created = DateTimeOffset.UtcNow,
            Status = CustomerStatuses.Active,
        });
        await ControllerContext.SaveChangesAsync();

        var settingsDto = new SetCustomerSettingsDto
        {
            CoreConnectionString = "Test"
        };

        var encryptedConnectionString = RandomNumberGenerator.GetBytes(4);
        
        FeatureManagerMock
            .Setup(fm => fm.IsEnabledAsync(FeatureFlags.CustomerSettingsSensitiveDataEncryption))
            .ReturnsAsync(true);

        EncryptorMock
            .Setup(e => e.EncryptAsync(settingsDto.CoreConnectionString))
            .ReturnsAsync(encryptedConnectionString);

        EncryptorMock
            .Setup(e => e.DecryptAsync(It.Is<byte[]>(b => b.SequenceEqual(encryptedConnectionString))))
            .ReturnsAsync(settingsDto.CoreConnectionString);

        var returnedSettings = await CustomersService.SetCustomerSettings(customerId, settingsDto);
        Assert.IsTrue(returnedSettings.Encrypted);
        Assert.AreEqual(returnedSettings.CoreDatabaseConnectionString, settingsDto.CoreConnectionString);

        var storedSettings = await ControllerContext.Customers
            .Include(c => c.Settings)
            .Where(c => c.Id == customerId)
            .Select(c => c.Settings)
            .FirstOrDefaultAsync();
        
        Assert.IsNotNull(storedSettings);
        Assert.IsTrue(storedSettings.Encrypted);
        Assert.AreEqual(Convert.ToBase64String(encryptedConnectionString), storedSettings.CoreDatabaseConnectionString);
    }

    [TestMethod]
    public async Task SetCustomerSettingsWithDisabledEncryption_Should_EncryptSensitiveData()
    {
        var customerId = TestCustomerId + 3;
        ControllerContext.Customers.Add(new()
        {
            Id = customerId,
            Name = "Test Customer 4",
            Domain = "Test4.Customer",
            Created = DateTimeOffset.UtcNow,
            Status = CustomerStatuses.Active,
        });
        await ControllerContext.SaveChangesAsync();

        var settingsDto = new SetCustomerSettingsDto
        {
            CoreConnectionString = "Test"
        };

        FeatureManagerMock
            .Setup(fm => fm.IsEnabledAsync(FeatureFlags.CustomerSettingsSensitiveDataEncryption))
            .ReturnsAsync(false);

        var returnedSettings = await CustomersService.SetCustomerSettings(customerId, settingsDto);
        Assert.IsFalse(returnedSettings.Encrypted);
        Assert.AreEqual(returnedSettings.CoreDatabaseConnectionString, settingsDto.CoreConnectionString);

        var storedSettings = await ControllerContext.Customers
            .Include(c => c.Settings)
            .Where(c => c.Id == customerId)
            .Select(c => c.Settings)
            .FirstOrDefaultAsync();
        
        Assert.IsNotNull(storedSettings);
        Assert.IsFalse(storedSettings.Encrypted);
        Assert.AreEqual(storedSettings.CoreDatabaseConnectionString, storedSettings.CoreDatabaseConnectionString);
    }
}