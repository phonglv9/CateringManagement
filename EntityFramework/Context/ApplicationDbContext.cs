using DAL.Configurations;
using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=HP\SQLEXPRESS;Initial Catalog=CateringManagement;Integrated Security=True");
            //optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CateringManagement;Integrated Security=True");

        }

        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<AppRoles> AppRoles { get; set; }
        public DbSet<IngredientImports> IngredientImports { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }
        public DbSet<MealIngredients> MealIngredients { get; set; }
        public DbSet<Meals> Meals { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUsers_Configuration());
            modelBuilder.ApplyConfiguration(new AppRoles_Configuration());
            modelBuilder.ApplyConfiguration(new IngredientImports_Configuration());
            modelBuilder.ApplyConfiguration(new Ingredients_Configuration());
            modelBuilder.ApplyConfiguration(new MealIngredients_Configuration());
            modelBuilder.ApplyConfiguration(new Meals_Configuration());
            modelBuilder.ApplyConfiguration(new Orders_Configuration());
            modelBuilder.ApplyConfiguration(new OrderDetail_Configuration());
            modelBuilder.ApplyConfiguration(new Users_Configuration());
        }

    }
}
