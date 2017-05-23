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
            var requestModel = new TokenRequest() { username = "{Kullanıcı-Adı}", password = "Şifre" };
            var tokenResponse = requestService.GetToken(requestModel);
            #endregion

            #region Account
            var templateListResponse = requestService.GetTemplateList(Constant.InvoiceType.eInvoice, accessToken);

            var model = new GetTemplateRequest()
            {
                TemplateType = GetTemplateType.EInvoice,
                Title = "{Tasarım-Başlığı}"
            };
            var templateResponse = requestService.GetTemplate(model, accessToken);
            #endregion

            #region Customer
            string vknTckn = "{vkn-tckn}";
            var checkCustomer = requestService.Check(vknTckn, accessToken);

            var getAllCustomer = requestService.GetAllCustomerByList(accessToken);
            #endregion


        }
    }
}
