using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TestSession.Models
{
    public partial class dbFPTSystem : DbContext
    {
        public dbFPTSystem()
            : base("name=dbFPTSystem")
        {
        }
        public virtual DbSet<AccountDB> AccountDBs { get; set; }
        public virtual DbSet<CategoryDB> CategoryDBs { get; set; }
        public virtual DbSet<CourseDB> CourseDBs { get; set; }
        public virtual DbSet<CourseDetailsDB> CourseDetailsDBs { get; set; }
        public virtual DbSet<InfoAccDB> InfoAccDBs { get; set; }
        public virtual DbSet<InfoDetailsDB> InfoDetailsDBs { get; set; }
        public virtual DbSet<PermitDB> PermitDBs { get; set; }
        public virtual DbSet<TopicDB> TopicDBs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountDB>()
                 .HasMany(e => e.InfoAccDBs)
                 .WithRequired(e => e.AccountDB)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<InfoDetailsDB>()
                .Property(e => e.date_birthday)
                .IsUnicode(false);

            modelBuilder.Entity<InfoDetailsDB>()
                .Property(e => e.telephone)
                .IsUnicode(false);

            modelBuilder.Entity<InfoDetailsDB>()
                .Property(e => e.toeic_score)
                .IsUnicode(false);

            modelBuilder.Entity<InfoDetailsDB>()
                .Property(e => e.ex_or_in)
                .IsUnicode(false);

            modelBuilder.Entity<InfoDetailsDB>()
                .HasMany(e => e.InfoAccDBs)
                .WithRequired(e => e.InfoDetailsDB)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PermitDB>()
                .HasMany(e => e.InfoAccDBs)
                .WithRequired(e => e.PermitDB)
                .WillCascadeOnDelete(false);
            //modelBuilder.Entity<AccountDB>()
            //    .HasMany(e => e.InfoAccDBs)
            //    .WithRequired(e => e.AccountDB)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<CategoryDB>()
            //    .HasMany(e => e.CourseDetailsDBs)
            //    .WithRequired(e => e.CategoryDB)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<CourseDB>()
            //    .HasMany(e => e.CourseDetailsDBs)
            //    .WithRequired(e => e.CourseDB)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<InfoDetailsDB>()
            //    .Property(e => e.date_birthday)
            //    .IsUnicode(false);

            //modelBuilder.Entity<InfoDetailsDB>()
            //    .Property(e => e.telephone)
            //    .IsUnicode(false);

            //modelBuilder.Entity<InfoDetailsDB>()
            //    .Property(e => e.toeic_score)
            //    .IsUnicode(false);

            //modelBuilder.Entity<InfoDetailsDB>()
            //    .Property(e => e.ex_or_in)
            //    .IsUnicode(false);

            //modelBuilder.Entity<InfoDetailsDB>()
            //    .HasMany(e => e.InfoAccDBs)
            //    .WithRequired(e => e.InfoDetailsDB)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<PermitDB>()
            //    .HasMany(e => e.InfoAccDBs)
            //    .WithRequired(e => e.PermitDB)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<TopicDB>()
            //    .HasMany(e => e.CourseDetailsDBs)
            //    .WithRequired(e => e.TopicDB)
            //    .WillCascadeOnDelete(false);
        }
    }
}
