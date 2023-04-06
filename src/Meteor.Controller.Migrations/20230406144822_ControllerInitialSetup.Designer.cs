﻿// <auto-generated />

#nullable disable

using Meteor.Controller.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Meteor.Controller.Migrations
{
    [DbContext(typeof(ControllerContext))]
    [Migration("20230406144822_ControllerInitialSetup")]
    partial class ControllerInitialSetup
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Meteor.Controller.Core.Models.ContactPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("integer")
                        .HasColumnName("customer_id");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("email_address");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("full_name");

                    b.HasKey("Id")
                        .HasName("pk_contact_persons");

                    b.HasIndex("CustomerId")
                        .HasDatabaseName("ix_contact_persons_customer_id");

                    b.ToTable("contact_persons", (string)null);
                });

            modelBuilder.Entity("Meteor.Controller.Core.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("domain");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_customers");

                    b.ToTable("customers", (string)null);
                });

            modelBuilder.Entity("Meteor.Controller.Core.Models.CustomerSettings", b =>
                {
                    b.Property<int>("CustomerId")
                        .HasColumnType("integer")
                        .HasColumnName("customer_id");

                    b.Property<string>("CoreDatabaseConnectionString")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)")
                        .HasColumnName("core_database_connection_string");

                    b.HasKey("CustomerId")
                        .HasName("pk_customer_settings");

                    b.ToTable("customer_settings", (string)null);
                });

            modelBuilder.Entity("Meteor.Controller.Core.Models.ContactPerson", b =>
                {
                    b.HasOne("Meteor.Controller.Core.Models.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_contact_persons_customers_customer_id");
                });

            modelBuilder.Entity("Meteor.Controller.Core.Models.CustomerSettings", b =>
                {
                    b.HasOne("Meteor.Controller.Core.Models.Customer", null)
                        .WithOne("Settings")
                        .HasForeignKey("Meteor.Controller.Core.Models.CustomerSettings", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_customer_settings_customers_customer_id");
                });

            modelBuilder.Entity("Meteor.Controller.Core.Models.Customer", b =>
                {
                    b.Navigation("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
