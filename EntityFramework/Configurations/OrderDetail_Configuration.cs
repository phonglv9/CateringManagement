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
            builder.HasKey(e => new { e.OrderId, e.MealId });
            builder.HasOne(d => d.OrderId_Navigation).WithMany(p => p.OrderDetails).HasForeignKey(d => d.OrderId);
            builder.HasOne(d => d.MealId_Navigation).WithMany(p => p.OrderDetails).HasForeignKey(d => d.MealId);
        }
    }
}
