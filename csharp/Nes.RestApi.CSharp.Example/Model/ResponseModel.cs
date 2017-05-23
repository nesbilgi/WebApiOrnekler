using System.Net;

namespace Nes.RestApi.CSharp.Example.Model
{
    #region Servis Genel Dönüş Tipi
    public class Status
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class GeneralResponse<T>
    {
        public Status ErrorStatus { get; set; }
        public HttpStatusCode HttpCode { get; set; }
        public T Result { get; set; }
    }
    #endregion

    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string error { get; set; }
    }
}
