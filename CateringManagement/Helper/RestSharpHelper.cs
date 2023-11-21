using RestSharp.Authenticators;
using RestSharp;

namespace CateringManagement.Helper
{
    public class RestSharpHelper
    {
        RestClient Client { get; set; }
        public RestSharpHelper(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                ThrowOnAnyError = false,
                Timeout = 1000000
            };
            Client = new RestClient(options);
        }
        /// <summary>
        /// Send Reques tới các Server khác
        /// </summary>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <param name="post_data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<RestResponse> RequestBaseAsync(string SOURCE
            , Method METHOD = Method.Get //--Mặc định là Get
            , Dictionary<string, string> formdata = null
            , Dictionary<string, string> headers = null
            , object post_data = null
            , string token = null)
        {
            try
            {
                var request = new RestRequest(SOURCE, METHOD);
                //-- Co parameter (ap dung cho login hay request can truyen parameter)
                if (formdata != null && formdata.Count > 0)
                {
                    foreach (var parameter in formdata)
                    {
                        request.AddParameter(parameter.Key, parameter.Value);
                    }
                }
                //-- Nếu có header
                if (headers != null && headers.Count > 0)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                //-- Nếu có token thì thêm vào
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var authenticator = new JwtAuthenticator(token);
                    Client.Authenticator = authenticator;
                }
                //--Nếu có dữ liệu cần bắn lên
                if (post_data != null) request.AddJsonBody(post_data);
                // ... and use it like we used to
                var response = await Client.ExecuteAsync(request);
                //--
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
