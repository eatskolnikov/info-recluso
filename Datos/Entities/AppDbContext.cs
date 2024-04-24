using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Datos.Entities;

public partial class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Prisoner> Prisoners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Prisoner>(entity =>
        {
            entity.HasKey(e => e.PriKey).HasName("PK__Prisoner__0DAA475148A56F94");

            entity.HasIndex(e => e.Id, "UQ__Prisoner__3214EC06AB85DEBD").IsUnique();

            entity.Property(e => e.AdmissionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.ConvictionStatus)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Crimes)
                .IsRequired()
                .IsUnicode(false);
            entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Id)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Names)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Prison)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}