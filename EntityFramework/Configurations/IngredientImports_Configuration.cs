using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class IngredientImports_Configuration : IEntityTypeConfiguration<IngredientImports>
    {
        public void Configure(EntityTypeBuilder<IngredientImports> builder)
        {
            builder.ToTable("IngredientImports");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.HasOne(c => c.IngredientId_Navigation).WithMany(p => p.IngredientImports).HasForeignKey(c => c.IngredientId);
        }
    }
}
