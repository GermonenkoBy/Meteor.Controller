using Meteor.Controller.Core.Models.Enums;

namespace Meteor.Controller.Core.Models;

public class Customer
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Customer name is required.")]
    [StringLength(100, ErrorMessage = "Max name length is 100.")]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Customer domain is required.")]
    public string Domain { get; set; } = string.Empty;

    public DateTimeOffset Created { get; set; }

    public CustomerStatuses Status { get; set; }

    public CustomerSettings? Settings { get; set; }
}