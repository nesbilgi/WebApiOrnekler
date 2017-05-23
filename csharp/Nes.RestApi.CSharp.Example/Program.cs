using Nes.RestApi.CSharp.Example.Model;
using Nes.RestApi.CSharp.Example.Services;
using System.Collections.Generic;
using System.IO;

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

            var downloadZip = requestService.GetAllCustomerByZIP(accessToken);
            File.WriteAllBytes(@"D:\CustomerList.zip", downloadZip.Result);
            #endregion

            #region EArchive
            string invoiceUuid = "{invoiceUuid}";
            var documentStatus = requestService.GetDocumentStatus(invoiceUuid, accessToken);

            var getMailStatistic = requestService.GetMailStatistics(invoiceUuid, accessToken);

            var setInvoiceCancel = requestService.SetInvoiceCancel(invoiceUuid, accessToken);

            var sendMailRequest = new SendMailRequest()
            {
                InvoiceUUID = invoiceUuid,
                ReceiverMailList = new List<string>() { "alıcı-mail-adresi", "alıcı-mail-adresi-1", "alıcı-mail-adresi-2" }
            };

            var sendMailResponse = requestService.SendMail(sendMailRequest, accessToken);

            #endregion

            #region EInvoice
            var einvoiceUuid = "{einvoice-invoiceUuid}";
            var getDocumentStatus = requestService.GetDocumentStatus(einvoiceUuid, accessToken);

            var getUnAnsweredInvoiceUUIDList = requestService.GetUnAnsweredInvoiceUUIDList(accessToken);

            string alias = "urn:mail:defaultpk@nesbilgi.com.tr";
            var getUnTransferredInvoiceUUIDByAlias = requestService.GetUnTransferredInvoiceUUIDByAlias(alias, accessToken);

            var getUnTransferredInvoiceUUIDList = requestService.SetInvoiceTransferred(einvoiceUuid, accessToken);

            var setInvoiceAnswerRequest = new SetInvoiceAnswer()
            {
                Answer = ServiceAnswer.Accepted,
                InvoiceUuid = "{einvoiceUuid}",
                IsDirectSend = true,
                RejectNote = "TEST RESTAPI"
            };

            var setInvoiceAnswer = requestService.SetInvoiceAnswer(setInvoiceAnswerRequest, accessToken);
            #endregion
        }
    }
}
