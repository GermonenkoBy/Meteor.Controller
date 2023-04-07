using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Meteor.Controller.Core.Models.Configuration;

public class ContactPersonEntityTypeConfiguration : IEntityTypeConfiguration<ContactPerson>
{
    public void Configure(EntityTypeBuilder<ContactPerson> builder)
    {
        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(cp => cp.CustomerId);

        builder.HasIndex(cp => new { cp.CustomerId, cp.EmailAddress })
            .IsUnique();
    }
}