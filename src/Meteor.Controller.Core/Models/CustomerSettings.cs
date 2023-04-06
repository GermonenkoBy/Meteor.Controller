namespace Meteor.Controller.Core.Models;

public class CustomerSettings
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Core database connection string is required.")]
    [StringLength(400, ErrorMessage = "Max connection string length is 400.")]
    public string CoreDatabaseConnectionString { get; set; } = string.Empty;
}