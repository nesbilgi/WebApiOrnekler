using Nes.RestApi.CSharp.Example.Model;
using RestSharp;
using System.Collections.Generic;

namespace Nes.RestApi.CSharp.Example.Services
{
    public class RequestService
    {
        public RestClient Client { get; set; }
        public RestRequest Request { get; set; }

        public RequestService()
        {
            Client = new RestClient(Nes.RestApi.CSharp.Example.Constant.BaseUrl);
        }

        public RestRequest SetHeaders(string apiPath, string token)
        {
            Request = new RestRequest();
            Request.Resource = apiPath;
            Request.AddHeader("Content-Type", "application/json");
            Request.AddHeader("Authorization", "bearer " + token);
            Request.RequestFormat = DataFormat.Json;
            return Request;
        }



        #region Authorization
        public GeneralResponse<TokenResponse> GetToken(TokenRequest model)
        {
            var client = new RestClient(Nes.RestApi.CSharp.Example.Constant.BaseUrl);
            var request = new RestRequest("/token", Method.POST);

            request.AddHeader("Content-Type", "application/json"); //istek data tipi

            request.AddParameter("grant_type", "password"); //auth servisi için sabit bu değerin kullanılması gerekmektedir.
            request.AddParameter("username", model.username); //kullanıcı adı
            request.AddParameter("password", model.password); //şifre

            var response = client.Execute<TokenResponse>(request);

            return new GeneralResponse<TokenResponse>()
            {
                HttpCode = response.StatusCode,
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

            var response = Client.Execute<List<AccountTemplateResponse>>(request);

            return new GeneralResponse<List<AccountTemplateResponse>>()
            {
                HttpCode = response.StatusCode,
                ErrorStatus = response.ErrorException != null ? new Status() { Code = (int)response.StatusCode, Message = response.ErrorException.Message } : null,
                Result = response.Data
            };
        }

        public GeneralResponse<string> GetTemplate(GetTemplateRequest model, string accessToken)
        {
            var request = SetHeaders("/account/getTemplate", accessToken);
            request.Method = Method.POST;
            request.AddBody(model);

            var response = Client.Execute<GeneralResponse<string>>(request);

            return new GeneralResponse<string>()
            {
                HttpCode = response.StatusCode,
                ErrorStatus = response.Data.ErrorStatus,
                Result = response.Data.Result
            };
        }

        #endregion
    }
}
