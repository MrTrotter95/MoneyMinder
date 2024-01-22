using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MoneyMinderWebAPI.DomainLayer.Models;

namespace MoneyMinderWebAPI.DataAccessLayer.Context;

public partial class MoneyMinderDataContext : DbContext
{
    public MoneyMinderDataContext()
    {
    }

    public MoneyMinderDataContext(DbContextOptions<MoneyMinderDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<AutomaticPayment> AutomaticPayments { get; set; }
    public virtual DbSet<AutomaticPaymentLog> AutomaticPaymentLog { get; set; }

    public virtual DbSet<HouseSaving> HouseSavings { get; set; }

    public virtual DbSet<PaymentFrequency> PaymentFrequencies { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionCategory> TransactionCategories { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=MoneyMinderData;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC27BA9D8EFB");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.FkAccountTypeId).HasColumnName("FK_AccountTypeID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.FkAccountType).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.FkAccountTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Account_AccountTypeID");
        });

        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountT__3214EC27522B8DBA");

            entity.ToTable("AccountType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AutomaticPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Automati__3214EC278300C151");

            entity.ToTable("AutomaticPayment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.FkAccountId).HasColumnName("FK_AccountID");
            entity.Property(e => e.FkCategoryId).HasColumnName("FK_CategoryID");
            entity.Property(e => e.FkFrequencyId).HasColumnName("FK_FrequencyID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.LastPaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkAccount).WithMany(p => p.AutomaticPayments)
                .HasForeignKey(d => d.FkAccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Automatic_Payment_AccountID");

            entity.HasOne(d => d.FkCategory).WithMany(p => p.AutomaticPayments)
                .HasForeignKey(d => d.FkCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Automatic_Payment_CategoryID");

            entity.HasOne(d => d.FkFrequency).WithMany(p => p.AutomaticPayments)
                .HasForeignKey(d => d.FkFrequencyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Automatic_Payment_FrequencyID");
        });

        modelBuilder.Entity<HouseSaving>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HouseSav__3214EC273F4DCF4A");

            entity.ToTable("HouseSaving");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.FkAccountId).HasColumnName("FK_AccountID");
            entity.Property(e => e.GoalAmount).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.GoalDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkAccount).WithMany(p => p.HouseSavings)
                .HasForeignKey(d => d.FkAccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_HouseSaving_AccountID");
        });

        modelBuilder.Entity<PaymentFrequency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentF__3214EC279C0A526C");

            entity.ToTable("PaymentFrequency");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC278CDE2116");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Balance).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.FkAccountId).HasColumnName("FK_AccountID");
            entity.Property(e => e.FkTransactionCategoryId).HasColumnName("FK_TransactionCategoryID");
            entity.Property(e => e.FkTransactionTypeId).HasColumnName("FK_TransactionTypeID");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");

            entity.HasOne(d => d.FkAccount).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.FkAccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Transaction_Account");

            entity.HasOne(d => d.FkTransactionCategory).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.FkTransactionCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Transaction_TransactionCategory");

            entity.HasOne(d => d.FkTransactionType).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.FkTransactionTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Transaction_TransactionType");
        });

        modelBuilder.Entity<TransactionCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC274C7960D3");

            entity.ToTable("TransactionCategory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TransactionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC272AEE813A");

            entity.ToTable("TransactionType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
