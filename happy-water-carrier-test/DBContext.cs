using System;
using happy_water_carrier_test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace happy_water_carrier_test
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderTag> OrderTags { get; set; }
        public virtual DbSet<Subdivision> Subdivisions { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("u0906946_default_user")
                .HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("employee_pk")
                    .IsClustered(false);

                entity.ToTable("employee");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("datetime")
                    .HasColumnName("date-of-birth");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("first-name");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("last-name");

                entity.Property(e => e.PatronymicName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("patronymic-name");

                entity.Property(e => e.SubdivisionId).HasColumnName("subdivision_id");

                entity.HasOne(d => d.Subdivision)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.SubdivisionId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("employee_subdivision_fk");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("order_pk")
                    .IsClustered(false);

                entity.ToTable("order");

                entity.HasIndex(e => e.Id, "order_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("product_name");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("order_employee_fk");
            });

            modelBuilder.Entity<OrderTag>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.TagId })
                    .HasName("order_tag_pk")
                    .IsClustered(false);

                entity.ToTable("order_tag");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderTags)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("order_tag_order_id_fk");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.OrderTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("order_tag_tag_id_fk");
            });

            modelBuilder.Entity<Subdivision>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("subdivision_pk")
                    .IsClustered(false);

                entity.ToTable("subdivision");

                entity.HasIndex(e => e.DirectorId, "subdivision_pk_2")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DirectorId).HasColumnName("director_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasOne(d => d.Director)
                    .WithOne(p => p.SubdivisionNavigation)
                    .HasForeignKey<Subdivision>(d => d.DirectorId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("subdivision_director_fk");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("tag_pk")
                    .IsClustered(false);

                entity.ToTable("tag");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
