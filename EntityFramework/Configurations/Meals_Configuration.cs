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
    public class Meals_Configuration : IEntityTypeConfiguration<Meals>
    {
        public void Configure(EntityTypeBuilder<Meals> builder)
        {
            builder.ToTable("Meals");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.HasOne(c => c.IngredientId_Navigation).WithMany(p => p.Meals).HasForeignKey(c => c.IngredientId);
        }
    }
}
