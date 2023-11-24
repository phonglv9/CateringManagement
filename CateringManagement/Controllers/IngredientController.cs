using CateringManagement.Helper;
using CateringManagement.Models.DTO;
using CateringManagement.Models.Requests;
using CateringManagement.Repository;
using DAL.DomainClass;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CateringManagement.Controllers
{
    [Authorize(Roles = "admin,chef,storage")]
    public class IngredientController : Controller
    {
        IngredientsRepository _ingredientsRepo = new IngredientsRepository();
        IngredientImportsRepository _ingredientImportsRepo = new IngredientImportsRepository();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetListIngredients()
        {
            var data = await _ingredientsRepo.GetAllData();
            return Json(data, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        public async Task<IActionResult> AddIngredient([FromBody] IngredientCreateRequest request)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            // validate
            if (string.IsNullOrEmpty(request.Name))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the name" });
            }
            if (request.PriceUnit <= 0)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Invalid unit price" });
            }

            var ingredient = new Ingredients
            {
                Name = request.Name,
                Unit = request.Unit,
                PriceUnit = request.PriceUnit,
                CreatedBy = userSesion.Id,
                UpdatedBy = userSesion.Id
            };

            try
            {
                await _ingredientsRepo.Create(ingredient);
                return Json(new ResponseModel { Status = 1, Mess = "Add ingredient successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Add ingredient failed" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetIngredientDetail(Guid id)
        {
            var ingredient = await _ingredientsRepo.GetByID(id);
            if (ingredient == null)
            {
                return Json(new ResponseModel<IngredientDetailDTO> { Status = 0, Mess = "Ingredient not found" });
            }

            var data = new IngredientDetailDTO
            {
                Id = id,
                Name = ingredient.Name,
                Unit = IngredientHelper.GetUnitName(ingredient.Unit),
                UnitPrice = ingredient.PriceUnit
            };
            return Json(new ResponseModel<IngredientDetailDTO> { Status = 1, Data = data });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateIngredient([FromBody] IngredientUpdateRequest request)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var ingredient = await _ingredientsRepo.GetByID(request.Id);
            if (ingredient == null)
            {
                return Json(new ResponseModel<IngredientDetailDTO> { Status = 0, Mess = "Ingredient not found" });
            }

            // validate
            if (string.IsNullOrEmpty(request.Name))
            {
                return Json(new ResponseModel { Status = 0, Mess = "Please enter the name" });
            }

            ingredient.Name = request.Name;
            ingredient.UpdatedBy = userSesion.Id;

            try
            {
                await _ingredientsRepo.Update(ingredient);
                return Json(new ResponseModel { Status = 1, Mess = "Update ingredient successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Update ingredient failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIngredient(Guid id)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var ingredient = await _ingredientsRepo.GetByID(id);
            if (ingredient == null)
            {
                return Json(new ResponseModel<IngredientDetailDTO> { Status = 0, Mess = "Ingredient not found" });
            }

            //if (ingredient.Quantity > 0)
            //{
            //    return Json(new ResponseModel<IngredientDetailDTO> { Status = 0, Mess = "You can not delete this ingredient" });
            //}

            ingredient.IsDeleted = 1;
            ingredient.UpdatedBy = userSesion.Id;

            try
            {
                await _ingredientsRepo.Update(ingredient);
                return Json(new ResponseModel { Status = 1, Mess = "Delete ingredient successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Delete ingredient failed" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSimpleListIngredients()
        {
            var list = await _ingredientsRepo.GetAllData();
            var data = list.Select(x => new IngredientNameDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return Json(new ResponseModel<List<IngredientNameDTO>> { Status = 1, Data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalPriceForImport(Guid id, int quantity)
        {
            var ingredient = await _ingredientsRepo.GetByID(id);
            if (ingredient == null)
            {
                return Json(new ResponseModel<IngredientDetailDTO> { Status = 0, Mess = "Ingredient not found" });
            }

            var totalPrice = ingredient.PriceUnit * quantity;
            return Json(new ResponseModel<decimal> { Status = 1, Data = totalPrice });
        }

        [HttpPost]
        public async Task<IActionResult> ImportIngredientToStorage([FromBody] ImportIngredientToStorageRequest request)
        {
            var userSesion = HttpContext.Session.GetObjectFromJson<Users>("userLogin");

            var ingredient = await _ingredientsRepo.GetByID(request.IngredientId);
            if (ingredient == null)
            {
                return Json(new ResponseModel<IngredientDetailDTO> { Status = 0, Mess = "Ingredient not found" });
            }

            if (request.Quantity <= 0)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Invalid quantity" });
            }
            if (request.TotalPrice <= 0)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Invalid price" });
            }

            ingredient.Quantity += request.Quantity;
            ingredient.Price += request.TotalPrice;
            ingredient.UpdatedBy = userSesion.Id;

            var ingredientImport = new IngredientImports
            {
                IngredientId = ingredient.Id,
                Quantity = request.Quantity,
                ExpiredDate = DateTime.Now,
                CreatedBy = userSesion.Id,
                UpdatedBy = userSesion.Id
            };

            try
            {
                await _ingredientImportsRepo.Create(ingredientImport);
                await _ingredientsRepo.Update(ingredient);
                return Json(new ResponseModel { Status = 1, Mess = "Import ingredient successfully" });
            }
            catch (Exception)
            {
                return Json(new ResponseModel { Status = 0, Mess = "Import ingredient failed" });
            }            
        }
    }
}
