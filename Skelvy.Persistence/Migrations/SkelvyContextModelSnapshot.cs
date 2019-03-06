﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Skelvy.Persistence;

namespace Skelvy.Persistence.Migrations
{
    [DbContext(typeof(SkelvyContext))]
    partial class SkelvyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Skelvy.Domain.Entities.Drink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Drinks");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("DrinkId");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.HasKey("Id");

                    b.HasIndex("DrinkId");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("MeetingId");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("UserId");

                    b.ToTable("MeetingChatMessages");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<int>("MaxAge");

                    b.Property<DateTime>("MaxDate");

                    b.Property<int>("MinAge");

                    b.Property<DateTime>("MinDate");

                    b.Property<string>("Status");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("MeetingRequests");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingRequestDrink", b =>
                {
                    b.Property<int>("MeetingRequestId");

                    b.Property<int>("DrinkId");

                    b.HasKey("MeetingRequestId", "DrinkId");

                    b.HasIndex("DrinkId");

                    b.ToTable("MeetingRequestDrinks");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingUser", b =>
                {
                    b.Property<int>("MeetingId");

                    b.Property<int>("UserId");

                    b.HasKey("MeetingId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("MeetingUsers");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FacebookId")
                        .HasMaxLength(50);

                    b.Property<string>("GoogleId");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserProfilePhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProfileId");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2048);

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.ToTable("UserProfilePhotos");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.Meeting", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.Drink", "Drink")
                        .WithMany()
                        .HasForeignKey("DrinkId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingChatMessage", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.Meeting", "Meeting")
                        .WithMany("ChatMessages")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithMany("MeetingChatMessages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingRequest", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithOne("MeetingRequest")
                        .HasForeignKey("Skelvy.Domain.Entities.MeetingRequest", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingRequestDrink", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.Drink", "Drink")
                        .WithMany()
                        .HasForeignKey("DrinkId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Skelvy.Domain.Entities.MeetingRequest", "MeetingRequest")
                        .WithMany("Drinks")
                        .HasForeignKey("MeetingRequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingUser", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.Meeting", "Meeting")
                        .WithMany("Users")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserProfile", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("Skelvy.Domain.Entities.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserProfilePhoto", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.UserProfile", "Profile")
                        .WithMany("Photos")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
