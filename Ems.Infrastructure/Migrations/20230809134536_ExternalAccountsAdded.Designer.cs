﻿// <auto-generated />
using System;
using Ems.Infrastructure.Storages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ems.Infrastructure.Migrations
{
    [DbContext(typeof(EmsDbContext))]
    [Migration("20230809134536_ExternalAccountsAdded")]
    partial class ExternalAccountsAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ems.Core.Entities.Abstractions.Email", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Recipient")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("recipient");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("Emails");

                    b.HasDiscriminator<int>("Type");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Ems.Core.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset?>("ConfirmationExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmation_expires_at");

                    b.Property<string>("ConfirmationToken")
                        .HasColumnType("text")
                        .HasColumnName("confirmation_token");

                    b.Property<DateTimeOffset?>("ConfirmedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmed_at");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<int>("FailedAttempts")
                        .HasColumnType("integer")
                        .HasColumnName("failed_attempts");

                    b.Property<DateTimeOffset?>("LockExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("locked_at");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<DateTimeOffset?>("PasswordResetExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("password_reset_expires_at");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("text")
                        .HasColumnName("password_reset_token");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_salt");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("accounts", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.AccountRole", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("AccountId", "Role");

                    b.ToTable("account_roles", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.Class", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("ClassPeriodId")
                        .HasColumnType("uuid")
                        .HasColumnName("class_period_id");

                    b.Property<Guid?>("ClassVersionId")
                        .HasColumnType("uuid")
                        .HasColumnName("class_version_id");

                    b.Property<Guid>("ClassroomId")
                        .HasColumnType("uuid")
                        .HasColumnName("classroom_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int?>("Day")
                        .HasColumnType("integer")
                        .HasColumnName("day");

                    b.Property<DateTimeOffset?>("EndingAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ending_at");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<Guid>("LecturerId")
                        .HasColumnType("uuid")
                        .HasColumnName("lecturer_id");

                    b.Property<Guid>("LessonId")
                        .HasColumnType("uuid")
                        .HasColumnName("lesson_id");

                    b.Property<string>("Link")
                        .HasColumnType("text")
                        .HasColumnName("link");

                    b.Property<int?>("Quarter")
                        .HasColumnType("integer")
                        .HasColumnName("quarter");

                    b.Property<DateTimeOffset?>("StartingAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("starting_at");

                    b.Property<Guid?>("TemplateId")
                        .HasColumnType("uuid")
                        .HasColumnName("template_id");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("ClassPeriodId");

                    b.HasIndex("ClassVersionId");

                    b.HasIndex("ClassroomId");

                    b.HasIndex("GroupId");

                    b.HasIndex("LecturerId");

                    b.HasIndex("LessonId");

                    b.HasIndex("TemplateId");

                    b.ToTable("classes", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.ClassPeriod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<TimeSpan>("EndingAt")
                        .HasColumnType("interval")
                        .HasColumnName("ending_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<TimeSpan>("StartingAt")
                        .HasColumnType("interval")
                        .HasColumnName("starting_at");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("class_periods", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.ClassVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("class_versions", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.Classroom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("classrooms", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Course")
                        .HasColumnType("integer")
                        .HasColumnName("course");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("groups", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.IdlePeriod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset>("EndingAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ending_at");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<DateTimeOffset>("StartingAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("starting_at");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("idle_periods", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.Lecturer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text")
                        .HasColumnName("middle_name");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("lecturers", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.Lesson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("lessons", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("RevokedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("revoked_at");

                    b.Property<Guid?>("SessionTokenId")
                        .HasColumnType("uuid")
                        .HasColumnName("session_token_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<DateTimeOffset?>("UsedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("used_at");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("SessionTokenId");

                    b.ToTable("refresh_tokens", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.Setting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("CurrentQuarter")
                        .HasColumnType("integer")
                        .HasColumnName("current_quarter");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.ToTable("settings", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.Student", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid")
                        .HasColumnName("group_id");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text")
                        .HasColumnName("middle_name");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("GroupId");

                    b.ToTable("students", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.StudentRecord", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("uuid")
                        .HasColumnName("student_id");

                    b.Property<Guid>("ClassId")
                        .HasColumnType("uuid")
                        .HasColumnName("class_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("StudentId", "ClassId");

                    b.HasIndex("ClassId");

                    b.ToTable("student_records", "main");
                });

            modelBuilder.Entity("Ems.Core.ExternalAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("ExternalAccountProvider")
                        .HasColumnType("integer")
                        .HasColumnName("external_account_provider");

                    b.Property<string>("ExternalEmail")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("external_email");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("external_accounts", "main");
                });

            modelBuilder.Entity("Ems.Core.Entities.NewPasswordEmail", b =>
                {
                    b.HasBaseType("Ems.Core.Entities.Abstractions.Email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Ems.Core.Entities.PasswordResetEmail", b =>
                {
                    b.HasBaseType("Ems.Core.Entities.Abstractions.Email");

                    b.Property<DateTimeOffset>("PasswordResetExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("password_reset_expires_at");

                    b.Property<string>("PasswordResetToken")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_reset_token");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Ems.Core.Entities.ReconfirmationEmail", b =>
                {
                    b.HasBaseType("Ems.Core.Entities.Abstractions.Email");

                    b.Property<DateTimeOffset>("ConfirmationExpiresAt")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmation_expires_at");

                    b.Property<string>("ConfirmationToken")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("text")
                        .HasColumnName("confirmation_token");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Ems.Core.Entities.RegistrationEmail", b =>
                {
                    b.HasBaseType("Ems.Core.Entities.Abstractions.Email");

                    b.Property<DateTimeOffset>("ConfirmationExpiresAt")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmation_expires_at");

                    b.Property<string>("ConfirmationToken")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("text")
                        .HasColumnName("confirmation_token");

                    b.Property<string>("Password")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Ems.Core.Entities.AccountRole", b =>
                {
                    b.HasOne("Ems.Core.Entities.Account", "Account")
                        .WithMany("Roles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Ems.Core.Entities.Class", b =>
                {
                    b.HasOne("Ems.Core.Entities.ClassPeriod", "ClassPeriod")
                        .WithMany()
                        .HasForeignKey("ClassPeriodId");

                    b.HasOne("Ems.Core.Entities.ClassVersion", "ClassVersion")
                        .WithMany("Classes")
                        .HasForeignKey("ClassVersionId");

                    b.HasOne("Ems.Core.Entities.Classroom", "Classroom")
                        .WithMany()
                        .HasForeignKey("ClassroomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ems.Core.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("Ems.Core.Entities.Lecturer", "Lecturer")
                        .WithMany()
                        .HasForeignKey("LecturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ems.Core.Entities.Lesson", "Lesson")
                        .WithMany()
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ems.Core.Entities.Class", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId");

                    b.Navigation("ClassPeriod");

                    b.Navigation("ClassVersion");

                    b.Navigation("Classroom");

                    b.Navigation("Group");

                    b.Navigation("Lecturer");

                    b.Navigation("Lesson");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("Ems.Core.Entities.IdlePeriod", b =>
                {
                    b.HasOne("Ems.Core.Entities.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Ems.Core.Entities.Lecturer", b =>
                {
                    b.HasOne("Ems.Core.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Ems.Core.Entities.RefreshToken", b =>
                {
                    b.HasOne("Ems.Core.Entities.Account", "Account")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ems.Core.Entities.RefreshToken", "SessionToken")
                        .WithMany()
                        .HasForeignKey("SessionTokenId");

                    b.Navigation("Account");

                    b.Navigation("SessionToken");
                });

            modelBuilder.Entity("Ems.Core.Entities.Student", b =>
                {
                    b.HasOne("Ems.Core.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ems.Core.Entities.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Ems.Core.Entities.StudentRecord", b =>
                {
                    b.HasOne("Ems.Core.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ems.Core.Entities.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Ems.Core.ExternalAccount", b =>
                {
                    b.HasOne("Ems.Core.Entities.Account", "Account")
                        .WithMany("ExternalAccounts")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Ems.Core.Entities.Account", b =>
                {
                    b.Navigation("ExternalAccounts");

                    b.Navigation("RefreshTokens");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Ems.Core.Entities.ClassVersion", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("Ems.Core.Entities.Group", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
