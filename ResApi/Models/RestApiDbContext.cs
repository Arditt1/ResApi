using Microsoft.EntityFrameworkCore;
using ResApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResApi.Models
{
    public partial class RestApiDbContext : DbContext
    {
        RestApiDbContext()
        {
        }

        RestApiDbContext(DbContextOptions<RestApiDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            //To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {

               // entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.ContactInfo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                //entity.Property(e => e.Orders)
                //      .HasMaxLength(50)
                //      .IsUnicode(false);

                //entity.Property(e => e.AssignedTables)
                //      .HasMaxLength(150)
                //      .IsUnicode(false);

                entity.Property(e => e.RoleID)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);
                    //.HasDefaultValueSql("((1))");


                //entity.Property(e => e.EmployeeID)
                //      .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsUnicode(false);
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
