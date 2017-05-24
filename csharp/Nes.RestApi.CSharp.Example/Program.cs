using Ionic.Zip;
using Nes.RestApi.CSharp.Example.Model;
using Nes.RestApi.CSharp.Example.Services;
using System.Collections.Generic;
using System.IO;
using static Nes.RestApi.CSharp.Example.Constant;

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
            string eArchiveinvoiceUuid = "{eArchiveinvoiceUuid}";
            var documentStatus = requestService.GetDocumentStatus(eArchiveinvoiceUuid, accessToken);

            var getMailStatistic = requestService.GetMailStatistics(eArchiveinvoiceUuid, accessToken);

            var setInvoiceCancel = requestService.SetInvoiceCancel(eArchiveinvoiceUuid, accessToken);

            var sendMailRequest = new SendMailRequest()
            {
                InvoiceUUID = eArchiveinvoiceUuid,
                ReceiverMailList = new List<string>() { "alıcı-mail-adresi", "alıcı-mail-adresi-1", "alıcı-mail-adresi-2" }
            };

            var sendMailResponse = requestService.SendMail(sendMailRequest, accessToken);

            #endregion

            #region EInvoice
            var einvoiceUuid = "{einvoiceUuid}";
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
                RejectNote = "{red-yanıtı}"
            };

            var setInvoiceAnswer = requestService.SetInvoiceAnswer(setInvoiceAnswerRequest, accessToken);
            #endregion

            #region InvoiceGeneral
            string InvoiceGeneralUuid = "{InvoiceGeneralUuid}";
            var getUBLInvoiceContent = requestService.GetUBLInvoiceContent(InvoiceGeneralUuid, accessToken);

            var getInvoiceHtml = requestService.GetInvoiceHtml(InvoiceGeneralUuid, accessToken);

            var getInvoicePdf = requestService.GetInvoicePdf(InvoiceGeneralUuid, accessToken);
            File.WriteAllBytes(@"D:\Invoice.pdf", getInvoicePdf.Result);

            var getInvoiceNumberFromUUID = requestService.GetInvoiceNumberFromUUID(InvoiceGeneralUuid, accessToken);

            ZipFile zip = new ZipFile();
            var xmlFile = @"D:\e-FaturaPaketi\e-FaturaPaketi\xml\be770327-b0fe-4511-9ff7-15936bcd1c17.xml";
            zip.AddEntry(Path.GetFileName(xmlFile), File.ReadAllBytes(xmlFile));

            MemoryStream ms = new MemoryStream();
            zip.Save(ms);
            ms.Position = 0;

            byte[] data = ms.ToArray();

            var sendUBLInvoiceRequest = new
            {
                transferDocument = new NESTransferDocument()
                {
                    FileName = Path.GetFileNameWithoutExtension(xmlFile) + ".zip",
                    IsDirectSend = true,
                    UUID = InvoiceGeneralUuid,
                    ZIPBinaryDataArray = data
                },
                invoiceProfile = InvoiceProfile.TICARIFATURA,
                customerRegisterNumber = "6310694807",
                eInvoiceAlias = "urn:mail:defaultpk@nesbilgi.com.tr"
            };
            var sendUBLInvoice = requestService.SendUBLInvoice(sendUBLInvoiceRequest, accessToken);

            #endregion

        }
    }
}
