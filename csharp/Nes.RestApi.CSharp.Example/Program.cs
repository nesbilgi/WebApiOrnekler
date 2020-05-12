using Ionic.Zip;
using Nes.RestApi.CSharp.Example.Model;
using RestSharp;
using System.Collections.Generic;
using System.IO;
using static Nes.RestApi.CSharp.Example.Constant;

namespace Nes.RestApi.CSharp.Example
{
    class Program
    {
        public static string accessToken = string.Empty;
        public static string EInvoiceUUID = System.Guid.NewGuid().ToString(); // Örnek "198725EA-9E40-4503-996D-7D9ACD6B97AC";
        public static string EArchiveInvoiceUUID = System.Guid.NewGuid().ToString(); // Örnek "65BF1A86-A2E8-4475-9B7B-806409BBC277";
        public static string CustomerVknTckn = "1234567802";
        public static string CustomerAlias = "urn:mail:defaultpk@nesbilgi.com.tr";

        static RestClient Client = null;
        static RestRequest Request = null;

        static void Main(string[] args)
        {
            Client = new RestClient(Nes.RestApi.CSharp.Example.Constant.BaseUrl);

            #region Account
            var templateListResponse = TemplateListRequest(Constant.InvoiceType.eInvoice); // Hesabınızda tanımlı olan XSLT listesinin detaylarını almak için kullanılır. 
            var getTemplateResponse = GetTemplateRequest(new GetTemplateRequest() { TemplateType = GetTemplateType.EInvoice, Title = "TASARIM_BASLIGI" }); //Hesabınızda tanımlı olan XSLT yi almak için kullanılır.
            #endregion

            #region Customer
            var checkResponse = CheckRequest(CustomerVknTckn); //Bir firma/kişinin e-Fatura mükellefi olup olmadığını sorgulamak için kullanılır.
            var getAllResponse = GetAllRequest(); //GIB'de kayıtlı bütün E-Fatura mükelleflerini çekmek için kullanılır.
            var downloadZipResponse = DownloadZipRequest(); //GIB'de kayıtlı bütün E-Fatura mükelleflerini ZIP içerisinde XML olarak çekmek için kullanılır.
            #endregion

            #region EArchive
            var getDocumentStatusResponse = GetDocumentStatusRequest(EArchiveInvoiceUUID); //E-Arşiv faturalarının durumlarını sorgulamak için kullanılır.
            var getMailStatisticsResponse = GetMailStatisticsRequest(EArchiveInvoiceUUID); //E-Arşiv faturalarının mail durumlarını sorgulamak için kullanılır.
            var setInvoiceCancelResponse = SetInvoiceCancelRequest(EArchiveInvoiceUUID); //E-Arşiv faturaların iptal edilmesi için kullanılır.

            var sendMailRequest = new SendMailRequest()
            {
                InvoiceUUID = EArchiveInvoiceUUID,
                ReceiverMailList = new List<string>() { "ALICI_MAIL_ADRESI", "ALICI_MAIL_ADRESI_1", "ALICI_MAIL_ADRESI_2" }
            };
            var sendMailResponse = SendMailRequest(sendMailRequest); //E-Arşiv faturalarını mail olarak göndermek için kullanılır
            #endregion

            #region EInvoice

            var saleInvoiceStatusResponse = SaleInvoiceStatusRequest(EInvoiceUUID); //Giden E-Faturaların durumlarını sorgulamak için kullanılır.
            var getUnAnsweredInvoiceUUIDListResponse = GetUnAnsweredInvoiceUUIDListRequest(); //Firmanıza gelen ve cevap verilmemiş olan Ticari Faturaların ETTn listesini almak için kullanılır. Burada dönen listedeki ETTN ler üzerinden faturaya cevap verme işlemini gerçekleştirebilirsiniz.
            var getUnTransferredInvoiceUUIDListResponse = GetUnTransferredInvoiceUUIDListRequest(CustomerAlias); //Firmanıza gelen ve içeriye aktarılmamış (ERP'ye Alınmamış) faturaların ETTN listesini almak için kullanılır. Etiket bilgisi opsiyoneldir. Detalar dokümantasyonda mevcuttur.
            var setInvoiceTransferredResponse = SetInvoiceTransferredRequest(EInvoiceUUID); //ERP'ye yazılmış olan Gelen E-Faturanın NESBilgi üzerinde Transfer Edildi olarak işaretlemek için kullanılır.

            var setInvoiceAnswerRequest = new SetInvoiceAnswer()
            {
                Answer = ServiceAnswer.Accepted,
                InvoiceUuid = EInvoiceUUID,
                IsDirectSend = true,
                RejectNote = "RED_YANITI_GIRILIR"
            };
            var setInvoiceAnswerResponse = SetInvoiceAnswerRequest(setInvoiceAnswerRequest); //Gelen Ticari E-Faturaya cevap vermek için kullanılır.
            #endregion

            #region InvoiceGeneral
            var getUBLXmlContentResponse = GetUBLXmlContentRequest(EInvoiceUUID); //Gelen/Giden E-Fatura veya E-Ariv Faturalarının UBL Xml içeriğini almak için kullanılır.
            var getInvoiceHtmlRequestResponse = GetInvoiceHtmlRequest(EInvoiceUUID); //Gelen/Giden E-Fatura veya E-Ariv faturalarının HTML formatında önizlemesini almak için kullanılır.
            var getInvoicePdfResponse = GetInvoicePdfRequest(EInvoiceUUID); //Gelen/Giden E-Fatura veya E-Arşiv Faturalarının PDF halini almak için kullanılır.
            var GetInvoiceNumberFromUUIDResponse = GetInvoiceNumberFromUUIDRequest(EInvoiceUUID); //Gelen/Giden E-Fatura veya E-Arşiv faturalarının 16 Haneli fatura numarasını almak için kullanılır.


            #region SendUBLInvoice//UBL Xml Formatında E-Fatura Göndermek İçin Kullanılır
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
                    UUID = EInvoiceUUID,
                    ZIPBinaryDataArray = data
                },
                invoiceProfile = InvoiceProfile.TICARIFATURA,
                customerRegisterNumber = CustomerVknTckn,
                eInvoiceAlias = CustomerAlias
            };
            var sendUBLInvoiceResponse = SendUBLInvoiceRequest(sendUBLInvoiceRequest);
            #endregion

