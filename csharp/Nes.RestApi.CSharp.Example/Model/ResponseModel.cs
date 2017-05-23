using System;
using System.Collections.Generic;
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

    #region Account
    public class AccountTemplateResponse
    {
        public string Title { get; set; }
        public bool IsDefault { get; set; }
    }

    public enum GetTemplateType
    {
        EInvoice = 1,
        EArchive = 2
    }
    #endregion

    #region Customer
    public class GlobalCustomer
    {
        public string RegisterNumber { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Type { get; set; }
        public DateTime FirstCreationTime { get; set; }
        public DateTime AliasCreationTime { get; set; }
    }

    public class CustomerCheckResponse
    {
        public List<GlobalCustomer> CustomerList { get; set; }
        public bool ISEInvoiceCustomer { get; set; }
    }
    #endregion
}
