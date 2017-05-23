using System.Collections.Generic;

namespace Nes.RestApi.CSharp.Example.Model
{
    #region Token
    public class TokenRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    #endregion

    #region Account
    public class GetTemplateRequest
    {
        public string Title { get; set; }
        public GetTemplateType TemplateType { get; set; }
    }
    #endregion

    #region EArchive
    public class SendMailRequest
    {
        public string InvoiceUUID { get; set; }
        public List<string> ReceiverMailList { get; set; }
    }
    #endregion

    #region EInvoice
    public class SetInvoiceAnswer
    {
        public string InvoiceUuid { get; set; }
        public ServiceAnswer Answer { get; set; }
        public string RejectNote { get; set; }
        public bool IsDirectSend { get; set; }
    }
    public enum ServiceAnswer
    {
        Accepted = 2,
        Rejectted = 3
    }
    #endregion
}
