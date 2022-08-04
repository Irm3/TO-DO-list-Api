using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TO_DO_list_Api.Models;
using Microsoft.Extensions.Configuration;

namespace TO_DO_list_Api.Database
{
    public partial class ToDoListDBContext : DbContext
    {
        public ToDoListDBContext()
        {
        }

        public ToDoListDBContext(DbContextOptions<ToDoListDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ToDoList> ToDoLists { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json");
                IConfiguration configuration = builder.Build();
                string connectionString = configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoList>(entity =>
            {
                entity.ToTable("ToDoList");

                entity.Property(e => e.TodoListId).HasColumnName("TodoList_id");

                entity.Property(e => e.FkUserId).HasColumnName("FK_User_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkUser)
                    .WithMany(p => p.ToDoLists)
                    .HasForeignKey(d => d.FkUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("User_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ResetToken)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ResetTokenExpiration)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'NULL'");


            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
