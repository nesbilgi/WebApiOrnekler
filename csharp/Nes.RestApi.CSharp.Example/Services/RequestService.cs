using Nes.RestApi.CSharp.Example.Model;
using RestSharp;
using RestSharp.Deserializers;
using System.Collections.Generic;

namespace Nes.RestApi.CSharp.Example.Services
{
    public class RequestService
    {
        public RestClient Client { get; set; }
        public RestRequest Request { get; set; }

        JsonDeserializer serializer;

        public RequestService()
        {
            Client = new RestClient(Nes.RestApi.CSharp.Example.Constant.BaseUrl);
            serializer = new JsonDeserializer();
        }

        public RestRequest SetHeaders(string apiPath, string accessToken, string contentType = "application/json")
        {
            Request = new RestRequest();
            Request.Resource = apiPath;
            Request.AddHeader("Content-Type", contentType);
            Request.AddHeader("Authorization", "bearer " + accessToken);
            Request.RequestFormat = DataFormat.Json;
            return Request;
        }

        #region Authorization
        public GeneralResponse<TokenResponse> GetToken(TokenRequest model)
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
        #endregion

        #region Account
        public GeneralResponse<List<AccountTemplateResponse>> GetTemplateList(Constant.InvoiceType invoiceType, string accessToken)
        {
            var request = SetHeaders("/account/templateList", accessToken);
            request.Method = Method.POST;
            request.AddBody(invoiceType);
            var response = Client.Execute(request);
            return response.Parse<List<AccountTemplateResponse>>();
        }
        public GeneralResponse<string> GetTemplate(GetTemplateRequest model, string accessToken)
        {
            var request = SetHeaders("/account/getTemplate", accessToken);
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            return response.Parse<string>();
        }
        #endregion

        #region Customer
        public GeneralResponse<CustomerCheckResponse> Check(string vknTckn, string accessToken)
        {
            var request = SetHeaders("/customer/check", accessToken);
            request.Method = Method.POST;
            request.AddBody(vknTckn);
            var response = Client.Execute(request);
            return response.Parse<CustomerCheckResponse>();
        }

        public GeneralResponse<List<GlobalCustomer>> GetAllCustomerByList(string accessToken)
        {
            var request = SetHeaders("/customer/getAll", accessToken);
            request.Method = Method.POST;
            var response = Client.Execute(request);
            return response.Parse<List<GlobalCustomer>>();
        }

        public GeneralResponse<byte[]> GetAllCustomerByZIP(string accessToken)
        {
            var request = SetHeaders("/customer/downloadZip", accessToken, "application/zip");
            request.Method = Method.GET;
            var response = Client.Execute(request);

            return response.Parse<byte[]>();
        }
        #endregion

        #region EArchive
        public GeneralResponse<InvoiceStatus> GetDocumentStatus(string invoiceUuid, string accessToken)
        {
            var request = SetHeaders("/earchive/getDocumentStatus", accessToken);
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            return response.Parse<InvoiceStatus>();
        }

        public GeneralResponse<List<MailSendInfo>> GetMailStatistics(string invoiceUuid, string accessToken)
        {
            var request = SetHeaders("/earchive/getMailStatistics", accessToken);
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            return response.Parse<List<MailSendInfo>>();
        }

        public GeneralResponse<bool> SetInvoiceCancel(string invoiceUuid, string accessToken)
        {
            var request = SetHeaders("/earchive/setInvoiceCancel", accessToken);
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            return response.Parse<bool>();
        }

        public GeneralResponse<List<SendMailResult>> SendMail(SendMailRequest model, string accessToken)
        {
            var request = SetHeaders("/earchive/sendMail", accessToken);
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            return response.Parse<List<SendMailResult>>();
        }
        #endregion

        #region EInvoice
        public GeneralResponse<EInvoiceStatusResult> EInvoiceGetDocumentStatus(string invoiceUuid, string accessToken)
        {
            var request = SetHeaders("/einvoice/saleinvoicestatus", accessToken);
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            return response.Parse<EInvoiceStatusResult>();
        }

        public GeneralResponse<List<string>> GetUnAnsweredInvoiceUUIDList(string accessToken)
        {
            var request = SetHeaders("/einvoice/getUnAnsweredInvoiceUUIDList", accessToken);
            request.Method = Method.POST;
            var response = Client.Execute(request);
            return response.Parse<List<string>>();
        }

        public GeneralResponse<List<string>> GetUnTransferredInvoiceUUIDByAlias(string accountAlias, string accessToken)
        {
            var request = SetHeaders("/einvoice/getUnTransferredInvoiceUUIDByAlias", accessToken);
            request.Method = Method.POST;
            request.AddBody(accountAlias);
            var response = Client.Execute(request);
            return response.Parse<List<string>>();
        }

        public GeneralResponse<List<string>> GetUnTransferredInvoiceUUIDList(string accessToken)
        {
            var request = SetHeaders("/einvoice/getUnTransferredInvoiceUUIDList", accessToken);
            request.Method = Method.POST;
            var response = Client.Execute(request);
            return response.Parse<List<string>>();
        }

        public GeneralResponse<bool> SetInvoiceTransferred(string invoiceUuid, string accessToken)
        {
            var request = SetHeaders("/einvoice/setInvoiceTransferred", accessToken);
            request.Method = Method.POST;
            request.AddBody(invoiceUuid);
            var response = Client.Execute(request);
            return response.Parse<bool>();
        }

        public GeneralResponse<bool> SetInvoiceAnswer(SetInvoiceAnswer model, string accessToken)
        {
            var request = SetHeaders("/einvoice/setInvoiceAnswer", accessToken);
            request.Method = Method.POST;
            request.AddBody(model);
            var response = Client.Execute(request);
            return response.Parse<bool>();
        }
        #endregion
    }
}
