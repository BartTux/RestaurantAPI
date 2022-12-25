using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Entities;

public class RestaurantDbContext : DbContext
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Dish> Dishes { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> dbContextOptions)
        : base(dbContextOptions) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(30);

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Description)
            .HasMaxLength(100);

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Category)
            .IsRequired()
            .HasMaxLength(20);

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.ContactEmail)
            .HasMaxLength(30);

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.ContactNumber)
            .IsRequired()
            .HasMaxLength(15);

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.AddressId)
            .IsRequired();

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.CreatedById)
            .IsRequired();


        modelBuilder.Entity<Dish>()
            .Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(30);

        modelBuilder.Entity<Dish>()
            .Property(d => d.Description)
            .HasMaxLength(100);

        modelBuilder.Entity<Dish>()
            .Property(d => d.Price)
            .IsRequired();

        modelBuilder.Entity<Dish>()
            .Property(d => d.RestaurantId)
            .IsRequired();


        modelBuilder.Entity<Address>()
            .Property(a => a.City)
            .IsRequired()
            .HasMaxLength(20);

        modelBuilder.Entity<Address>()
            .Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(30);

        modelBuilder.Entity<Address>()
            .Property(a => a.PostalCode)
            .IsRequired()
            .HasMaxLength(8);


        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(30);

        modelBuilder.Entity<User>()
            .Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(20);

        modelBuilder.Entity<User>()
            .Property(u => u.LastName)
            .HasMaxLength(30);

        modelBuilder.Entity<User>()
            .Property(u => u.Nationality)
            .HasMaxLength(20);

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.RoleId)
            .IsRequired();


        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(20);
    }
}
