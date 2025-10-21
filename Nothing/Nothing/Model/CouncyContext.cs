using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Nothing.Model;

public partial class CouncyContext : DbContext
{
    public CouncyContext() { }

    public CouncyContext(DbContextOptions<CouncyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Commission> Commissions { get; set; }
    public virtual DbSet<CommissionChair> CommissionChairs { get; set; }
    public virtual DbSet<CommissionMeeting> CommissionMeetings { get; set; }
    public virtual DbSet<CommissionMembership> CommissionMemberships { get; set; }
    public virtual DbSet<DumaMember> DumaMembers { get; set; }
    public virtual DbSet<MeetingAttendee> MeetingAttendees { get; set; }
    public virtual DbSet<UserTable> UserTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=Councy.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Commission>(entity =>
        {
            entity.Property(e => e.CommissionId).HasColumnName("Commission_Id");
        });

        modelBuilder.Entity<CommissionChair>(entity =>
        {
            entity.HasKey(e => e.ChairsId);
            entity.Property(e => e.ChairsId).HasColumnName("Chairs_Id");
            entity.Property(e => e.CommissionId).HasColumnName("Commission_Id");
            entity.Property(e => e.MemberId).HasColumnName("Member_Id");

            entity.HasOne(d => d.Commission)
                  .WithMany(p => p.CommissionChairs)
                  .HasForeignKey(d => d.CommissionId);

            entity.HasOne(d => d.Member)
                  .WithMany(p => p.CommissionChairs)
                  .HasForeignKey(d => d.MemberId);
        });

        modelBuilder.Entity<CommissionMeeting>(entity =>
        {
            entity.HasKey(e => e.MeetingId);
            entity.Property(e => e.MeetingId).HasColumnName("Meeting_Id");
            entity.HasOne(d => d.Commission)
                  .WithMany(p => p.CommissionMeetings)
                  .HasForeignKey(d => d.CommissionId);
        });

        modelBuilder.Entity<CommissionMembership>(entity =>
        {
            entity.HasKey(e => e.ComisMemId);
            entity.Property(e => e.ComisMemId).HasColumnName("Comis_Mem_Id");
            entity.Property(e => e.CommissionId).HasColumnName("Commission_Id");
            entity.Property(e => e.MemberId).HasColumnName("Member_Id");

            entity.HasOne(d => d.Commission)
                  .WithMany(p => p.CommissionMemberships)
                  .HasForeignKey(d => d.CommissionId);

            entity.HasOne(d => d.Member)
                  .WithMany(p => p.CommissionMemberships)
                  .HasForeignKey(d => d.MemberId);
        });

        modelBuilder.Entity<MeetingAttendee>(entity =>
        {
            entity.HasKey(e => e.AttendeesId);
            entity.Property(e => e.AttendeesId).HasColumnName("Attendees_Id");
            entity.Property(e => e.RoleId).HasColumnName("column_name");
            entity.Property(e => e.MemberId).HasColumnName("Member_Id");

            entity.HasOne(d => d.Meeting)
                  .WithMany(p => p.MeetingAttendees)
                  .HasForeignKey(d => d.MeetingId);

            entity.HasOne(d => d.Member)
                  .WithMany(p => p.MeetingAttendees)
                  .HasForeignKey(d => d.MemberId);
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.ToTable("UserTable");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

   
            entity.Property(e => e.Login)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Password)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.HasIndex(e => e.Login)
                  .IsUnique();
        });

        // ДОБАВЬ ЭТО - начальный пользователь
        modelBuilder.Entity<UserTable>().HasData(
            new UserTable { UserId = 1, Login = "admin", Password = "admin" }
        );


        // modelBuilder.Entity<UserTable>().HasData(
        //     new UserTable { UserId = 1, Login = "admin", Password = "admin" }
        // );
    }
}

//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace Nothing.Model;

//public partial class CouncyContext : DbContext
//{
//    public CouncyContext() { }

//    public CouncyContext(DbContextOptions<CouncyContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Commission> Commissions { get; set; }
//    public virtual DbSet<CommissionChair> CommissionChairs { get; set; }
//    public virtual DbSet<CommissionMeeting> CommissionMeetings { get; set; }
//    public virtual DbSet<CommissionMembership> CommissionMemberships { get; set; }
//    public virtual DbSet<DumaMember> DumaMembers { get; set; }
//    public virtual DbSet<MeetingAttendee> MeetingAttendees { get; set; }
//    public virtual DbSet<UserTable> UserTables { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        => optionsBuilder.UseSqlite("Data Source=Councy.sqlite"); // упрощённый путь

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Commission>(entity =>
//        {
//            entity.Property(e => e.CommissionId).HasColumnName("Commission_Id");
//        });

//        modelBuilder.Entity<CommissionChair>(entity =>
//        {
//            entity.HasKey(e => e.ChairsId);
//            entity.Property(e => e.ChairsId).HasColumnName("Chairs_Id");
//            entity.Property(e => e.CommissionId).HasColumnName("Commission_Id");
//            entity.Property(e => e.MemberId).HasColumnName("Member_Id");

//            entity.HasOne(d => d.Commission)
//                  .WithMany(p => p.CommissionChairs)
//                  .HasForeignKey(d => d.CommissionId);

//            entity.HasOne(d => d.Member)
//                  .WithMany(p => p.CommissionChairs)
//                  .HasForeignKey(d => d.MemberId);
//        });

//        modelBuilder.Entity<CommissionMeeting>(entity =>
//        {
//            entity.HasKey(e => e.MeetingId);
//            entity.Property(e => e.MeetingId).HasColumnName("Meeting_Id");
//            entity.HasOne(d => d.Commission)
//                  .WithMany(p => p.CommissionMeetings)
//                  .HasForeignKey(d => d.CommissionId);
//        });

//        modelBuilder.Entity<CommissionMembership>(entity =>
//        {
//            entity.HasKey(e => e.ComisMemId);
//            entity.Property(e => e.ComisMemId).HasColumnName("Comis_Mem_Id");
//            entity.Property(e => e.CommissionId).HasColumnName("Commission_Id");
//            entity.Property(e => e.MemberId).HasColumnName("Member_Id");

//            entity.HasOne(d => d.Commission)
//                  .WithMany(p => p.CommissionMemberships)
//                  .HasForeignKey(d => d.CommissionId);

//            entity.HasOne(d => d.Member)
//                  .WithMany(p => p.CommissionMemberships)
//                  .HasForeignKey(d => d.MemberId);
//        });

//        modelBuilder.Entity<MeetingAttendee>(entity =>
//        {
//            entity.HasKey(e => e.AttendeesId);
//            entity.Property(e => e.AttendeesId).HasColumnName("Attendees_Id");
//            entity.Property(e => e.RoleId).HasColumnName("column_name");
//            entity.Property(e => e.MemberId).HasColumnName("Member_Id");

//            entity.HasOne(d => d.Meeting)
//                  .WithMany(p => p.MeetingAttendees)
//                  .HasForeignKey(d => d.MeetingId);

//            entity.HasOne(d => d.Member)
//                  .WithMany(p => p.MeetingAttendees)
//                  .HasForeignKey(d => d.MemberId);
//        });

//        modelBuilder.Entity<UserTable>(entity =>
//        {
//            entity.HasKey(e => e.UserId);
//            entity.ToTable("UserTable");
//            entity.Property(e => e.UserId).HasColumnName("User_Id");
//        });
//    }
//}