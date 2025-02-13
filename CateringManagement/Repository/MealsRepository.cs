﻿using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Repository.Genneric;
using DAL.DomainClass;
using Microsoft.EntityFrameworkCore;

namespace CateringManagement.Repository
{
    public class MealsRepository : GenericRepository<Meals>
    {
        public async Task<List<MealDTO>> GetAllData()
        {
            var data = await db.Meals.Include(x => x.MealCategory).Where(x => x.IsDeleted == 0)
                .Select(x => new MealDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Category = x.MealCategory.Name,
                    Description = x.Description,
                    Image = MealHelper.GetMealImageSrc(x.Image)
                })
                .ToListAsync();

            return data;
        }

        public async Task<int> InsertMealIngredientList(List<MealIngredients> mealIngredients)
        {
            db.MealIngredients.AddRange(mealIngredients);
            return await db.SaveChangesAsync();
        }

        public async Task<int> DeleteMealIngredientList(List<MealIngredients> mealIngredients, Guid userId)
        {
            List<MealIngredients> dataUpdate = new();
            foreach (var item in mealIngredients)
            {
                item.IsDeleted = 1;
                item.UpdatedBy = userId;
                dataUpdate.Add(item);
            }
            db.MealIngredients.UpdateRange(dataUpdate);
            return await db.SaveChangesAsync();
        }
    }
}
