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
    public class MealIngredients_Configuration : IEntityTypeConfiguration<MealIngredients>
    {
        public void Configure(EntityTypeBuilder<MealIngredients> builder)
        {
            builder.ToTable("MealIngredients");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.HasOne(c => c.MealId_Navigation).WithMany(p => p.MealIngredients).HasForeignKey(c => c.MealId);
        }
    }
}
