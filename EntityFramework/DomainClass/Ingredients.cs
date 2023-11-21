using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DomainClass
{
    public class Ingredients // Bảng Nguyên Liệu
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }// đơn vị đo lường (ml,gram,kg,..)
        [Column(TypeName = "decimal(20, 0)")]
        public decimal Price { get; set; }
        public int TotalUnit { get; set; }
        [Column(TypeName = "decimal(20, 0)")]
        public decimal TotalPrice { get; set; }


        public List<IngredientImports>? IngredientImports { get; set; }
        public List<Meals>? Meals { get; set; }
    }
}
