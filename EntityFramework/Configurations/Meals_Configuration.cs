using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class Meals_Configuration : IEntityTypeConfiguration<Meals>
    {
        public void Configure(EntityTypeBuilder<Meals> builder)
        {
            builder.ToTable("Meals");
        }
    }
}
