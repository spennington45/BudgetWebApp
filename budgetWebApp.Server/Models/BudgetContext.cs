﻿using System;
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
            entity.HasKey(e => e.BudgetId).HasName("PK__Budget__E38E792457DF958A");

            entity.ToTable("Budget");

            entity.Property(e => e.Date).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Budget_User");
        });

        modelBuilder.Entity<BudgetLineItem>(entity =>
        {
            entity.HasKey(e => e.BugetLineItemId).HasName("PK__BudgetLi__9C4FD84556BBE264");

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
            entity.HasKey(e => e.BudgetTotalId).HasName("PK__BudgetTo__B82C6DFD74088DC0");

            entity.ToTable("BudgetTotal");

            entity.Property(e => e.TotalValue).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.BudgetTotals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BudgetTotal_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0BB85B395C");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(150);
        });

        modelBuilder.Entity<RecurringExpense>(entity =>
        {
            entity.HasKey(e => e.RecurringExpenseId).HasName("PK__Recurrin__D0747341A39227A9");

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
            entity.HasKey(e => e.SourceTypeId).HasName("PK__SourceTy__7E17EC2F045BDE5E");

            entity.ToTable("SourceType");

            entity.Property(e => e.SourceName)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4CBEC3F0B5");

            entity.ToTable("User");

            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
