using Ems.Core.Entities;
using Ems.Core.Entities.Abstractions;
using Ems.Core.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Ems.Infrastructure.Storages;

public class EmsDbContext : DbContext
{
    public EmsDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<ExternalAccount> ExternalAccounts { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<ClassPeriod> ClassPeriods { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<ClassVersion> ClassVersions { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<IdlePeriod> IdlePeriods { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentRecord> StudentRecords { get; set; }
    public DbSet<GeolocationStudentRecord> GeolocationStudentRecords { get; set; }
    public DbSet<StudentRecordSession> StudentRecordSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountRole>()
            .HasKey(ar => new { ar.AccountId, ar.Role });
        modelBuilder.Entity<StudentRecord>()
            .HasKey(sr => new { sr.StudentId, sr.ClassId });
        modelBuilder.Entity<StudentRecord>()
            .HasDiscriminator(x => x.Type)
            .HasValue<GeolocationStudentRecord>(StudentRecordType.Geolocation);
        modelBuilder.Entity<StudentRecordSession>()
            .HasDiscriminator(x => x.Type)
            .HasValue<GeolocationStudentRecordSession>(StudentRecordSessionType.Geolocation);
        modelBuilder.Entity<Email>()
            .HasDiscriminator(x => x.Type)
            .HasValue<RegistrationEmail>(EmailType.Registration)
            .HasValue<ReconfirmationEmail>(EmailType.Reconfirmation)
            .HasValue<PasswordResetEmail>(EmailType.PasswordReset)
            .HasValue<NewPasswordEmail>(EmailType.NewPassword);

        base.OnModelCreating(modelBuilder);
    }
}