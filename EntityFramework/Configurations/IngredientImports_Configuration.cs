using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class IngredientImports_Configuration : IEntityTypeConfiguration<IngredientImports>
    {
        public void Configure(EntityTypeBuilder<IngredientImports> builder)
        {
            builder.ToTable("IngredientImports");
            //builder.HasOne(c => c.Ingredient).WithMany(p => p.IngredientImports).HasForeignKey(c => c.IngredientId);
        }
    }
}
