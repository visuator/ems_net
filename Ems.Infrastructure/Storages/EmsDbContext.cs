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

    public DbSet<Class> Classes { get; set; }
    public DbSet<ClassPeriod> ClassPeriods { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<ClassVersion> ClassVersions { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<IdlePeriod> IdlePeriods { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentRecord> StudentRecords { get; set; }
    public DbSet<GeolocationStudentRecord> GeolocationStudentRecords { get; set; }
    public DbSet<StudentRecordSession> StudentRecordSessions { get; set; }
    public DbSet<QrCodeAttempt> QrCodeAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentRecord>()
            .HasKey(sr => new { sr.StudentId, sr.ClassId });
        modelBuilder.Entity<StudentRecord>()
            .HasDiscriminator(x => x.Type)
            .HasValue<GeolocationStudentRecord>(StudentRecordType.Geolocation);
        modelBuilder.Entity<StudentRecordSession>()
            .HasDiscriminator(x => x.Type)
            .HasValue<GeolocationStudentRecordSession>(StudentRecordSessionType.Geolocation)
            .HasValue<QrCodeStudentRecordSession>(StudentRecordSessionType.QrCode);

        base.OnModelCreating(modelBuilder);
    }
}