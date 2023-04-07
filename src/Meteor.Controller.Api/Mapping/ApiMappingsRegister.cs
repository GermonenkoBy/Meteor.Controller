﻿using Google.Protobuf.WellKnownTypes;
using Mapster;
using Meteor.Controller.Core.Dtos;
using Meteor.Controller.Core.Models;

namespace Meteor.Controller.Api.Mapping;

public class ApiMappingsRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Customer, Grpc.Customer>()
            .Map(c => c.Created, c => Timestamp.FromDateTimeOffset(c.Created));

        config.ForType<Grpc.SetCustomerSettingsRequest, SetCustomerSettingsDto>()
            .Map(dto => dto.CoreConnectionString, req => req.CoreConnectionString, req => req.HasCoreConnectionString);

        config.ForType<CustomerSettings, Grpc.CustomerSettings>()
            .Map(cs => cs.CoreConnectionString, cs => cs.CoreDatabaseConnectionString);
    }
}