using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.PayJunctionApi
{
    public class TransactionResult
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("amountBase")]
        public decimal AmountBase { get; set; }

        [JsonProperty("amountTax")]
        public decimal AmountTax { get; set; }

        [JsonProperty("amountShipping")]
        public decimal AmountShipping { get; set; }

        [JsonProperty("amountTip")]
        public decimal AmountTip { get; set; }

        [JsonProperty("amountSurcharge")]
        public decimal AmountSurcharge { get; set; }

        [JsonProperty("amountTotal")]
        public decimal AmountTotal { get; set; }

        [JsonProperty("invoiceNumber")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("purchaseOrderNumber")]
        public string PurchaseOrderNumber { get; set; }

        [JsonProperty("signatureStatus")]
        public string SignatureStatus { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created")]
        public DateTime? Created { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("response")]
        public TransactionResponse Response { get; set; }

        [JsonProperty("settlement")]
        public TransactionSettlement Settlement { get; set; }

        [JsonProperty("vault")]
        public TransactionVault Vault { get; set; }

        [JsonProperty("billing")]
        public TransactionContact BillingContact { get; set; }

        [JsonProperty("shipping")]
        public TransactionContact ShippingContact { get; set; }

        public override string ToString()
        {
            return this.Action + ": " + this.AmountTotal.ToCurrency(true) + " (" + this.TransactionId + ")";
        }

        public class TransactionResponse
        {
            [JsonProperty("approved")]
            public bool Approved { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("processor")]
            public ResponseProcessor Processor { get; set; }

            public class ResponseProcessor
            {
                [JsonProperty("authorized")]
                public bool Authorized { get; set; }

                [JsonProperty("approvalCode")]
                public string ApprovalCode { get; set; }

                [JsonProperty("avs")]
                public ProcessorAVS AVS { get; set; }

                [JsonProperty("cvv")]
                public ProcessorCVV CVV { get; set; }

                public class ProcessorAVS
                {
                    [JsonProperty("status")]
                    public string Status { get; set; }

                    [JsonProperty("requested")]
                    public string Requested { get; set; }

                    [JsonProperty("match")]
                    public AVSMatch Match { get; set; }

                    public class AVSMatch
                    {
                        [JsonProperty("ZIP")]
                        public bool ZipCode { get; set; }

                        [JsonProperty("ADDRESS")]
                        public bool Address { get; set; }
                    }
                }

                public class ProcessorCVV
                {
                    [JsonProperty("status")]
                    public string Status { get; set; }
                }
            }
        }

        public class TransactionSettlement
        {
            [JsonProperty("settled")]
            public bool Settled { get; set; }
        }

        public class TransactionVault
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("accountType")]
            public string AccountType { get; set; }

            [JsonProperty("lastFour")]
            public string LastFour { get; set; }

            public override string ToString()
            {
                return String.Format("{0}: {1} ({2})", this.Type, this.AccountType, this.LastFour);
            }
        }

        public class TransactionContact
        {
            [JsonProperty("firstName")]
            public string FirstName { get; set; }

            [JsonProperty("middleName")]
            public string MiddleName { get; set; }

            [JsonProperty("lastName")]
            public string LastName { get; set; }

            [JsonProperty("companyName")]
            public string CompanyName { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("phone")]
            public string Phone1 { get; set; }

            [JsonProperty("phone2")]
            public string Phone2 { get; set; }

            [JsonProperty("jobTitle")]
            public string JobTitle { get; set; }

            [JsonProperty("identifier")]
            public string Identifier { get; set; }

            [JsonProperty("website")]
            public string Website { get; set; }

            [JsonProperty("address")]
            public ContactAddress Address { get; set; }

            public class ContactAddress
            {
                [JsonProperty("address")]
                public string Address { get; set; }

                [JsonProperty("city")]
                public string City { get; set; }

                [JsonProperty("state")]
                public string State { get; set; }

                [JsonProperty("country")]
                public string Country { get; set; }

                [JsonProperty("zip")]
                public string ZipCode { get; set; }
            }
        }
    }
}
