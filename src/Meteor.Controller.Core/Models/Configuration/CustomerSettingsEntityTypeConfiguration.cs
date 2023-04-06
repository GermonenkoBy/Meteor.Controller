using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meteor.Controller.Core.Models.Configuration;

public class CustomerSettingsEntityTypeConfiguration : IEntityTypeConfiguration<CustomerSettings>
{
    private const string CustomerIdFieldName = "CustomerId";

    public void Configure(EntityTypeBuilder<CustomerSettings> builder)
    {
        builder.ToTable("customer_settings");
        builder.Property<int>(CustomerIdFieldName);
        builder.HasKey(CustomerIdFieldName);
    }
}