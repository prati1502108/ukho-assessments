using Batch_Manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Batch_Manager.DatabaseContext
{
    public class BatchContext : DbContext
    {
        public BatchContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Batch> Batch { get; set; }
        public DbSet<BatchFile> File { get; set; }
        public DbSet<BatchAttribute> BatchAttribute { get; set; }
        public DbSet<Acl> Acl { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<FileAttribute> FileAttribute { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batch>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<BatchAttribute>()
                .HasOne(g => g.Batch)
                .WithMany(c => c.Attributes)
                .HasForeignKey(d => d.Id);
            //modelBuilder.Entity<BatchFile>()
            //    .HasOne(g => g.Batch)
            //    .WithMany(c => c.Files)
            //    .HasForeignKey(d => d.Id);
            modelBuilder.Entity<BatchFile>()
                .HasMany(g => g.Attributes)
                .WithOne(c => c.File)
                .HasForeignKey(d => d.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
