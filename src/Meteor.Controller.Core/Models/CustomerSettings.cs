namespace Meteor.Controller.Core.Models;

public class CustomerSettings
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Core database connection string is required.")]
    [StringLength(400, ErrorMessage = "Max connection string length is 400.")]
    public string CoreDatabaseConnectionString { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Max full text search URL is 200.")]
    public string? FullTextSearchUrl { get; set; }

    [StringLength(200, ErrorMessage = "Max full text search API key is 200.")]
    public string? FullTextSearchApiKey { get; set; }

    public bool Encrypted { get; set; }
}