namespace Meteor.Controller.Core;

using Models;
using Models.Configuration;

public class ControllerContext : DbContext
{
    public ControllerContext(DbContextOptions<ControllerContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<ContactPerson> ContactPersons => Set<ContactPerson>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContactPersonEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerSettingsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
    }
}