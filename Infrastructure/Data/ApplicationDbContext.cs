using Infrastructure.Data.Entities;
using Infrastructure.Data.Models;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<School> Schools { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialCategory> MaterialCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // School -> Professions
            builder.Entity<Profession>()
                .HasOne(p => p.School)
                .WithMany(s => s.Professions)
                .HasForeignKey(p => p.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            // Profession -> Materials
            builder.Entity<Material>()
                .HasOne(m => m.Profession)
                .WithMany(p => p.Materials)
                .HasForeignKey(m => m.ProfessionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Category -> Materials
            builder.Entity<Material>()
                .HasOne(m => m.MaterialCategory)
                .WithMany(c => c.Materials)
                .HasForeignKey(m => m.MaterialCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Teacher(User) -> Materials
            builder.Entity<Material>()
                .HasOne(m => m.Teacher)
                .WithMany(u => u.MaterialsCreated)
                .HasForeignKey(m => m.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Material -> Comments
            builder.Entity<Comment>()
                .HasOne(c => c.Material)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Comments
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment -> Replies
            builder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // QuizResult -> User
            builder.Entity<QuizResult>()
                .HasOne(q => q.User)
                .WithMany(u => u.QuizResults)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // QuizResult -> Profession (Recommended)
            builder.Entity<QuizResult>()
                .HasOne(q => q.RecommendedProfession)
                .WithMany()
                .HasForeignKey(q => q.RecommendedProfessionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.School)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.SchoolId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
