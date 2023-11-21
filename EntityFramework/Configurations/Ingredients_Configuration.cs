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
    public class Ingredients_Configuration : IEntityTypeConfiguration<Ingredients>
    {
        public void Configure(EntityTypeBuilder<Ingredients> builder)
        {
            builder.ToTable("Ingredients");
            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
        }
    }
}
