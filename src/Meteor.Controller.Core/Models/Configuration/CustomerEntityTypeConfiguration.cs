using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meteor.Controller.Core.Models.Configuration;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    private const string CustomerIdFieldName = "CustomerId";

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasOne(c => c.Settings)
            .WithOne()
            .HasForeignKey<CustomerSettings>(CustomerIdFieldName);
    }
}