using Nes.RestApi.CSharp.Example.Model;
using Nes.RestApi.CSharp.Example.Services;

namespace Nes.RestApi.CSharp.Example
{
    class Program
    {
        public static RequestService requestService;
        public static string accessToken;
        static void Main(string[] args)
        {
            requestService = new RequestService();
            var requestModel = new TokenRequest() { username = "{Kullanıcı-Adı}", password = "{Şifre}" };
            var tokenResponse = requestService.GetToken(requestModel);

            if (tokenResponse.HttpCode == System.Net.HttpStatusCode.OK)
            {
                accessToken = tokenResponse.Result.access_token;
            }
            else
            {
                var message = tokenResponse.Result.error;//hatalı kullanıcı adı/şifre
            }

        }
    }
}
