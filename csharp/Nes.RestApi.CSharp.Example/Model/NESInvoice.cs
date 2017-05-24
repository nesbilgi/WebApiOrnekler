using System;
using System.Collections.Generic;
using static Nes.RestApi.CSharp.Example.Constant;

namespace Nes.RestApi.CSharp.Example.Model
{
    public class Tax
    {
        public string TaxCode { get; set; }
        public decimal Total { get; set; }
        public decimal Percent { get; set; }
        public string ReasonCode { get; set; }
    }
    public class NESInvoice
    {
        public InvoiceInfo InvoiceInfo { get; set; }
        public PartyInfo CompanyInfo { get; set; }
        public PartyInfo CustomerInfo { get; set; }
        public ExportCustomerInfo ExportCustomerInfo { get; set; }
        public TaxFreeInfo TaxFreeInfo { get; set; }
        public List<InvoiceLine> InvoiceLines { get; set; }
        public List<string> Notes { get; set; }
        public bool ISEArchiveInvoice { get; set; }
    }


    public class ExportCustomerInfo
    {
        public string PartyName { get; set; }
        public string PersonName { get; set; }
        public string PersonSurName { get; set; }
        public string CompanyID { get; set; }
        public string LegalRegistrationName { get; set; }
        public AddressInfo AddressInfo { get; set; }
    }

    public class TaxFreeInfo
    {
        public TouristInfo TouristInfo { get; set; }
        public TaxRepresentativeInfo TaxRepresentativeInfo { get; set; }

    }

    public class TouristInfo
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string CountryCode { get; set; }
        public string PassportNo { get; set; }
        public DateTime PassportDate { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public FinancialAccountInfo FinancialAccountInfo { get; set; }
    }

    public class FinancialAccountInfo
    {
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string ID { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentNote { get; set; }
    }

    public class TaxRepresentativeInfo
    {
        public string RegisterNumber { get; set; }
        public string Alias { get; set; }
        public AddressInfo Address { get; set; }
    }

    public class AdditioanlDocumentReference
    {
        public string ID { get; set; }
        public DateTime? IssueDate { get; set; }
        public string DocumentType { get; set; }
        public string DocumentTypeCode { get; set; }
        public Attachment Attachment { get; set; }
    }

    public class Attachment
    {
        public string Base64Data { get; set; }
        public string MimeCode { get; set; }
        public string FileName { get; set; }
    }

    public class TaxExemptionReasonInfo
    {
        public string KDVExemptionReasonCode { get; set; }
        public string OTVExemptionReasonCode { get; set; }
    }
    public class InvoiceInfo
    {
        public string UUID { get; set; } 
        public SendInvoiceType InvoiceType { get; set; }
        public string InvoiceSerieOrNumber { get; set; } 
        public DateTime IssueDate { get; set; }
        public DateTime? IssueTime { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public InvoiceProfile? InvoiceProfile { get; set; }
        public List<KeyValue> DespatchDocumentReference { get; set; }
        public KeyValue OrderReference { get; set; }
        public AdditioanlDocumentReference OrderReferenceDocument { get; set; }
        public List<AdditioanlDocumentReference> AdditionalDocumentReferences { get; set; }
        public TaxExemptionReasonInfo TaxExemptionReasonInfo { get; set; }
        public string XSLTTitle { get; set; }
        public string XSLTPath { get; set; }
        public string ERPRefNo { get; set; }
        public string ERPCustomerRefNo { get; set; }
        public EArchiveInfo EArchiveInfo { get; set; }
        public int? DraftInvoiceID { get; set; }
        public PaymentTerms PaymentTermsInfo { get; set; }
        public PaymentMeans PaymentMeansInfo { get; set; }
        public OKCInfo OKCInfo { get; set; }
        public List<ReturnInvoiceInfo> ReturnInvoiceInfo { get; set; }
    }

    public class ReturnInvoiceInfo
    {
        public string InvoiceNumber { get; set; }
        public DateTime IssueDate { get; set; }
    }

    public class PaymentTerms
    {
        public decimal Percent { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }

    public class PaymentMeans
    {
        public string Code { get; set; }
        public string ChannelCode { get; set; }
        public DateTime? DueDate { get; set; }
        public string PayeeFinancialAccountID { get; set; }
        public string Note { get; set; }
    }

    public class OKCInfo
    {
        public string ID { get; set; }
        public DateTime IssueDate { get; set; }
        public string Time { get; set; }
        public string ZNo { get; set; }
        public string EndPointID { get; set; }
        public string DocumentDescription { get; set; }
    }

    public class KeyValue
    {
        public DateTime IssueDate { get; set; }
        public string Value { get; set; }
    }
    public class PartyInfo : AddressInfo
    {
        public string RegisterNumber { get; set; }
        public string Name { get; set; }


        public string TaxOffice { get; set; }
        public List<PartyIdentification> PartyIdentifications { get; set; }
        public List<PartyIdentification> AgentPartyIdentifications { get; set; }
        public string ReceiverAlias { get; set; }
    }

    public class AddressInfo
    {
        public string Address { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mail { get; set; }
        public string WebSite { get; set; }
    }
    public class PartyIdentification
    {
        public string SchemeID { get; set; }
        public string Value { get; set; }
    }
    public class InvoiceLine
    {
        public string Index { get; set; }
        public string SellerCode { get; set; }
        public string BuyerCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string UnitType { get; set; }
        public decimal Price { get; set; }
        public decimal AllowanceTotal { get; set; }
        public decimal KDVPercent { get; set; }
        public List<Tax> Taxes { get; set; }
        public string ManufacturerCode { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string Note { get; set; }
        public DeliveryInfo DeliveryInfo { get; set; }
    }

    public class DeliveryInfo
    {
        public string GTIPNo { get; set; }
        public string DeliveryTermCode { get; set; }
        public string TransportModeCode { get; set; }
        public string PackageBrandName { get; set; }
        public string ProductTraceID { get; set; }
        public string PackageID { get; set; }
        public decimal PackageQuantity { get; set; }
        public string PackageTypeCode { get; set; }
        public AddressInfo DeliveryAddress { get; set; }
    }

    public class EArchiveInfo
    {
        public SalesPlatform SalesPlatform { get; set; }
        public SendType SendType { get; set; }
        public bool ISDespatch { get; set; }
        public InternetInfo InternetInfo { get; set; }
    }
    public class InternetInfo
    {
        public string WebSite { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string TransporterName { get; set; }
        public string TransporterRegisterNumber { get; set; }
        public DateTime? TransportDate { get; set; }
    }
}
