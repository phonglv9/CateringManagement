using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class MealCategories_Configuration : IEntityTypeConfiguration<MealCategories>
    {
        public void Configure(EntityTypeBuilder<MealCategories> builder)
        {
            builder.ToTable("MealCategories");
        }
    }
}
