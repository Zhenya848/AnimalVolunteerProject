﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetProject.Infastructure;

#nullable disable

namespace PetProject.Infastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetProject.Domain.Aggregates.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("CountOfHomelessAnimals")
                        .HasColumnType("integer")
                        .HasColumnName("count_of_homeless_animals");

                    b.Property<int>("CountOfIllAnimals")
                        .HasColumnType("integer")
                        .HasColumnName("count_of_ill_animals");

                    b.Property<int>("CountOfShelterAnimals")
                        .HasColumnType("integer")
                        .HasColumnName("count_of_shelter_animals");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("description");

                    b.Property<int>("EXP")
                        .HasColumnType("integer")
                        .HasColumnName("exp");

                    b.Property<string>("TelephoneNumber")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)")
                        .HasColumnName("telephone_number");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "PetProject.Domain.Aggregates.Volunteer.Name#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("name_first_name");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("name_last_name");

                            b1.Property<string>("Patronymic")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("name_patronymic");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("Volunteers", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("address");

                    b.Property<DateOnly>("BirthdayTime")
                        .HasColumnType("date")
                        .HasColumnName("birthday_time");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("breed");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("color");

                    b.Property<DateOnly>("DateOfCreation")
                        .HasColumnType("date")
                        .HasColumnName("date_of_creation");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("description");

                    b.Property<string>("HealthInfo")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("health_info");

                    b.Property<float>("Height")
                        .HasColumnType("real")
                        .HasColumnName("height");

                    b.Property<int>("HelpStatus")
                        .HasMaxLength(50)
                        .HasColumnType("integer")
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

                    b.Property<string>("PetType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("pet_type");

                    b.Property<string>("TelephoneNumber")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)")
                        .HasColumnName("telephone_number");

                    b.Property<float>("Weight")
                        .HasColumnType("real")
                        .HasColumnName("weight");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("Pets", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Entities.PetPhoto", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsMainPhoto")
                        .HasColumnType("boolean")
                        .HasColumnName("is_main_photo");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("path");

                    b.Property<Guid?>("pet_id")
                        .HasColumnType("uuid")
                        .HasColumnName("pet_id");

                    b.HasKey("Id")
                        .HasName("pk_pet_photos");

                    b.HasIndex("pet_id")
                        .HasDatabaseName("ix_pet_photos_pet_id");

                    b.ToTable("PetPhotos", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Requisite", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<Guid?>("pet_id")
                        .HasColumnType("uuid")
                        .HasColumnName("pet_id");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.HasKey("Id")
                        .HasName("pk_requisites");

                    b.HasIndex("pet_id")
                        .HasDatabaseName("ix_requisites_pet_id");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_requisites_volunteer_id");

                    b.ToTable("Requisites", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Entities.SotialNetwork", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("reference");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.HasKey("Id")
                        .HasName("pk_sotial_networks");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_sotial_networks_volunteer_id");

                    b.ToTable("SotialNetworks", (string)null);
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Pet", b =>
                {
                    b.HasOne("PetProject.Domain.Aggregates.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");
                });

            modelBuilder.Entity("PetProject.Domain.Entities.PetPhoto", b =>
                {
                    b.HasOne("PetProject.Domain.Entities.Pet", null)
                        .WithMany("PetPhotos")
                        .HasForeignKey("pet_id")
                        .HasConstraintName("fk_pet_photos_pets_pet_id");
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Requisite", b =>
                {
                    b.HasOne("PetProject.Domain.Entities.Pet", null)
                        .WithMany("Requisites")
                        .HasForeignKey("pet_id")
                        .HasConstraintName("fk_requisites_pets_pet_id");

                    b.HasOne("PetProject.Domain.Aggregates.Volunteer", null)
                        .WithMany("Requisites")
                        .HasForeignKey("volunteer_id")
                        .HasConstraintName("fk_requisites_volunteers_volunteer_id");
                });

            modelBuilder.Entity("PetProject.Domain.Entities.SotialNetwork", b =>
                {
                    b.HasOne("PetProject.Domain.Aggregates.Volunteer", null)
                        .WithMany("SotialNetworks")
                        .HasForeignKey("volunteer_id")
                        .HasConstraintName("fk_sotial_networks_volunteers_volunteer_id");
                });

            modelBuilder.Entity("PetProject.Domain.Aggregates.Volunteer", b =>
                {
                    b.Navigation("Pets");

                    b.Navigation("Requisites");

                    b.Navigation("SotialNetworks");
                });

            modelBuilder.Entity("PetProject.Domain.Entities.Pet", b =>
                {
                    b.Navigation("PetPhotos");

                    b.Navigation("Requisites");
                });
#pragma warning restore 612, 618
        }
    }
}
