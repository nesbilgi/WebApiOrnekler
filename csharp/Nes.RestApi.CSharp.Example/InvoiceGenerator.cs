using Nes.RestApi.CSharp.Example.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nes.RestApi.CSharp.Example.Constant;

namespace Nes.RestApi.CSharp.Example
{
    public class InvoiceGenerator
    {
        public static NESInvoice GetStandarInvoice()
        {
            return new NESInvoice()
            {
                CompanyInfo = new PartyInfo()
                {
                    Address = "Barbaros Mah. Ak Zambak Sok. Uphill Towers A Blok K:16/92",
                    City = "İstanbul",
                    Country = "Türkiye",
                    District = "Ataşehir",
                    Fax = "216 688 51 99",
                    Mail = "info@nesbilgi.com.tr",
                    Name = "NES BİLGİ VERİ TEK. VE SAK. HİZ. A.Ş.",
                    Phone = "216 688 51 00",
                    RegisterNumber = "1234567801",
                    TaxOffice = "KOZYATAĞI",
                    WebSite = "http://nesbilgi.com.tr",
                },
                CustomerInfo = new PartyInfo()
                {
                    Address = "Barbaros Mah. Ak Zambak Sok. Uphill Towers A Blok K:16/92",
                    City = "İstanbul",
                    Country = "Türkiye",
                    District = "Ataşehir",
                    Fax = "216 688 51 99",
                    Mail = "info@nesbilgi.com.tr",
                    Name = "NES BİLGİ VERİ TEK. VE SAK. HİZ. A.Ş.",
                    Phone = "216 688 51 00",
                    RegisterNumber = "12345678902",
                    TaxOffice = "KOZYATAĞI",
                    WebSite = "http://nesbilgi.com.tr"
                },
                InvoiceLines = new System.Collections.Generic.List<InvoiceLine>()
                {
                    new InvoiceLine() {
                         Price=18,
                         Quantity=1M,
                         Name="Laptop",
                         UnitType="C62",
                         AllowanceTotal=0,
                         KDVPercent = 18,
                         Taxes = new System.Collections.Generic.List<Tax>()
                         {
                            new Tax() {  TaxCode="9040", Percent=20}
                         }
                    },
                },
                InvoiceInfo = new InvoiceInfo()
                {
                    CurrencyCode = "TRY",
                    IssueDate = DateTime.Now,
                    InvoiceProfile = InvoiceProfile.TEMELFATURA,
                    InvoiceType = SendInvoiceType.SATIS,
                    InvoiceSerieOrNumber = $"NES{DateTime.Now.Year}000000001", // NES2020000000001 (16 karakter)
                    UUID = Guid.NewGuid().ToString(),
                    IssueTime = DateTime.Now,
                    OrderReference = new KeyValue()
                    {
                        IssueDate = DateTime.Now,
                        Value = "SIP.001"
                    }
                }
            };
        }
    }
}
