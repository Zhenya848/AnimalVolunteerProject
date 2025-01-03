﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotificationProject.Infrastructure.DbContexts;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NotificationProject.Migrations
{
    [DbContext(typeof(NotificationDbContext))]
    [Migration("20241229085009_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NotificationProject.Entities.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsSent")
                        .HasColumnType("boolean")
                        .HasColumnName("is_sent");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("roles");

                    b.Property<string>("Users")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("users");

                    b.HasKey("Id")
                        .HasName("pk_notifications");

                    b.ToTable("notifications", (string)null);
                });

            modelBuilder.Entity("NotificationProject.Entities.NotificationSettings", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<bool>("Email")
                        .HasColumnType("boolean")
                        .HasColumnName("email");

                    b.Property<bool>("Telegram")
                        .HasColumnType("boolean")
                        .HasColumnName("telegram");

                    b.Property<bool>("Web")
                        .HasColumnType("boolean")
                        .HasColumnName("web");

                    b.HasKey("UserId")
                        .HasName("pk_notification_settings");

                    b.ToTable("notification_settings", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
