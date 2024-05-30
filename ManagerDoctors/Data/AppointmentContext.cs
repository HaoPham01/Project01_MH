using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ManagerDoctors.Data;

public partial class AppointmentContext : DbContext
{
    public AppointmentContext()
    {
    }

    public AppointmentContext(DbContextOptions<AppointmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doctor> Doctors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-AIJ005TR;Initial Catalog=appointment;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
