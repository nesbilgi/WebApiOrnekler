using Nes.RestApi.CSharp.Example.Model;
using RestSharp;
using RestSharp.Deserializers;

namespace Nes.RestApi.CSharp.Example
{
    public static class ResponseParser
    {
        public static GeneralResponse<T> Parse<T>(this IRestResponse response)
        {
            JsonDeserializer restcsharpDeserializer = new JsonDeserializer();

            var result = new GeneralResponse<T>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK) { return restcsharpDeserializer.Deserialize<GeneralResponse<T>>(response); }
            else
            {
                result = new GeneralResponse<T>() { Result = result.Result};

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    result.ErrorStatus = new Status() { Code = (int)response.StatusCode, Message = "API Istek Yolu Hatalı" };
                }
                else
                {
                    result.ErrorStatus = restcsharpDeserializer.Deserialize<Status>(response);
                    result.ErrorStatus.Code = (int)response.StatusCode;
                }
                return result;
            }
        }
    }
}