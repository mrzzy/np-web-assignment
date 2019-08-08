using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace folio.Models
{
    /* EPortfolioDB provides a data access layer (DAL) that bridges the game 
     * between the models and the database.
    */
    public partial class EPortfolioDB : DbContext
    {
        /* Models managed by EPortfolioDB */
        public virtual DbSet<Lecturer> Lecturers { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<SkillSet> SkillSets { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Suggestion> Suggestions { get; set; }
        public virtual DbSet<StudentSkillSet> StudentSkillSets { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }

        /* constructors */
        public EPortfolioDB() { }
        public EPortfolioDB(DbContextOptions<EPortfolioDB> options) : base(options) { }
    
        /* Database Context methods */
        /* configure EPortfolioDB to connect to the database */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(EPortfolioDB.ReadConnectionString());
            }
        }
        
        /* configure mapping of models to database tables */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");
        
            // Lecturer model
            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.ToTable("Lecturer");
                        
                // primary key - LecturerID
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

            // Project Model
            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                // Primary key - ProjectID
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

            // SkillSet Model
            modelBuilder.Entity<SkillSet>(entity =>
            {
                entity.ToTable("SkillSet");

                // Primary Key SkillSetID
                entity.HasKey(e => e.SkillSetId)
                    .ForSqlServerIsClustered(false);
                entity.Property(e => e.SkillSetId).HasColumnName("SkillSetID");

                entity.Property(e => e.SkillSetName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            // Student Model
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                // Primary Key StudentID
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

                // Foreign Key MentorID - One Mentor(Lecturer) to Many Students
                // On mentor(Lecturer) deletion: set to null
                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.MentorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_MentorID");
            });

            // Suggestion Model
            modelBuilder.Entity<Suggestion>(entity =>
            {
                entity.ToTable("Suggestion");
                
                // Primary Key SuggestionID
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

                // Foreign Key Lecturer - One Lecturer to Many Suggestions
                // On Lecturer deletion: set lecturer to null
                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.Suggestions)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Suggestion_LecturerID");

                // Foreign Key StudentID- One Student to Many Suggestions
                // On Student deletion: delete this suggestion too (cascade)
                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Suggestions)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Suggestion_StudentID");
            });

            // ProjectMember Model
            modelBuilder.Entity<ProjectMember>((entity) => 
            {
                entity.ToTable("ProjectMember");
                // Composite Primary Key - ProjectId, StudentId
                entity.HasKey(e => new { e.ProjectId, e.StudentId })
                    .ForSqlServerIsClustered(false);
                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");
                entity.Property(e => e.StudentId).HasColumnName("StudentID");
                
                // Foreign Key ProjectID - One Project to Many ProjectMembers
                // On project deletion: manually delete this ProjectMember too
                entity.HasOne<Project>(projectMember => projectMember.Project)
                    .WithMany(project => project.ProjectMembers)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(projectMember => projectMember.ProjectId)
                    .HasConstraintName("FK_ProjectMember_ProjectID");

                // Foreign Key StudentID - One Student to Many ProjectMembers
                // On student deletion: manually delete this ProjectMember too
                entity.HasOne<Student>(projectMember => projectMember.Member)
                    .WithMany(student => student.ProjectMembers)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(projectMember => projectMember.StudentId)
                    .HasConstraintName("FK_ProjectMember_StudentID");
            });
            
            // StudentSkillset Model
            modelBuilder.Entity<StudentSkillSet>((entity) => 
            {
                entity.ToTable("StudentSkillSet");

                // Composite Primary Key - StudentId, SkillSetId
                entity.HasKey(e => new { e.StudentId, e.SkillSetId })
                    .ForSqlServerIsClustered(false);
                entity.Property(e => e.StudentId).HasColumnName("StudentID");
                entity.Property(e => e.SkillSetId).HasColumnName("SkillSetID");
                
                // Foreign Key StudentID - One Student to Many StudentSkillSets
                // On student deletion: manually delete this StudentSkillSets too
                entity.HasOne<Student>(studentSkillSet => studentSkillSet.Student)
                    .WithMany(student => student.StudentSkillSets)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(studentSkillSet => studentSkillSet.StudentId)
                    .HasConstraintName("FK_StudentSkillSet_StudentID");
                
                // Foreign Key SkillSetID - One SkillSet to Many StudentSkillSets
                // On student deletion: manually delete this StudentSkillSet too
                entity.HasOne<SkillSet>(studentSkillSet => studentSkillSet.SkillSet)
                    .WithMany(skillset => skillset.StudentSkillSets)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(studentSkillSet => studentSkillSet.SkillSetId)
                    .HasConstraintName("FK_StudentSkillSet_SkillSetID");
            });
        }
        
        /* Utilties */
        /* Attempts to read and return the connection string:
         * - Tries to read the connection string from the environment variable
         *   DB_CONNECTION_STR 
         * - Otherwise attempts to read the connection string from appsettings.json
        */
        public static string ReadConnectionString()
        {
            // Attempt to read connection string environment variable
            String envConnectionStr = Environment.GetEnvironmentVariable(
                    "DB_CONNECTION_STR");
            if(!string.IsNullOrWhiteSpace(envConnectionStr)) return envConnectionStr;
            
            // Loads connection string from appsettings.json
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            
            IConfiguration configuration = builder.Build();
            string connectionStr = configuration.GetConnectionString(
                    "EPortfolioConnectionString");

            return connectionStr;
        }
    }
}
