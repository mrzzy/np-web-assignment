﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace folio.Models
{
    public partial class EPortfolioDB : DbContext
    {
        public EPortfolioDB()
        {
        }

        public EPortfolioDB(DbContextOptions<EPortfolioDB> options)
            : base(options)
        {
        }

        public virtual DbSet<Lecturer> Lecturer { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<SkillSet> SkillSet { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Suggestion> Suggestion { get; set; }

        // Unable to generate entity type for table 'dbo.StudentSkillSet'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProjectMember'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                /* TODO: inject connection string */
                throw NotImplementedException("Connection String injection not implemented"):
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.HasKey(e => e.LecturerId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.LecturerId).HasColumnName("LecturerID");

                entity.Property(e => e.Description)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddr)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('p@55Lecturer')");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.Description)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectPoster)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectUrl)
                    .HasColumnName("ProjectURL")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SkillSet>(entity =>
            {
                entity.HasKey(e => e.SkillSetId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.SkillSetId).HasColumnName("SkillSetID");

                entity.Property(e => e.SkillSetName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.Achievement)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.Course)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddr)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalLink)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MentorId).HasColumnName("MentorID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('p@55Student')");

                entity.Property(e => e.Photo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.MentorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_MentorID");
            });

            modelBuilder.Entity<Suggestion>(entity =>
            {
                entity.HasKey(e => e.SuggestionId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.SuggestionId).HasColumnName("SuggestionID");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(3000)
                    .IsUnicode(false);

                entity.Property(e => e.LecturerId).HasColumnName("LecturerID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.Suggestion)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Suggestion_LecturerID");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Suggestion)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Suggestion_StudentID");
            });
        }
    }
}
