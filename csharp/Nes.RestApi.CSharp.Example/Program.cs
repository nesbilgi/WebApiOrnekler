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
        static RestClient Client = null;
        static RestRequest Request = null;

        static void Main(string[] args)
        {
            Client = new RestClient(Nes.RestApi.CSharp.Example.Constant.BaseUrl);

            #region Token Alma İşlemi
            GetToken(new TokenRequest() { username = "{Kullanıcı-Adı}", password = "{Şifre}" });
            #endregion

            #region Account
            TemplateList(Constant.InvoiceType.eInvoice); // Hesabınızda tanımlı olan XSLT listesinin detaylarını almak için kullanılır. 
            GetTemplate(new GetTemplateRequest() { TemplateType = GetTemplateType.EInvoice, Title = "{Tasarım-Başlığı}" }); //Hesabınızda tanımlı olan XSLT yi almak için kullanılır.
            #endregion

            #region Customer
            Check("{vkn-tckn}"); //Bir firma/kişinin e-Fatura mükellefi olup olmadığını sorgulamak için kullanılır.
            GetAll(); //GIB'de kayıtlı bütün E-Fatura mükelleflerini çekmek için kullanılır.
            DownloadZip(); //GIB'de kayıtlı bütün E-Fatura mükelleflerini ZIP içerisinde XML olarak çekmek için kullanılır.
            #endregion

            #region EArchive
            GetDocumentStatus("{eArchiveinvoiceUuid}"); //E-Arşiv faturalarının durumlarını sorgulamak için kullanılır.
            GetMailStatistics("{eArchiveinvoiceUuid}"); //E-Arşiv faturalarının mail durumlarını sorgulamak için kullanılır.
            SetInvoiceCancel("{eArchiveinvoiceUuid}"); //E-Arşiv faturaların iptal edilmesi için kullanılır.

            var sendMailRequest = new SendMailRequest()
            {
                InvoiceUUID = "{eArchiveinvoiceUuid}",
                ReceiverMailList = new List<string>() { "alıcı-mail-adresi", "alıcı-mail-adresi-1", "alıcı-mail-adresi-2" }
            };
            SendMail(sendMailRequest); //E-Arşiv faturalarını mail olarak göndermek için kullanılır
            #endregion

            #region EInvoice

            SaleInvoiceStatus("{einvoiceUuid}"); //Giden E-Faturaların durumlarını sorgulamak için kullanılır.
            GetUnAnsweredInvoiceUUIDList(); //Firmanıza gelen ve cevap verilmemiş olan Ticari Faturaların ETTn listesini almak için kullanılır. Burada dönen listedeki ETTN ler üzerinden faturaya cevap verme işlemini gerçekleştirebilirsiniz.
            GetUnTransferredInvoiceUUIDList("{etiket/alias}"); //Firmanıza gelen ve içeriye aktarılmamış (ERP'ye Alınmamış) faturaların ETTN listesini almak için kullanılır. Etiket bilgisi opsiyoneldir. Detalar dokümantasyonda mevcuttur.
            SetInvoiceTransferred("{einvoiceUuid}"); //ERP'ye yazılmış olan Gelen E-Faturanın NESBilgi üzerinde Transfer Edildi olarak işaretlemek için kullanılır.

            var setInvoiceAnswerRequest = new SetInvoiceAnswer()
            {
                Answer = ServiceAnswer.Accepted,
                InvoiceUuid = "{einvoiceUuid}",
                IsDirectSend = true,
                RejectNote = "{red-yanıtı}"
            };
            SetInvoiceAnswer(setInvoiceAnswerRequest); //Gelen Ticari E-Faturaya cevap vermek için kullanılır.
            #endregion

            #region InvoiceGeneral
            GetUBLXmlContent("{InvoiceUUId}"); //Gelen/Giden E-Fatura veya E-Ariv Faturalarının UBL Xml içeriğini almak için kullanılır.
            GetInvoiceHtml("{InvoiceUUId}"); //Gelen/Giden E-Fatura veya E-Ariv faturalarının HTML formatında önizlemesini almak için kullanılır.
            GetInvoicePdf("{InvoiceUUId}"); //Gelen/Giden E-Fatura veya E-Arşiv Faturalarının PDF halini almak için kullanılır.
            GetInvoiceNumberFromUUID("{InvoiceUUId}"); //Gelen/Giden E-Fatura veya E-Arşiv faturalarının 16 Haneli fatura numarasını almak için kullanılır.


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
                    UUID = "be770327-b0fe-4511-9ff7-15936bcd1c17",
                    ZIPBinaryDataArray = data
                },
                invoiceProfile = InvoiceProfile.TICARIFATURA,
                customerRegisterNumber = "6310694807",
                eInvoiceAlias = "urn:mail:defaultpk@nesbilgi.com.tr"
            };
            SendUBLInvoice(sendUBLInvoiceRequest); //


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
                customerRegisterNumber = "6310694807",
                isDirectSend = true
            };
            SendNESInvoice(sendNESInvoiceRequest);
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
        public static void GetToken(TokenRequest model)
        {
            var request = new RestRequest("/token", Method.POST);
            request.AddHeader("Content-Type", "application/json"); //istek data tipi
            request.AddParameter("grant_type", "password"); //auth servisi için sabit bu değerin kullanılması gerekmektedir.
            request.AddParameter("username", model.username); //kullanıcı adı
            request.AddParameter("password", model.password); //şifre

            var response = Client.Execute<TokenResponse>(request);
            var result = new GeneralResponse<TokenResponse>()
            {
                ErrorStatus = response.ErrorException != null ? new Status() { Code = (int)response.StatusCode, Message = response.ErrorException.Message } : null,
                Result = response.Data
            };
        }
        public static void TemplateList(Constant.InvoiceType invoiceType)
        {
            var request = SetHeaders("/account/templateList");
            request.Method = Method.POST;
            request.AddBody(invoiceType);
            var response = Client.Execute(request);
            var result = response.Parse<List<AccountTemplateResponse>>();
        }
        public static void GetTemplate(GetTemplateRequest model)
        {
            var request = SetHeaders("/account/getTemplate", accessToken);
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            var result = response.Parse<string>();
        }
        public static void Check(string vknTckn)
        {
            var request = SetHeaders("/customer/check", accessToken);
            request.Method = Method.POST;
            request.AddBody(vknTckn);
            var response = Client.Execute(request);
            var result = response.Parse<CustomerCheckResponse>();
        }
        public static void GetAll()
        {
            var request = SetHeaders("/customer/getAll", accessToken);
            request.Method = Method.POST;
            var response = Client.Execute(request);
            var result = response.Parse<List<GlobalCustomer>>();
        }
        public static void DownloadZip()
        {
            var request = SetHeaders("/customer/downloadZip", "application/zip");
            request.Method = Method.GET;
            var response = Client.Execute(request);
            var responseData = response.Parse<byte[]>();
            //File.WriteAllBytes(@"D:\CustomerList.zip", responseData.Result); //Gelen data istenilen konuma yazdırılabilir.
        }
        public static void GetDocumentStatus(string invoiceUuid)
        {
            var request = SetHeaders("/earchive/getDocumentStatus");
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            var result = response.Parse<InvoiceStatus>();
        }
        public static void GetMailStatistics(string invoiceUuid)
        {
            var request = SetHeaders("/earchive/getMailStatistics");
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            var result = response.Parse<List<MailSendInfo>>();
        }
        public static void SetInvoiceCancel(string invoiceUuid)
        {
            var request = SetHeaders("/earchive/setInvoiceCancel");
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            var result = response.Parse<bool>();
        }
        public static void SendMail(SendMailRequest model)
        {
            var request = SetHeaders("/earchive/sendMail");
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            var result = response.Parse<List<SendMailResult>>();
        }
        public static void SaleInvoiceStatus(string invoiceUuid)
        {
            var request = SetHeaders("/einvoice/saleinvoicestatus");
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            var result = response.Parse<EInvoiceStatusResult>();
        }
        public static void GetUnAnsweredInvoiceUUIDList()
        {
            var request = SetHeaders("/einvoice/getUnAnsweredInvoiceUUIDList");
            request.Method = Method.POST;
            var response = Client.Execute(request);
            var result = response.Parse<List<string>>();
        }
        public static void GetUnTransferredInvoiceUUIDList(string accountAlias)
        {
            var request = SetHeaders("/einvoice/getUnTransferredInvoiceUUIDList");
            request.Method = Method.POST;
            request.AddBody(accountAlias); //opsiyonel
            var response = Client.Execute(request);
            var result = response.Parse<List<string>>();
        }
        public static void SetInvoiceTransferred(string invoiceUuid)
        {
            var request = SetHeaders("/einvoice/setInvoiceTransferred");
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            var result = response.Parse<bool>();
        }
        public static void SetInvoiceAnswer(SetInvoiceAnswer model)
        {
            var request = SetHeaders("/einvoice/setInvoiceAnswer");
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            var result = response.Parse<bool>();
        }
        public static void GetUBLXmlContent(string invoiceUuid)
        {
            var request = SetHeaders("/invoicegeneral/getUBLXmlContent");
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            var result = response.Parse<GetInvoiceXmlResult>();
        }
        public static void GetInvoiceHtml(string invoiceUuid)
        {
            var request = SetHeaders("/invoicegeneral/getInvoiceHtml");
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            var result = response.Parse<string>();
        }
        public static void GetInvoicePdf(string invoiceUuid)
        {
            var request = SetHeaders("/invoicegeneral/getInvoicePdf", "application/pdf");
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            var responseData = response.Parse<byte[]>();
            //File.WriteAllBytes(@"D:\Invoice.pdf", responseData.Result); ////Gelen data istenilen konuma yazdırılabilir.
        }
        public static void GetInvoiceNumberFromUUID(string invoiceUuid)
        {
            var request = SetHeaders("/invoicegeneral/getInvoiceNumberFromUUID");
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            var result = response.Parse<string>();
        }
        public static void SendUBLInvoice<T>(T model)
        {
            var request = SetHeaders("/invoicegeneral/sendUBLInvoice");
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            var result = response.Parse<bool>();
        }
        public static void SendNESInvoice<T>(T model)
        {
            var request = SetHeaders("/invoicegeneral/sendNESInvoice");
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            var result = response.Parse<bool>();
        }
        #endregion
    }
}
