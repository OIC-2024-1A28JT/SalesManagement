using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SalesManagement.Model;

public partial class SalesContext : DbContext
{
    public SalesContext()
    {
        Database.EnsureCreated();
    }

    public SalesContext(DbContextOptions<SalesContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<MDivision> MDivisions { get; set; }

    public virtual DbSet<MMessage> MMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["SalesContext"].ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MDivision>(entity =>
        {
            entity.HasKey(e => e.MDivisionId).HasName("PK__tmp_ms_x__4596EAAF3BCDF8B2");

            entity.ToTable("M_Division");

            entity.Property(e => e.MDivisionId).HasColumnName("M_DivisionID");
            entity.Property(e => e.Comments)
                .HasMaxLength(80)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DivisionName)
                .HasMaxLength(50)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.DspFlg).HasColumnName("DspFLG");
        });

        modelBuilder.Entity<MMessage>(entity =>
        {
            entity.HasKey(e => e.MsgId).HasName("PK__M_Messag__662358921AD33F80");

            entity.ToTable("M_Message");

            entity.Property(e => e.MsgId)
                .HasMaxLength(6)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasColumnName("MsgID");
            entity.Property(e => e.MsgComments).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.MsgTitle).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
