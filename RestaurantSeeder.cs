using RestaurantAPI.Entities;

namespace RestaurantAPI;

public class RestaurantSeeder
{
    private readonly RestaurantDbContext _dbContext;

    public RestaurantSeeder(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Seed()
    {
        if (!_dbContext.Database.CanConnect())
            return;

        if (!_dbContext.Restaurants.Any())
        {
            var restaurants = GetRestaurants();
            _dbContext.Restaurants.AddRange(restaurants);
            _dbContext.SaveChanges();
        }
            
        if (!_dbContext.Roles.Any())
        {
            var roles = GetRoles();
            _dbContext.Roles.AddRange(roles);
            _dbContext.SaveChanges();
        }
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        var restaurants = new List<Restaurant>
        {
            new Restaurant
            {
                Name = "KFC",
                Category = "Fast Food",
                Description = "KFC is an American fast food restaurant",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,

                Dishes = new List<Dish>
                {
                    new Dish { Name = "Nashville Hot Chicken", Price = 10.30M },
                    new Dish { Name = "Chicken Nuggets", Price = 5.30M }
                },

                Address = new Address
                {
                    City = "Kraków",
                    Street = "Długa 5",
                    PostalCode = "30-001"
                }
            },

            new Restaurant
            {
                Name = "McDonald's",
                Category = "Fast Food",
                Description = "McDonald's is an American fast food restaurant",
                ContactEmail = "contact@mcdonalds.com",
                HasDelivery = true,

                Dishes = new List<Dish>
                {
                    new Dish { Name = "Big Mac", Price = 6.50M },
                    new Dish { Name = "2ForU", Price = 5.50M }
                },

                Address = new Address
                {
                    City = "Kraków",
                    Street = "Walerego Sławka 10",
                    PostalCode = "30-001"
                }
            }
        };

        return restaurants;
    }

    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>
        {
            new Role { Name = "User" },
            new Role { Name = "Manager" },
            new Role { Name = "Admin" }
        };

        return roles;
    }
}
