namespace Meteor.Controller.Core.Dtos;

public record struct SetCustomerSettingsDto()
{
    public string? CoreConnectionString = null;

    public string? FullTextSearchApiKey = null;

    public string? FullTextSearchUrl = null;
}