using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DomainClass
{
    public class Meals
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid? IngredientId { get; set; }

        public Ingredients? IngredientId_Navigation { get; set; }
        public List<MealIngredients>? MealIngredients { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
