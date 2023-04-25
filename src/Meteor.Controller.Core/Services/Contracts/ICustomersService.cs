using Meteor.Common.Core.Models;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Models;

namespace Meteor.Controller.Core.Services.Contracts;

public interface ICustomersService
{
    Task<Customer> GetCustomerAsync(int customerId);

    Task<Customer> GetCustomerAsync(string domain);

    Task<PagedResult<Customer>> GetCustomersAsync(CustomersFilter filter);

    Task<CustomerSettings> GetCustomerSettings(int customerId);

    Task<CustomerSettings> SetCustomerSettings(int customerId, SetCustomerSettingsDto settings);
}