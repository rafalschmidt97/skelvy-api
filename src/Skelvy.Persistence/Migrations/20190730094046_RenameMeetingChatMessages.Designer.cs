﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Skelvy.Persistence;

namespace Skelvy.Persistence.Migrations
{
    [DbContext(typeof(SkelvyContext))]
    [Migration("20190730094046_RenameMeetingChatMessages")]
    partial class RenameMeetingChatMessages
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Skelvy.Domain.Entities.Attachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2048);

                    b.HasKey("Id");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.BlockedUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BlockUserId");

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<bool>("IsRemoved");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsConcurrencyToken();

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BlockUserId");

                    b.HasIndex("IsRemoved");

                    b.HasIndex("UserId");

                    b.ToTable("BlockedUsers");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.DrinkType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("DrinkTypes");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<bool>("IsRemoved");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsConcurrencyToken();

                    b.Property<string>("RemovedReason")
                        .HasMaxLength(15);

                    b.HasKey("Id");

                    b.HasIndex("IsRemoved");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.GroupUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<int>("GroupId");

                    b.Property<bool>("IsRemoved");

                    b.Property<int>("MeetingRequestId");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsConcurrencyToken();

                    b.Property<string>("RemovedReason")
                        .HasMaxLength(15);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("IsRemoved");

                    b.HasIndex("MeetingRequestId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<DateTimeOffset>("Date");

                    b.Property<int>("DrinkTypeId");

                    b.Property<int>("GroupId");

                    b.Property<bool>("IsRemoved");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsConcurrencyToken();

                    b.Property<string>("RemovedReason")
                        .HasMaxLength(15);

                    b.HasKey("Id");

                    b.HasIndex("Date");

                    b.HasIndex("DrinkTypeId");

                    b.HasIndex("GroupId");

                    b.HasIndex("IsRemoved");

                    b.HasIndex("Latitude");

                    b.HasIndex("Longitude");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<bool>("IsRemoved");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<int>("MaxAge");

                    b.Property<DateTimeOffset>("MaxDate");

                    b.Property<int>("MinAge");

                    b.Property<DateTimeOffset>("MinDate");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsConcurrencyToken();

                    b.Property<string>("RemovedReason")
                        .HasMaxLength(15);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("IsRemoved");

                    b.HasIndex("Latitude");

                    b.HasIndex("Longitude");

                    b.HasIndex("MaxAge");

                    b.HasIndex("MaxDate");

                    b.HasIndex("MinAge");

                    b.HasIndex("MinDate");

                    b.HasIndex("Status");

                    b.HasIndex("UserId");

                    b.ToTable("MeetingRequests");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingRequestDrinkType", b =>
                {
                    b.Property<int>("MeetingRequestId");

                    b.Property<int>("DrinkTypeId");

                    b.HasKey("MeetingRequestId", "DrinkTypeId");

                    b.HasIndex("DrinkTypeId");

                    b.ToTable("MeetingRequestDrinkTypes");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AttachmentId");

                    b.Property<DateTimeOffset>("Date");

                    b.Property<int>("GroupId");

                    b.Property<string>("Text")
                        .HasMaxLength(500);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("Date");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<string>("DisabledReason")
                        .HasMaxLength(1024);

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<string>("FacebookId")
                        .HasMaxLength(50);

                    b.Property<DateTimeOffset?>("ForgottenAt");

                    b.Property<string>("GoogleId")
                        .HasMaxLength(50);

                    b.Property<bool>("IsDisabled");

                    b.Property<bool>("IsRemoved");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsConcurrencyToken();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("FacebookId")
                        .IsUnique()
                        .HasFilter("[FacebookId] IS NOT NULL");

                    b.HasIndex("GoogleId")
                        .IsUnique()
                        .HasFilter("[GoogleId] IS NOT NULL");

                    b.HasIndex("IsRemoved");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("Birthday");

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Birthday");

                    b.HasIndex("Gender");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserProfilePhoto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttachmentId");

                    b.Property<int>("Order");

                    b.Property<int>("ProfileId");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("ProfileId");

                    b.ToTable("UserProfilePhotos");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.BlockedUser", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.User", "BlockUser")
                        .WithMany()
                        .HasForeignKey("BlockUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.GroupUser", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Skelvy.Domain.Entities.MeetingRequest", "MeetingRequest")
                        .WithMany()
                        .HasForeignKey("MeetingRequestId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.Meeting", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.DrinkType", "DrinkType")
                        .WithMany()
                        .HasForeignKey("DrinkTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Skelvy.Domain.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingRequest", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithMany("MeetingRequests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.MeetingRequestDrinkType", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.DrinkType", "DrinkType")
                        .WithMany()
                        .HasForeignKey("DrinkTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Skelvy.Domain.Entities.MeetingRequest", "MeetingRequest")
                        .WithMany("DrinkTypes")
                        .HasForeignKey("MeetingRequestId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.Message", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Skelvy.Domain.Entities.Group", "Group")
                        .WithMany("Messages")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserProfile", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("Skelvy.Domain.Entities.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserProfilePhoto", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.Attachment", "Attachment")
                        .WithMany()
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Skelvy.Domain.Entities.UserProfile", "Profile")
                        .WithMany("Photos")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Skelvy.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("Skelvy.Domain.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
