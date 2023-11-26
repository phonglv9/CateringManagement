using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using DAL.DomainClass;
using static System.Net.Mime.MediaTypeNames;

namespace CateringManagement.Helper
{
    public static class Commons
    {
        /// <summary>
        /// Tghoong tin người dùng khi đăng nhập vào,nếu chưa đăng nhập thì mặc định là ""
        /// </summary>
        public static string mylocalhost = "https://localhost:7009/api/";
        public static string email = "nhamvdph18699@fpt.edu.vn";
        public static string passemail = "14082002";
        public static Guid id = Guid.Empty;

        public static T? ConverObject<T>(object obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// Tạo mã ngẫu nhiên
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string valid = "QWERTYUIOPASDFGHJKLZXCVBNM0123456789";
            StringBuilder res = new StringBuilder();
            Random rd = new Random();
            while (0 < length--)  //Chạy đến khi đủ độ dài mật khẩu mong muốn
            {
                res.Append(valid[rd.Next(valid.Length)]); //Add thêm kí từ random trong valid
            }
            return res.ToString();
        }

        public static string GenerateRandomCode(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Convert sang string
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static string? NullToString(this object? @objValue)
        {
            return @objValue == null ? String.Empty : @objValue.ToString();
        }

        /// <summary>
        /// Convert to double
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static Double NullToDouble(this object? @objValue)
        {
            if (@objValue == null) return 0;
            //--
            _ = double.TryParse(objValue.NullToString(), out double _rs);
            return _rs;
        }

        /// <summary>
        /// Convert to double
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static Decimal NullToDecimal(this object @objValue)
        {
            _ = decimal.TryParse(objValue.NullToString(), out decimal _rs);
            return _rs;
        }

        /// <summary>
        /// Convert to int32
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static int NullToInt(this object @objValue)
        {
            Int32.TryParse(objValue.NullToString(), out int _rs);
            return _rs;
        }

        /// <summary>
        /// Convert to long
        /// </summary>
        /// <param name="vobjValue"></param>
        /// <returns></returns>
        public static long NullToLong(this object @objValue)
        {
            Int64.TryParse(objValue.NullToString(), out Int64 _rs);
            return _rs;
        }

        /// <summary>
        /// Kiểm tra định dạng Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseJson<T>(this string @this, out T result)
        {
            //--
            try
            {
                bool success = true;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore, //Bỏ qua những propertie NULL
                    Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                    MissingMemberHandling = MissingMemberHandling.Error
                };
                var _rs = JsonConvert.DeserializeObject<T>(@this, settings);
                if (_rs != null) result = _rs; else result = default;
                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<List<T>> GetAll<T>(string url)
        {
            try
            {
                List<T> objs = new List<T>();
                var Rest = new RestSharpHelper(url);
                var Response = await Rest.RequestBaseAsync(url, RestSharp.Method.Get);
                if (Response.StatusCode == HttpStatusCode.OK && !string.IsNullOrWhiteSpace(Response.Content))
                {
                    Response.Content.TryParseJson(out List<T> result);
                    objs = result;
                }
                Console.WriteLine(objs);
                return objs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static async Task<bool> Add_or_UpdateAsync<T>(this T obj, string url)
        {
            try
            {
                var Rest = new RestSharpHelper(url);
                var Response = await Rest.RequestBaseAsync(url, RestSharp.Method.Post, null, null, obj);
                return Response.IsSuccessful;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void setObjectAsString(this ISession session, string key, string value)
        {
            session.SetString(key, value);
        }

        public static string GetObjectFromString(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : value;
        }
        public static void setObjectAsJson(ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static List<T> GetListFromJsonData<T>(string path)
        {
            try
            {
                List<T> objects = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(path));
                return objects;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<T> GetListFromJsonData3<T>(string path)
        {
            try
            {
                List<T> objects = JsonConvert.DeserializeObject<List<T>>(path);
                return objects;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<T> GetListFromJsonData2<T>(string path)
        {
            try
            {
                using (StreamReader getfile = File.OpenText(path))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    List<T> objects = (List<T>)serializer.Deserialize(getfile, typeof(List<T>));
                    return objects;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetEnumDescription<T>(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }
            return value.ToString();
        }


        public static string SEOUrl(string url)
        {
            url = url.ToLower();
            url = Regex.Replace(url, @"[áàạảãâấầậẩẫăắằặẳẵ]", "a");
            url = Regex.Replace(url, @"[éèẹẻẽêếềệểễ]", "e");
            url = Regex.Replace(url, @"[óòọỏõôốồộổỗơớờợởỡ]", "o");
            url = Regex.Replace(url, @"[íìịỉĩ]", "i");
            url = Regex.Replace(url, @"[ýỳỵỉỹ]", "y");
            url = Regex.Replace(url, @"[úùụủũưứừựửữ]", "u");
            url = Regex.Replace(url, @"[đ]", "d");

            //2. Chỉ cho phép nhận:[0-9a-z-\s]
            url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim();
            //xử lý nhiều hơn 1 khoảng trắng --> 1 kt
            url = Regex.Replace(url.Trim(), @"\s+", "-");
            //thay khoảng trắng bằng -
            url = Regex.Replace(url, @"\s", "-");
            while (true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }
            return url;
        }

        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        public static async Task<string?> UploadFile(IFormFile file, string sDirectory)
        {
            try
            {
                int timestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                string extension = Path.GetExtension(file.FileName);
                var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{timestamp}{extension}";
                
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory);
                CreateIfMissing(path);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, fileName);

                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
                if (!supportedTypes.Contains(extension[1..].ToLower()))
                {
                    return null;
                }

                using (var stream = new FileStream(pathFile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch
            {
                return null;
            }
        }
    }
}
