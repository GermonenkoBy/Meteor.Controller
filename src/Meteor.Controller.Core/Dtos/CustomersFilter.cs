using Meteor.Common.Core.Models;
using Meteor.Controller.Core.Models.Enums;

namespace Meteor.Controller.Core.Dtos;

public record struct CustomersFilter()
{
    public List<CustomerStatuses> Statuses = new(4);

    public Paging Paging = new();
}