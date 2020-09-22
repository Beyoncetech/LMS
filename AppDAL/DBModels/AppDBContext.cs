﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AppDAL.DBModels
{
    public partial class AppDBContext : DbContext
    {
        public AppDBContext()
        {
        }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appuser> Appuser { get; set; }
        public virtual DbSet<Tblmclassroom> Tblmclassroom { get; set; }
        public virtual DbSet<Tblmstandard> Tblmstandard { get; set; }
        public virtual DbSet<Tblmstudent> Tblmstudent { get; set; }
        public virtual DbSet<Tblmsubject> Tblmsubject { get; set; }
        public virtual DbSet<Tblmteacher> Tblmteacher { get; set; }
        public virtual DbSet<Tblrstudentclassroom> Tblrstudentclassroom { get; set; }
        public virtual DbSet<Tblrteacherclassroom> Tblrteacherclassroom { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Mdb123;database=lmsdb", x => x.ServerVersion("10.4.13-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appuser>(entity =>
            {
                entity.ToTable("appuser");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.CustomData)
                    .HasColumnType("varchar(4000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.IsPassReset)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Mobile)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ResetPassContext)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ResetPassValidity).HasColumnType("date");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.UserPerm)
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasColumnType("varchar(10)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Tblmclassroom>(entity =>
            {
                entity.ToTable("tblmclassroom");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasColumnType("varchar(250)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.RefId)
                    .IsRequired()
                    .HasColumnName("RefID")
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Scheduler)
                    .IsRequired()
                    .HasColumnType("varchar(2000)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.StandardId)
                    .HasColumnName("StandardID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SubjectID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Tblmstandard>(entity =>
            {
                entity.ToTable("tblmstandard");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Tblmstudent>(entity =>
            {
                entity.ToTable("tblmstudent");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ContactNo)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CreatedBy).HasColumnType("int(11)");

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.RegNo).HasColumnType("int(11)");

                entity.Property(e => e.StandardId)
                    .HasColumnName("StandardID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Tblmsubject>(entity =>
            {
                entity.ToTable("tblmsubject");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy).HasColumnType("int(11)");

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Tblmteacher>(entity =>
            {
                entity.ToTable("tblmteacher");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.ContactNo)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CreatdBy).HasColumnType("int(11)");

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.EducationalQualification)
                    .HasColumnType("varchar(500)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");
            });

            modelBuilder.Entity<Tblrstudentclassroom>(entity =>
            {
                entity.ToTable("tblrstudentclassroom");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClassRoomId)
                    .HasColumnName("ClassRoomID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClosedOn).HasColumnType("date");

                entity.Property(e => e.CreatedBy)
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.StudentId)
                    .HasColumnName("StudentID")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Tblrteacherclassroom>(entity =>
            {
                entity.ToTable("tblrteacherclassroom");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClassActivationThreshold).HasColumnType("int(11)");

                entity.Property(e => e.ClassRoomId)
                    .HasColumnName("ClassRoomID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClosedOn).HasColumnType("date");

                entity.Property(e => e.CreatedBy).HasColumnType("int(11)");

                entity.Property(e => e.CreatedOn).HasColumnType("date");

                entity.Property(e => e.TeacherId)
                    .HasColumnName("TeacherID")
                    .HasColumnType("int(11)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
