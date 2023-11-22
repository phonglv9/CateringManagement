using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using static DAL.Enums.CommonEnum;

namespace DAL.DomainClass
{
    public class Ingredients : BaseEntity // Bảng Nguyên Liệu
    {
        public string Name { get; set; }
        public UnitEnum Unit { get; set; }// đơn vị đo lường (ml,gram,kg,..)
        [Column(TypeName = "decimal(20, 0)")]
        public decimal PriceUnit { get; set; }
        public int Quantity { get; set; } = 0;
        [Column(TypeName = "decimal(20, 0)")]
        public decimal Price { get; set; } = 0;

        public string? Image { get; set; }


        public virtual ICollection<IngredientImports>? IngredientImports { get; set; }
    }
}
