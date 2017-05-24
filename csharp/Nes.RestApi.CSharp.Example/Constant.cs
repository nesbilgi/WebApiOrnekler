namespace Nes.RestApi.CSharp.Example
{
    public  class Constant
    {
        public const string BaseUrl = "http://localhost:49271";

        public enum InvoiceType
        {
            eInvoice = 1,
            eArchive = 2,
        }

        public enum InvoiceProfile
        {
            TEMELFATURA = 0,
            TICARIFATURA = 1,
            IHRACAT = 2,
            YOLCUBERABERFATURA = 3,
            EARSIVFATURA = 4
        }

        public enum ServiceAnswer
        {
            Accepted = 2,
            Rejectted = 3
        }

        public enum GetTemplateType
        {
            EInvoice = 1,
            EArchive = 2
        }

        public enum InvoiceAnswer
        {
            None = 0,
            Wait = 1,
            Accepted = 2,
            Rejectted = 3
        }

        public enum NESInvoiceType
        {
            SaleInvoice = 1,
            PurchaseInvoice = 2,
            EArchiveInvoice = 3
        }
    }
}
