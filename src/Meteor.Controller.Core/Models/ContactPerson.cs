namespace Meteor.Controller.Core.Models;

public class ContactPerson
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Max name length is 100.")]
    public string FullName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
    [StringLength(250, ErrorMessage = "Max email address length is 100.")]
    [EmailAddress(ErrorMessage = "Must be a valid email address.")]
    public string EmailAddress { get; set; } = string.Empty;
}