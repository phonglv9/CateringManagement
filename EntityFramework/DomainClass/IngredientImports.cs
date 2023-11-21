using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DomainClass
{
    public class IngredientImports // Bảng Nhập Nguyên Liệu
    {
        public Guid? Id { get; set; }
        public Guid? IngredientId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiredDate { get; set; }


        public Ingredients? IngredientId_Navigation { get; set; }
    }
}
