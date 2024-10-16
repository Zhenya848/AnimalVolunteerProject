﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetProject.Infastructure.DbContexts;

#nullable disable

namespace PetProject.Infastructure.Migrations
{
    [DbContext(typeof(WriteDbContext))]
    [Migration("20241013193713_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetProject.Domain.Species.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("species_id")
                        .HasColumnType("uuid")
                        .HasColumnName("species_id");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_breed");

                    b.HasIndex("species_id")
                        .HasDatabaseName("ix_breed_species_id");

                    b.ToTable("breed", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Species.Species", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("species", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Volunteers.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("BirthdayTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("birthday_time");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("color");

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_of_creation");

                    b.Property<string>("HealthInfo")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("health_info");

                    b.Property<float>("Height")
                        .HasColumnType("real")
                        .HasColumnName("height");

                    b.Property<string>("HelpStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("help_status");

                    b.Property<bool>("IsCastrated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_castrated");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("Photos")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("photos");

                    b.Property<string>("Requisites")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("requisites");

                    b.Property<float>("Weight")
                        .HasColumnType("real")
                        .HasColumnName("weight");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "PetProject.Domain.Volunteers.Pet.Address#Addres", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("city");

                            b1.Property<string>("State")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("state");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("street");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("zipcode");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetProject.Domain.Volunteers.Pet.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(1000)
                                .HasColumnType("character varying(1000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PetTypeInfo", "PetProject.Domain.Volunteers.Pet.PetTypeInfo#PetTypeInfo", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("pet_type_info_breed_id");

                            b1.Property<Guid>("SpeciesId")
                                .HasColumnType("uuid")
                                .HasColumnName("pet_type_info_species_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SerialNumber", "PetProject.Domain.Volunteers.Pet.SerialNumber#SerialNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("serial_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("TelephoneNumber", "PetProject.Domain.Volunteers.Pet.TelephoneNumber#TelephoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasMaxLength(13)
                                .HasColumnType("character varying(13)")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Volunteers.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Requisites")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("requisites");

                    b.Property<string>("SocialNetworks")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("social_networks");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetProject.Domain.Volunteers.Volunteer.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(1000)
                                .HasColumnType("character varying(1000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Experience", "PetProject.Domain.Volunteers.Volunteer.Experience#Experience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("experience");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "PetProject.Domain.Volunteers.Volunteer.Name#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("first_name");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("last_name");

                            b1.Property<string>("Patronymic")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("patronymic");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("TelephoneNumber", "PetProject.Domain.Volunteers.Volunteer.TelephoneNumber#TelephoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("PhoneNumber")
                                .IsRequired()
                                .HasMaxLength(13)
                                .HasColumnType("character varying(13)")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Species.Breed", b =>
                {
                    b.HasOne("PetProject.Domain.Species.Species", null)
                        .WithMany("Breeds")
                        .HasForeignKey("species_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_breed_species_species_id");
                });

            modelBuilder.Entity("PetProject.Domain.Volunteers.Pet", b =>
                {
                    b.HasOne("PetProject.Domain.Volunteers.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");
                });

            modelBuilder.Entity("PetProject.Domain.Species.Species", b =>
                {
                    b.Navigation("Breeds");
                });

            modelBuilder.Entity("PetProject.Domain.Volunteers.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
