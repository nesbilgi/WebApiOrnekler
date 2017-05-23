using Nes.RestApi.CSharp.Example.Model;
using Nes.RestApi.CSharp.Example.Services;

namespace Nes.RestApi.CSharp.Example
{
    class Program
    {
        public static RequestService requestService;
        public static string accessToken = string.Empty;
        static void Main(string[] args)
        {
            requestService = new RequestService();

            #region GetToken
            var requestModel = new TokenRequest() { username = "{Kullanıcı-Adı}", password = "{Şifre}" };
            var tokenResponse = requestService.GetToken(requestModel);

            if (tokenResponse.HttpCode == System.Net.HttpStatusCode.OK)
            {
                accessToken = tokenResponse.Result.access_token;
            }
            else
            {
                var message = tokenResponse.Result.error; //hatalı kullanıcı adı/şifre
            }
            #endregion

            #region Account

            #region GetTemplateList
            var templateListResponse = requestService.GetTemplateList(Constant.InvoiceType.eArchive, accessToken);
            if (templateListResponse.HttpCode == System.Net.HttpStatusCode.OK)
            {
                var result = templateListResponse.Result; //yanıt olarak gelen data
            }
            else
            {
                var message = tokenResponse.Result.error; //hata mesajı içeriği

            }
            #endregion

            #region GetTemplate
            var model = new GetTemplateRequest()
            {
                TemplateType = GetTemplateType.EInvoice,
                Title = "NESYeni"
            };

            var templateResponse = requestService.GetTemplate(model, accessToken);
            #endregion
            #endregion


        }
    }
}