            #region Token Alma İşlemi
            var tokenResult = GetToken(new TokenRequest() { username = "test01@nesbilgi.com.tr", password = "V9zH7Hh55LIl" });
            if (tokenResult.ErrorStatus == null) { accessToken = tokenResult.Result.access_token; }
            #endregion


            #region SendNESInvoice//NESInvoice Formatında XML Göndermek İçin Kullanılır
            var invoice = InvoiceGenerator.GetStandarInvoice();
            //Fatura üzerine ek bilgiler eklemek için kullanabilirsiniz.
            invoice.InvoiceInfo.AdditionalDocumentReferences = new System.Collections.Generic.List<AdditioanlDocumentReference>() { };
            invoice.InvoiceInfo.AdditionalDocumentReferences.Add(new AdditioanlDocumentReference()
            {
                DocumentType = "BRANCH_NAME",
                ID = "Şube Adı"
            });
            //Özel değer
            invoice.InvoiceInfo.AdditionalDocumentReferences.Add(new AdditioanlDocumentReference()
            {
                DocumentType = "BRANCH_ADDRESS",
                ID = "Şube adresi"
            });
            var sendNESInvoiceRequest = new
            {
                nesInvoice = invoice,
                invoiceProfile = InvoiceProfile.TICARIFATURA,
                customerRegisterNumber = CustomerVknTckn,
                isDirectSend = true
            };
            var sendNESInvoiceResponse = SendNESInvoiceRequest(sendNESInvoiceRequest);
            #endregion
            #endregion
        }

        #region Metodlar
        public static RestRequest SetHeaders(string apiPath, string contentType = "application/json")
        {
            Request = new RestRequest();
            Request.Resource = apiPath;
            Request.AddHeader("Content-Type", contentType);
            Request.AddHeader("Authorization", "bearer " + accessToken);
            Request.RequestFormat = DataFormat.Json;
            return Request;
        }
        public static GeneralResponse<TokenResponse> GetToken(TokenRequest model)
        {
            var request = new RestRequest("/token", Method.POST);
            request.AddHeader("Content-Type", "application/json"); //istek data tipi
            request.AddParameter("grant_type", "password"); //auth servisi için sabit bu değerin kullanılması gerekmektedir.
            request.AddParameter("username", model.username); //kullanıcı adı
            request.AddParameter("password", model.password); //şifre

            var response = Client.Execute<TokenResponse>(request);
            return new GeneralResponse<TokenResponse>()
            {
                ErrorStatus = response.ErrorException != null ? new Status() { Code = (int)response.StatusCode, Message = response.ErrorException.Message } : null,
                Result = response.Data
            };
        }
        public static GeneralResponse<List<AccountTemplateResponse>> TemplateListRequest(Constant.InvoiceType invoiceType)
        {
            var request = SetHeaders($"/account/templateList/{invoiceType.ToString()}");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<List<AccountTemplateResponse>>();
        }
        public static GeneralResponse<string> GetTemplateRequest(GetTemplateRequest model)
        {
            var request = SetHeaders($"/account/downloadTemplate/{model.TemplateType.ToString()}/{model.Title}", accessToken);
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<string>();
        }
        public static GeneralResponse<CustomerCheckResponse> CheckRequest(string vknTckn)
        {
            var request = SetHeaders($"/customer/check/{vknTckn}", accessToken);
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<CustomerCheckResponse>();
        }
        public static GeneralResponse<List<GlobalCustomer>> GetAllRequest()
        {
            var request = SetHeaders("/customer/all", accessToken);
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<List<GlobalCustomer>>();
        }
        public static GeneralResponse<byte[]> DownloadZipRequest()
        {
            var request = SetHeaders("/customer/downloadZip", "application/zip");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            var responseData = response.Parse<byte[]>();
            //File.WriteAllBytes(@"D:\CustomerList.zip", responseData.Result); //Gelen data istenilen konuma yazdırılabilir.
            return responseData;
        }
        public static GeneralResponse<InvoiceStatus> GetDocumentStatusRequest(string invoiceUuid)
        {
            var request = SetHeaders($"/earchive/documentStatus/{invoiceUuid}");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<InvoiceStatus>();
        }
        public static GeneralResponse<List<MailSendInfo>> GetMailStatisticsRequest(string invoiceUuid)
        {
            var request = SetHeaders($"/earchive/mailStatistics/{invoiceUuid}");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<List<MailSendInfo>>();
        }
        public static GeneralResponse<bool> SetInvoiceCancelRequest(string invoiceUuid)
        {
            var request = SetHeaders($"/earchive/invoiceCancel/{invoiceUuid}");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<bool>();
        }
        public static GeneralResponse<List<SendMailResult>> SendMailRequest(SendMailRequest model)
        {
            var request = SetHeaders("/earchive/sendMail");
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            return response.Parse<List<SendMailResult>>();
        }
        public static GeneralResponse<EInvoiceStatusResult> SaleInvoiceStatusRequest(string invoiceUuid)
        {
            var request = SetHeaders($"/einvoice/saleinvoicestatus/{invoiceUuid}");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<EInvoiceStatusResult>();
        }
        public static GeneralResponse<List<string>> GetUnAnsweredInvoiceUUIDListRequest()
        {
            var request = SetHeaders("/einvoice/unAnsweredUUIDList");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<List<string>>();
        }
        public static GeneralResponse<List<string>> GetUnTransferredInvoiceUUIDListRequest(string accountAlias)
        {
            var url = "/einvoice/unTransferredUUIDList";
            if (!string.IsNullOrEmpty(accountAlias))
            {
                url += $"/{accountAlias}";
            }
            var request = SetHeaders(url);

            request.Method = Method.GET;

            var response = Client.Execute(request);
            return response.Parse<List<string>>();
        }
        public static GeneralResponse<bool> SetInvoiceTransferredRequest(string invoiceUuid)
        {
            var request = SetHeaders($"/einvoice/setTransferred/{invoiceUuid}");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            return response.Parse<bool>();
        }
        public static GeneralResponse<bool> SetInvoiceAnswerRequest(SetInvoiceAnswer model)
        {
            var request = SetHeaders("/einvoice/setAnswer");
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            return response.Parse<bool>();
        }
        public static GeneralResponse<byte[]> GetUBLXmlContentRequest(string invoiceUuid)
        {
            var request = SetHeaders($"/invoicegeneral/ublXmlContent/{invoiceUuid}", "application/xml");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            var responseData = response.Parse<byte[]>();
            string xmlContent = System.Text.Encoding.UTF8.GetString(responseData.Result);
            //File.WriteAllBytes(@"D:\Invoice.xml", responseData.Result); //Gelen data istenilen konuma yazdırılabilir.
            return responseData;
        }
        public static GeneralResponse<byte[]> GetInvoiceHtmlRequest(string invoiceUuid)
        {
            var request = SetHeaders($"/invoicegeneral/html/{invoiceUuid}", "text/html");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            var responseData = response.Parse<byte[]>();
            string htmlContent = System.Text.Encoding.UTF8.GetString(responseData.Result);
            //File.WriteAllBytes(@"D:\Invoice.html", responseData.Result); //Gelen data istenilen konuma yazdırılabilir.
            return responseData;
        }
        public static GeneralResponse<byte[]> GetInvoicePdfRequest(string invoiceUuid)
        {
            var request = SetHeaders($"/invoicegeneral/pdf/{invoiceUuid}", "application/pdf");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            var responseData = response.Parse<byte[]>();
            File.WriteAllBytes(@"D:\Invoice.pdf", responseData.Result); //Gelen data istenilen konuma yazdırılabilir.

            return responseData;
        }
        public static GeneralResponse<string> GetInvoiceNumberFromUUIDRequest(string invoiceUuid)
        {
            var request = SetHeaders($"/invoicegeneral/getInvoiceNumber/{invoiceUuid}");
            request.Method = Method.GET;
            var response = Client.Execute(request);

            return response.Parse<string>();
        }
        public static GeneralResponse<bool> SendUBLInvoiceRequest<T>(T model)
        {
            var request = SetHeaders("/invoicegeneral/sendUBLInvoice");
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            return response.Parse<bool>();
        }
        public static GeneralResponse<bool> SendNESInvoiceRequest<T>(T model)
        {
            var request = SetHeaders("/invoicegeneral/sendNESInvoice");
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            return response.Parse<bool>();
        }
        #endregion
    }
}
