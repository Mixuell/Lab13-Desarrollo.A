using APIDemoLab.Models;
using Microsoft.EntityFrameworkCore;

namespace APIDemoLab.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(x => x.IdGrade);
            entity.Property(x => x.Name).HasMaxLength(45).IsUnicode(false).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(45).IsUnicode(false).IsRequired();
            entity.HasMany(x => x.Students).WithOne(x => x.Grade)
                .HasForeignKey(x => x.Grade_idGrade).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(x => x.IdCourse);
            entity.Property(x => x.Name).HasMaxLength(45).IsUnicode(false).IsRequired();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(x => x.IdStudent);
            entity.Property(x => x.FirstName).HasMaxLength(45).IsUnicode(false).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(45).IsUnicode(false).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(45).IsUnicode(false).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(45).IsUnicode(false).IsRequired();
            entity.HasIndex(x => x.Email);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(x => x.IdEnrollment);
            entity.Property(x => x.Date).HasColumnType("date");
            entity.HasOne(x => x.Student).WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.Student_idStudent).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Course).WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.Course_idCourse).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
