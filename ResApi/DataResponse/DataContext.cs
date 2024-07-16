using Microsoft.EntityFrameworkCore;

namespace ResApi.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CategoryMenu> CategoryMenus { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<MenuItem> MenuItems { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Table> Tables { get; set; } = null!;
        public virtual DbSet<TableWaiter> TableWaiters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryMenu>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Category__19093A2B9ED53588");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.ToTable("CategoryMenu");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Photo)
                    .HasMaxLength(400)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Username, "UQ__Employee__536C85E4257508E0")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.ContactInfo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("Status");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Surname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Employees__RoleI__3D5E1FD2");
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__MenuItems__Categ__440B1D61");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.OrderTime).HasColumnType("datetime");

                entity.Property(e => e.TableId).HasColumnName("TableID");

                entity.Property(e => e.WaiterId).HasColumnName("WaiterID");

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.TableId)
                    .HasConstraintName("FK__Orders__TableID__46E78A0C");

                entity.HasOne(d => d.Waiter)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.WaiterId)
                    .HasConstraintName("FK__Orders__WaiterID__47DBAE45");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");
                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.MenuItem)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.MenuItemId)
                    .HasConstraintName("FK__OrderDeta__MenuI__4CA06362");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__4BAC3F29");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Permission1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Permission");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Permissio__RoleI__398D8EEE");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TableWaiter>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.TableId).HasColumnName("TableID");

                entity.Property(e => e.WaiterId).HasColumnName("WaiterID");

                entity.HasOne(d => d.Table)
                    .WithMany(p => p.TableWaiters)
                    .HasForeignKey(d => d.TableId)
                    .HasConstraintName("FK__TableWait__Table__4F7CD00D");

                entity.HasOne(d => d.Waiter)
                    .WithMany(p => p.TableWaiters)
                    .HasForeignKey(d => d.WaiterId)
                    .HasConstraintName("FK__TableWait__Waite__5070F446");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
