using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DomainClass
{
    public class MealIngredients
    {
        public Guid? Id { get; set; }
        public Guid? MealId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(20, 0)")]
        public decimal Price { get; set; }

        public Meals? MealId_Navigation { get; set; }

    }
}
