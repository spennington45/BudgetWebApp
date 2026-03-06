using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace budgetWebApp.Server.Models;

public partial class BudgetContext : DbContext
{
    public BudgetContext()
    {
    }

    public BudgetContext(DbContextOptions<BudgetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<BudgetLineItem> BudgetLineItems { get; set; }

    public virtual DbSet<BudgetTotal> BudgetTotals { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<PlaidAccount> PlaidAccounts { get; set; }

    public virtual DbSet<PlaidItem> PlaidItems { get; set; }

    public virtual DbSet<RecurringExpense> RecurringExpenses { get; set; }

    public virtual DbSet<SourceType> SourceTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Budget;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.BudgetId).HasName("PK__Budget__E38E7924F1B4A737");

            entity.ToTable("Budget");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Budget_User");
        });

        modelBuilder.Entity<BudgetLineItem>(entity =>
        {
            entity.HasKey(e => e.BudgetLineItemId).HasName("PK__BudgetLi__B7246DBE05255538");

            entity.ToTable("BudgetLineItem");

            entity.Property(e => e.Label)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Budget).WithMany(p => p.BudgetLineItems)
                .HasForeignKey(d => d.BudgetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BugetLineItem_Budget");

            entity.HasOne(d => d.Category).WithMany(p => p.BudgetLineItems)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BugetLineItem_Category");

            entity.HasOne(d => d.SourceType).WithMany(p => p.BudgetLineItems)
                .HasForeignKey(d => d.SourceTypeId)
                .HasConstraintName("FK_BudgetLineItem_SourceType");
        });

        modelBuilder.Entity<BudgetTotal>(entity =>
        {
            entity.HasKey(e => e.BudgetTotalId).HasName("PK__BudgetTo__B82C6DFD89C1F253");

            entity.ToTable("BudgetTotal");

            entity.Property(e => e.TotalValue).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.BudgetTotals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BudgetTotal_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B94C1DC93");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(150);
        });

        modelBuilder.Entity<PlaidAccount>(entity =>
        {
            entity.HasKey(e => e.PlaidAccountId).HasName("PK__PlaidAcc__F8772F1AAE6E49AF");

            entity.ToTable("PlaidAccount");

            entity.HasIndex(e => e.AccountId, "UQ_PlaidAccount_AccountId").IsUnique();

            entity.Property(e => e.AccountId).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Mask).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.OfficialName).HasMaxLength(200);
            entity.Property(e => e.Subtype).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.PlaidItem).WithMany(p => p.PlaidAccounts)
                .HasForeignKey(d => d.PlaidItemId)
                .HasConstraintName("FK_PlaidAccount_PlaidItem");
        });

        modelBuilder.Entity<PlaidItem>(entity =>
        {
            entity.HasKey(e => e.PlaidItemId).HasName("PK__PlaidIte__C0BAAEB82D641F31");

            entity.ToTable("PlaidItem");

            entity.HasIndex(e => e.ItemId, "UQ_PlaidItem_ItemId").IsUnique();

            entity.Property(e => e.AccessToken).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.InstitutionName).HasMaxLength(200);
            entity.Property(e => e.ItemId).HasMaxLength(200);

            entity.HasOne(d => d.User).WithMany(p => p.PlaidItems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_PlaidItem_User");
        });

        modelBuilder.Entity<RecurringExpense>(entity =>
        {
            entity.HasKey(e => e.RecurringExpenseId).HasName("PK__Recurrin__D07473416C2BE2DB");

            entity.ToTable("RecurringExpense");

            entity.Property(e => e.Label)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.RecurringExpenses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecurringExpenses_Category");

            entity.HasOne(d => d.SourceType).WithMany(p => p.RecurringExpenses)
                .HasForeignKey(d => d.SourceTypeId)
                .HasConstraintName("FK_RecurringExpenses_SourceType");

            entity.HasOne(d => d.User).WithMany(p => p.RecurringExpenses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecurringExpenses_User");
        });

        modelBuilder.Entity<SourceType>(entity =>
        {
            entity.HasKey(e => e.SourceTypeId).HasName("PK__SourceTy__7E17EC2F7CAF1BCA");

            entity.ToTable("SourceType");

            entity.Property(e => e.SourceName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CD4F6C9B6");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ_User_Email").IsUnique();

            entity.HasIndex(e => e.ExternalId, "UQ_User_ExternalId").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.ExternalId).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
