using System;
using System.Collections.Generic;
using System.Net;
using static Nes.RestApi.CSharp.Example.Constant;

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

    #region EArchive
    public class InvoiceStatus
    {
        public string InvoiceStatusDescription { get; set; }
        public string InvoiceStatusDetailDescription { get; set; }
        public int? InvoiceStatusCode { get; set; }
    }
    public class MailSendInfo
    {
        public string ReceiverMail { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool IsSend { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsStatusCheck { get; set; }
        public bool IsDownload { get; set; }
        public bool IsView { get; set; }
        public string SendErrorDescription { get; set; }
    }

    public class SendMailResult
    {
        public bool ResultStatus { get; set; }
        public string ReceiverMail { get; set; }
        public string Description { get; set; }
    }
    #endregion

    #region EInvoiceStatus

    public class EInvoiceAnswerResult
    {
        public EInvoiceAnswerStatus Status { get; set; }
        public EInvoiceAnswerDetail Detail { get; set; }
    }

    public class EInvoiceAnswerStatus
    {
        public int? Code { get; set; }
        public string Description { get; set; }
        public string DetailDescription { get; set; }
    }

    public class EInvoiceAnswerDetail
    {
        public InvoiceAnswer InvoiceAnswer { get; set; }
        public string AnswerNote { get; set; }
        public DateTime AnswerDate { get; set; }
    }

    public class EInvoiceStatus
    {
        public int? Code { get; set; }
        public string Description { get; set; }
        public string DetailDescription { get; set; }
    }

    public class EInvoiceEnvelopeInfo
    {
        public string EnvelopeNumber { get; set; }
        public DateTime? EnvelopeDate { get; set; }
    }

    public class EInvoiceStatusResult
    {
        public string InvoiceProfile { get; set; }
        public DateTime InvoiceDate { get; set; }
        public EInvoiceAnswerResult Answer { get; set; }
        public EInvoiceStatus InvoiceStatus { get; set; }
        public EInvoiceEnvelopeInfo EnvelopeInfo { get; set; }
    }

    
    #endregion

    #region InvoiceGeneral

    

    public class GetInvoiceXmlResult
    {
        public string XmlContent { get; set; }
        public NESInvoiceType InvoiceType { get; set; }
    }
    #endregion
}
