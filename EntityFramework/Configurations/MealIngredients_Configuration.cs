using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class MealIngredients_Configuration : IEntityTypeConfiguration<MealIngredients>
    {
        public void Configure(EntityTypeBuilder<MealIngredients> builder)
        {
            builder.ToTable("MealIngredients");
        }
    }
}
