using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace folio.Models
{
    /* EPortfolioDB provides a data access layer (DAL) that bridges the game 
     * between the models and the database.
    */
    public partial class EPortfolioDB : DbContext
    {
        /* Models managed by EPortfolioDB */
        public virtual DbSet<Lecturer> Lecturer { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<SkillSet> SkillSet { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Suggestion> Suggestion { get; set; }
        // Unable to generate entity type for table 'dbo.StudentSkillSet'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ProjectMember'. Please see the warning messages.

        /* constructors */
        public EPortfolioDB() { }
        public EPortfolioDB(DbContextOptions<EPortfolioDB> options) : base(options) { }

        /* configure EPortfolioDB to connect to the database */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                /* TODO: inject connection string */
                throw new NotImplementedException("Connection String injection not implemented");
            }
        }
        
        /* configure mapping of models to database tables */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");
        
            // Lecturer model
            modelBuilder.Entity<Lecturer>(entity =>
            {
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
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.MentorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_MentorID");
            });

            // Suggestion Model
            modelBuilder.Entity<Suggestion>(entity =>
            {
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
                // On Lecturer deletion: delete this suggestion too (cascade)
                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.Suggestion)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Suggestion_LecturerID");

                // Foreign Key StudentID- One Student to Many Suggestions
                // On Student deletion: delete this suggestion too (cascade)
                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Suggestion)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Suggestion_StudentID");
            });

            // StudentSkillset Model
            modelBuilder.Entity<StudentSkillSet>((entity) => 
            {
                // Composite Primary Key - StudentId, SkillSetId
                entity.HasKey(e => new { e.StudentId, e.SkillSetId })
                    .ForSqlServerIsClustered(false);
                entity.Property(e => e.StudentId).HasColumnName("StudentID");
                entity.Property(e => e.SkillSetId).HasColumnName("SkillSetID");
                
                // Foreign Key StudentID - One Student to Many StudentSkillSets
                // On student deletion: delete this StudentSkillSets too
                entity.HasOne<Student>(studentSkillSet => studentSkillSet.Student)
                    .WithMany(student => student.StudentSkillSets)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(studentSkillSet => studentSkillSet.StudentId)
                    .HasConstraintName("FK_StudentSkillSet_StudentID");
                
                // Foreign Key SkillSetID - One SkillSet to Many StudentSkillSets
                // On student deletion: delete this StudentSkillSets too
                entity.HasOne<SkillSet>(studentSkillSet => studentSkillSet.SkillSet)
                    .WithMany(skillset => skillset.StudentSkillSets)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasForeignKey(studentSkillSet => studentSkillSet.SkillSetId);
                    .HasConstraintName("FK_StudentSkillSet_SkillSetID");
            });
        }
    }
}
