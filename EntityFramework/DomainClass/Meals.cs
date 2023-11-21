using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DomainClass
{
    public class Meals : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public string? Image { get; set; }

        public virtual ICollection<MealIngredients> MealIngredients { get; set; }

    }
}
