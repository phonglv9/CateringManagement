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
    public class OrderDetail_Configuration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");
            builder.HasKey(c => c.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("(newid())");
            builder.HasOne(c => c.MealId_Navigation).WithMany(p => p.OrderDetails).HasForeignKey(c => c.MealId);
            builder.HasOne(c => c.OrderId_Navigation).WithMany(p => p.OrderDetails).HasForeignKey(c => c.OrderId);

        }
    }
}
