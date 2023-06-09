﻿using MapsterMapper;
using Meteor.Common.Core.Exceptions;
using Meteor.Common.Core.Models;
using Meteor.Common.Cryptography.Abstractions;
using Meteor.Controller.Core.Constants;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Models;
using Meteor.Controller.Core.Services.Contracts;
using Microsoft.FeatureManagement;

namespace Meteor.Controller.Core.Services;

public class CustomersService : ICustomersService
{
    private readonly ControllerContext _context;

    private readonly IMapper _mapper;

    private readonly IEncryptor _encryptor;

    private readonly IFeatureManager _featureManager;

    public CustomersService(
        ControllerContext context,
        IMapper mapper,
        IEncryptor encryptor,
        IFeatureManager featureManager
    )
    {
        _context = context;
        _mapper = mapper;
        _encryptor = encryptor;
        _featureManager = featureManager;
    }

    public async Task<Customer> GetCustomerAsync(int customerId)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer is null)
        {
            throw new MeteorNotFoundException("Customer not found.");
        }

        return customer;
    }

    public async Task<Customer> GetCustomerAsync(string domain)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Domain == domain);

        if (customer is null)
        {
            throw new MeteorNotFoundException($"Domain {domain} does not belong to a customer.");
        }

        return customer;
    }

    public async Task<PagedResult<Customer>> GetCustomersAsync(CustomersFilter filter)
    {
        var query = _context.Customers
            .AsNoTracking();

        if (filter.Statuses.Any())
        {
            query = query.Where(c => filter.Statuses.Contains(c.Status));
        }

        var total = await query.CountAsync();
        var customers = await query.OrderBy(c => c.Name)
            .Skip(filter.Paging.Offset)
            .Take(filter.Paging.Limit)
            .ToListAsync();

        return new(customers, total);
    }

    public async Task<CustomerSettings> GetCustomerSettings(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.Settings)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer is null)
        {
            throw new MeteorNotFoundException("Customer not found.");
        }

        if (customer.Settings is null)
        {
            throw new MeteorException("Customer settings are not setup.");
        }

        if (customer.Settings.Encrypted)
        {
            await DecryptSensitiveData(customer.Settings);
        }
        return customer.Settings;
    }

    public async Task<CustomerSettings> SetCustomerSettings(int customerId, SetCustomerSettingsDto settings)
    {
        var customer = await _context.Customers
            .Include(c => c.Settings)
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer is null)
        {
            throw new MeteorNotFoundException("Customer not found.");
        }

        customer.Settings ??= new();
        _mapper.Map(settings, customer.Settings);

        var encryptionEnabled = await _featureManager
            .IsEnabledAsync(FeatureFlags.CustomerSettingsSensitiveDataEncryption);

        if (encryptionEnabled)
        {
            await EncryptSensitiveData(customer.Settings);
        }

        await _context.SaveChangesAsync();

        return await GetCustomerSettings(customerId);
    }

    private async Task EncryptSensitiveData(CustomerSettings settings)
    {
        var encryptedCoreConnectionString = await _encryptor.EncryptAsync(settings.CoreDatabaseConnectionString);
        settings.CoreDatabaseConnectionString = Convert.ToBase64String(encryptedCoreConnectionString);

        if (!string.IsNullOrEmpty(settings.FullTextSearchUrl))
        {
            var bytes = await _encryptor.EncryptAsync(settings.FullTextSearchUrl);
            settings.FullTextSearchUrl = Convert.ToBase64String(bytes);
        }

        if (!string.IsNullOrEmpty(settings.FullTextSearchApiKey))
        {
            var bytes = await _encryptor.EncryptAsync(settings.FullTextSearchApiKey);
            settings.FullTextSearchApiKey = Convert.ToBase64String(bytes);
        }

        settings.Encrypted = true;
    }

    private async Task DecryptSensitiveData(CustomerSettings settings)
    {
        if (!settings.Encrypted)
        {
            return;
        }
        var coreConnectionStringBytes = Convert.FromBase64String(settings.CoreDatabaseConnectionString);
        settings.CoreDatabaseConnectionString = await _encryptor.DecryptAsync(coreConnectionStringBytes);

        if (!string.IsNullOrEmpty(settings.FullTextSearchUrl))
        {
            var bytes = Convert.FromBase64String(settings.FullTextSearchUrl);
            settings.FullTextSearchUrl = await _encryptor.DecryptAsync(bytes);
        }

        if (!string.IsNullOrEmpty(settings.FullTextSearchApiKey))
        {
            var bytes = Convert.FromBase64String(settings.FullTextSearchApiKey);
            settings.FullTextSearchApiKey = await _encryptor.DecryptAsync(bytes);
        }
    }
}