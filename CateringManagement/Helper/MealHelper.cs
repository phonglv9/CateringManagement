using static DAL.Enums.CommonEnum;

namespace CateringManagement.Helper
{
    public static class MealHelper
    {
        public static string GetMealImageSrc(string image)
        {
            return !string.IsNullOrEmpty(image) ? $"/images/meals/{image}" : "";
        }
    }
}
