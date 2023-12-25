using AppAny.Quartz.EntityFrameworkCore.Migrations;
using AppAny.Quartz.EntityFrameworkCore.Migrations.PostgreSQL;
using Ems.Core.Entities;
using Ems.Core.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Ems.Infrastructure.Storage;

public class EmsDbContext : DbContext
{
    public EmsDbContext(DbContextOptions options) : base(options) { }

    public required DbSet<Class> Classes { get; set; }
    public required DbSet<ClassPeriod> ClassPeriods { get; set; }
    public required DbSet<Classroom> Classrooms { get; set; }
    public required DbSet<ClassVersion> ClassVersions { get; set; }
    public required DbSet<Group> Groups { get; set; }
    public required DbSet<IdlePeriod> IdlePeriods { get; set; }
    public required DbSet<Lesson> Lessons { get; set; }
    public required DbSet<Setting> Settings { get; set; }
    public required DbSet<Person> Persons { get; set; }
    public required DbSet<Student> Students { get; set; }
    public required DbSet<Lecturer> Lecturers { get; set; }
    public required DbSet<StudentRecord> StudentRecords { get; set; }
    public required DbSet<QrCodeStudentRecord> QrCodeStudentRecords { get; set; }
    public required DbSet<GeolocationStudentRecord> GeolocationStudentRecords { get; set; }
    public required DbSet<StudentRecordSession> StudentRecordSessions { get; set; }
    public required DbSet<QrCodeStudentRecordSession> QrCodeStudentRecordSessions { get; set; }
    public required DbSet<GeolocationStudentRecordSession> GeolocationStudentRecordSessions { get; set; }
    public required DbSet<QrCodeAttempt> QrCodeAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentRecord>()
            .HasKey(sr => new { sr.StudentId, sr.ClassId });
        modelBuilder.AddQuartz(opt => opt.UsePostgreSql(schema: "public"));

        base.OnModelCreating(modelBuilder);
    }
}