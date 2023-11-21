namespace CateringManagement.Common
{
    public static class TextUtils
    {
        public static string ConvertDateToString (DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }
        public static string ConvertDateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
