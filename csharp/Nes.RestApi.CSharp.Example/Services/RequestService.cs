using Nes.RestApi.CSharp.Example.Model;
using RestSharp;
namespace Nes.RestApi.CSharp.Example.Services
{
    public class RequestService
    {
        public GeneralResponse<TokenResponse> GetToken(TokenRequest model)
        {
            var client = new RestClient(Nes.RestApi.CSharp.Example.Constant.BaseUrl);
            var request = new RestRequest("/token", Method.POST);

            request.AddHeader("Content-Type", "application/json");//istek data tipi

            request.AddParameter("grant_type", "password");//auth servisi için sabit bu değerin kullanılması gerekmektedir.
            request.AddParameter("username", model.username);//kullanıcı adı
            request.AddParameter("password", model.password);//şifre


            var response = client.Execute<TokenResponse>(request);
            return new GeneralResponse<TokenResponse>()
            {
                HttpCode = response.StatusCode,
                ErrorStatus = response.ErrorException != null ? new Status() { Code = (int)response.StatusCode, Message = response.ErrorException.Message } : null,
                Result = response.Data 
            };
        }
    }

}
