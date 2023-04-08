using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Models;

namespace Meteor.Controller.Core.Services.Contracts;

public interface ICustomersService
{
    Task<Customer> GetCustomerAsync(string domain);

    Task<CustomerSettings> GetCustomerSettings(int customerId);

    Task<CustomerSettings> SetCustomerSettings(int customerId, SetCustomerSettingsDto settings);
}