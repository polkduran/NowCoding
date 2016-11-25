using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Model;

namespace Model.Migrations
{
    [DbContext(typeof(PetsDbContext))]
    [Migration("20161125163745_AddPersonName")]
    partial class AddPersonName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Model.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Age");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<int?>("OwnerId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Animal");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Animal");
                });

            modelBuilder.Entity("Model.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Model.Dog", b =>
                {
                    b.HasBaseType("Model.Animal");

                    b.Property<string>("FurSoftness");

                    b.ToTable("Dog");

                    b.HasDiscriminator().HasValue("Dog");
                });

            modelBuilder.Entity("Model.Platypus", b =>
                {
                    b.HasBaseType("Model.Animal");

                    b.Property<int>("BeakPower");

                    b.ToTable("Platypus");

                    b.HasDiscriminator().HasValue("Platypus");
                });

            modelBuilder.Entity("Model.Animal", b =>
                {
                    b.HasOne("Model.Person", "Owner")
                        .WithMany("Pets")
                        .HasForeignKey("OwnerId");
                });
        }
    }
}
