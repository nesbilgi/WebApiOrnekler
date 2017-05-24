using System.Collections.Generic;
using static Nes.RestApi.CSharp.Example.Constant;

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
   
    #endregion

    #region InvoiceGeneral
    public class NESTransferDocument
    {
        public string UUID { get; set; }
        public string FileName { get; set; }
        public byte[] ZIPBinaryDataArray { get; set; }
        public bool IsDirectSend { get; set; }
    }

    public class CustomSendUBLInvoice
    {
        public NESTransferDocument TransferDocument { get; set; }
        public InvoiceProfile InvoiceProfile { get; set; }
        public string CustomerRegisterNumber { get; set; }
        public string eInvoiceAlias { get; set; }
    }
    #endregion
}
