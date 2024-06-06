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

        public virtual DbSet<DefOperator> DefOperator { get; set; }

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
            modelBuilder.Entity<DefOperator>(entity =>
            {
                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.ClientId)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Place)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Mob)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.Email)
                      .HasMaxLength(150)
                      .IsUnicode(false);

                entity.Property(e => e.TipKind)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Email)
                      .HasMaxLength(150)
                      .IsUnicode(false);

                entity.Property(e => e.CustomerId)
                      .IsUnicode(false);

                entity.Property(e => e.ConfirmedMail)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<PaymentLog>(entity =>
            {
                entity.Property(e => e.User)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PurchAmount)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Currency)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Req_rnd)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Req_hash)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OrgOrderId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TxnResult)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ErrorMessage)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ProcReturnCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.AuthCode)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.MaskedPan)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CardHolderName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseRnd)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ResponseHash)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PolicyType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PolicySubtype)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Policy)
                      .IsUnicode(false);

                entity.Property(e => e.Created)
                      .IsUnicode(false);

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PayerId)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Provider)
                    .HasMaxLength(250)
                    .IsUnicode(false);

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
