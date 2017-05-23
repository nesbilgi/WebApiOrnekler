using Nes.RestApi.CSharp.Example.Model;
using Nes.RestApi.CSharp.Example.Services;
using System.IO;

namespace Nes.RestApi.CSharp.Example
{
    class Program
    {
        public static RequestService requestService;
        public static string accessToken = "BzzTUJTZRspA3I2UQcTIoTfO2rm_g80rCrP1PXnBaAdNBtIzQpoGQMcAx4WwK36kq3tRTV-pqdTI8NWmQMfXCS_8Aw7YzUgd7uhwf7X8_ZoMM56TJlkgfP-TsxeJKXxcRVsQ1t8bEdVgyb6jE6cj88j4mhklK_W6kZ2qmlRrolQCDrDpjifwp2Rw9fWmRymEN6CAqGD1H6Ok0SOr-REKS4B2Yb76kzjzib4RCXT775NH-rZwIxKrWRAGXOMiqUTzLBG8uJX6N_xP-adC2JbUq39quzocBQ4bTbxoSbaAePw4tkfNU04Wqzc85TXt5A-PQKRDKQzzhmPMpQ24_P-AXKr6QsN64Fzv9kyf6q_rKpwspq2qwegVqgsdztIb5Phz87zZBP_f88_Tq6YgICAI-IqvJMuRSIoChcCMqkiVxt6UUPFbk1Oc-C0gAOiQVSitd1dBF_3mcKqN0cDyvtlBqAmzFMc82MV26G_cCDUGxnarROvsWvDRHF-a8lOwN791ShPxDaTBm0qo2sxPfMZjwpJgcZyIjnA-toQzTf9BMfDiOQFdO4327px6vH6QhZPOES8TAWVSGabcypdm3L9lYAQ2rtRrgH9efcTXVC_1U_qUYY6DWG4e0yjRpzzgCH2sGjN8EF10U431z_zilPqyHQ";
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

            var downloadZip = requestService.GetAllCustomerByZIP(accessToken);
            File.WriteAllBytes(@"D:\CustomerList.zip", downloadZip.Result);
            #endregion
        }
    }
}
