using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Apis.PayJunction
{
    public class PayJunctionApiClient
    {
        public PayJunctionServer Server { get; private set; }

        private string ApiUsername { get; set; }

        private string ApiPassword { get; set; }

        private string ApplicationKey { get; set; }

        public bool ValidateTransactionResultSuccess { get; set; }

        private string ApiBaseUrl => this.Server == PayJunctionServer.Development ? 
            "https://api.payjunctionlabs.com" : "https://api.payjunction.com";

        public PayJunctionApiClient(
            PayJunctionServer server, 
            string apiUsername, 
            string apiPassword, 
            string applicationKey, 
            bool validateTransactionResultSuccess = true)
        {
            this.Server = server;
            this.ApiUsername = apiUsername;
            this.ApiPassword = apiPassword;
            this.ApplicationKey = applicationKey;
            this.ValidateTransactionResultSuccess = validateTransactionResultSuccess;
        }

        public async Task<ResultsCollection<Terminal>> GetTerminalsAsync()
        {
            var apiEndPoint = "/terminals";
            var response = await SendRequestAsync(apiEndPoint, "GET");
            var result = JsonConvert.DeserializeObject<ResultsCollection<Terminal>>(response);
            return result;
        }

        public async Task<ResultsCollection<SmartTerminal>> GetSmartTerminalsAsync()
        {
            var apiEndPoint = "/smartterminals";
            var response = await SendRequestAsync(apiEndPoint, "GET");
            var result = JsonConvert.DeserializeObject<ResultsCollection<SmartTerminal>>(response);
            return result;
        }

        public async Task<SmartTerminalPaymentRequestStatus> GetSmartTerminalPaymentRequestStatusAsync(
            string requestPaymentId)
        {
            var apiEndPoint = String.Format("/smartterminals/requests/{0}", requestPaymentId);
            var response = await SendRequestAsync(apiEndPoint, "GET");
            var result = JsonConvert.DeserializeObject<SmartTerminalPaymentRequestStatus>(response);
            return result;
        }

        public async Task SendSmartTerminalToMainScreenAsync(
            string smartTerminalId)
        {
            var apiEndPoint = String.Format("/smartterminals/{0}/main", smartTerminalId);
            var response = await SendRequestAsync(apiEndPoint, "POST");
        }

        public async Task<SmartTerminalPaymentRequest> RequestSmartTerminalPaymentAsync(
            string smartTerminalId,
            string destinationTerminalId, 
            decimal baseAmount, 
            decimal taxAmount, 
            bool showReceipt = false,
            string invoiceNumber = null, 
            string purchaseOrderNumber = null, 
            string note = null)
        {
            var requestPayload = new StringBuilder();
            requestPayload.AppendFormat("amountBase={0}", baseAmount.ToCurrency(addCommas: false));
            requestPayload.AppendFormat("&amountTax={0}", taxAmount.ToCurrency(addCommas: false));
            requestPayload.AppendFormat("&terminalId={0}", destinationTerminalId);
            requestPayload.AppendFormat("&showReceiptPrompt={0}", showReceipt.ToString().ToLower());

            if (!String.IsNullOrEmpty(invoiceNumber))
                requestPayload.AppendFormat("&invoiceNumber={0}", invoiceNumber);
            if (!String.IsNullOrEmpty(purchaseOrderNumber))
                requestPayload.AppendFormat("&purchaseOrderNumber={0}", purchaseOrderNumber);
            if (!String.IsNullOrEmpty(note))
                requestPayload.AppendFormat("&note={0}", note);

            var apiEndPoint = String.Format("/smartterminals/{0}/request-payment", smartTerminalId);
            var response = await SendRequestAsync(apiEndPoint, "POST", requestPayload.ToString());
            var result = JsonConvert.DeserializeObject<SmartTerminalPaymentRequest>(response);
            return result;
        }

        private TransactionResult ValidateTransactionResult(
            TransactionResult result)
        {
            if (this.ValidateTransactionResultSuccess && !result.Response.Approved)
                throw new TransactionResultFailureException(result);

            return result;
        }

        private async Task<TransactionResult> ExecuteTransactionAsync(
            StringBuilder requestPayload,
            decimal baseAmount,
            decimal? taxAmount = null,
            decimal? shippingAmount = null,
            decimal? surchargeAmount = null,
            decimal? tipAmount = null,
            AVSType avs = AVSType.Bypass,
            string billingAddress = null,
            string billingZipCode = null,
            bool cvv = false,
            string cardCvv = null,
            string invoiceNumber = null,
            string purchaseOrderNumber = null,
            string note = null)
        {
            requestPayload.AppendFormat("&amountBase={0}", baseAmount.ToCurrency(addCommas: false));

            if (taxAmount.HasValue)
                requestPayload.AppendFormat("&amountTax={0}", taxAmount.Value.ToCurrency(addCommas: false));
            if (shippingAmount.HasValue)
                requestPayload.AppendFormat("&amountShipping={0}", shippingAmount.Value.ToCurrency(addCommas: false));
            if (surchargeAmount.HasValue)
                requestPayload.AppendFormat("&amountSurcharge={0}", surchargeAmount.Value.ToCurrency(addCommas: false));
            if (tipAmount.HasValue)
                requestPayload.AppendFormat("&amountTip={0}", tipAmount.Value.ToCurrency(addCommas: false));

            requestPayload.AppendFormat("&avs={0}", avs.ToString().ToUpper());
            if (!String.IsNullOrEmpty(billingAddress))
                requestPayload.AppendFormat("&billingAddress={0}", billingAddress);
            if (!String.IsNullOrEmpty(billingZipCode))
                requestPayload.AppendFormat("&billingZip={0}", billingZipCode);

            requestPayload.AppendFormat("&cvv={0}", cvv.ToOnOff().ToUpper());
            if (cvv)
                requestPayload.AppendFormat("&cardCvv={0}", cardCvv);

            if (!String.IsNullOrEmpty(invoiceNumber))
                requestPayload.AppendFormat("&invoiceNumber={0}", invoiceNumber);
            if (!String.IsNullOrEmpty(purchaseOrderNumber))
                requestPayload.AppendFormat("&purchaseOrderNumber={0}", purchaseOrderNumber);
            if (!String.IsNullOrEmpty(note))
                requestPayload.AppendFormat("&note={0}", note);

            var apiEndPoint = "/transactions";
            var response = await SendRequestAsync(apiEndPoint, "POST", requestPayload.ToString());
            var result = JsonConvert.DeserializeObject<TransactionResult>(response);

            return ValidateTransactionResult(result);
        }

        public async Task<TransactionResult> ChargeAsync(
            string cardNumber,
            string cardExpMonth,
            string cardExpYear,
            decimal baseAmount,
            decimal? taxAmount = null,
            decimal? shippingAmount = null,
            decimal? surchargeAmount = null,
            decimal? tipAmount = null,
            AVSType avs = AVSType.Bypass,
            string billingAddress = null,
            string billingZipCode = null,
            bool cvv = false,
            string cardCvv = null,
            string invoiceNumber = null,
            string purchaseOrderNumber = null,
            string note = null)
        {
            var requestPayload = new StringBuilder();
            requestPayload.Append("action=CHARGE");
            requestPayload.AppendFormat("&cardNumber={0}", cardNumber.Replace("-", "").Replace(" ", ""));
            requestPayload.AppendFormat("&cardExpMonth={0}", cardExpMonth);
            requestPayload.AppendFormat("&cardExpYear={0}", cardExpYear);

            return await ExecuteTransactionAsync(
                requestPayload,
                baseAmount,
                taxAmount,
                shippingAmount,
                surchargeAmount,
                tipAmount,
                avs,
                billingAddress,
                billingZipCode,
                cvv,
                cardCvv,
                invoiceNumber,
                purchaseOrderNumber,
                note);
        }

        public async Task<TransactionResult> ReChargeAsync(
            string destinationTerminalId,
            string previousTransactionId, 
            decimal baseAmount,
            decimal? taxAmount = null, 
            decimal? shippingAmount = null, 
            decimal? surchargeAmount = null, 
            decimal? tipAmount = null, 
            AVSType avs = AVSType.Bypass, 
            string billingAddress = null, 
            string billingZipCode = null, 
            bool cvv = false, 
            string cardCvv = null, 
            string invoiceNumber = null, 
            string purchaseOrderNumber = null, 
            string note = null)
        {
            var requestPayload = new StringBuilder();
            requestPayload.Append("action=CHARGE");
            requestPayload.AppendFormat("&transactionId={0}", previousTransactionId);
            requestPayload.AppendFormat("&terminalId={0}", destinationTerminalId);

            return await ExecuteTransactionAsync(
                requestPayload,
                baseAmount,
                taxAmount,
                shippingAmount,
                surchargeAmount,
                tipAmount,
                avs,
                billingAddress,
                billingZipCode,
                cvv,
                cardCvv,
                invoiceNumber,
                purchaseOrderNumber,
                note);
        }

        public async Task<TransactionResult> VoidTransactionAsync(
            string transactionId)
        {
            var apiEndPoint = String.Format("/transactions/{0}", transactionId);
            var response = await SendRequestAsync(apiEndPoint, "PUT", "status=VOID");
            var result = JsonConvert.DeserializeObject<TransactionResult>(response);
            return ValidateTransactionResult(result);
        }

        public async Task<TransactionResult> RefundTransactionAsync(
            string transactionId,
            decimal baseAmount,
            decimal? taxAmount = null,
            decimal? shippingAmount = null,
            decimal? surchargeAmount = null,
            decimal? tipAmount = null,
            AVSType avs = AVSType.Bypass,
            string billingAddress = null,
            string billingZipCode = null,
            bool cvv = false,
            string cardCvv = null,
            string invoiceNumber = null,
            string purchaseOrderNumber = null,
            string note = null)
        {
            var requestPayload = new StringBuilder();
            requestPayload.Append("action=REFUND");
            requestPayload.AppendFormat("&transactionId={0}", transactionId);

            return await ExecuteTransactionAsync(
                requestPayload,
                baseAmount,
                taxAmount,
                shippingAmount,
                surchargeAmount,
                tipAmount,
                avs,
                billingAddress,
                billingZipCode,
                cvv,
                cardCvv,
                invoiceNumber,
                purchaseOrderNumber,
                note);
        }

        public async Task<TransactionResult> GetTransactionAsync(
            string transactionId)
        {
            var apiEndPoint = String.Format("/transactions/{0}", transactionId);
            var response = await SendRequestAsync(apiEndPoint, "GET");
            var result = JsonConvert.DeserializeObject<TransactionResult>(response);
            return result;
        }

        private async Task<string> SendRequestAsync(
            string apiEndpoint, 
            string requestMethod, 
            string requestPayload = null)
        {
            var encoding = new UTF8Encoding();
            var usernamePassword = String.Format("{0}:{1}", this.ApiUsername,
                this.ApiPassword);
            var url = this.ApiBaseUrl + apiEndpoint;

            // Create the request.
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = requestMethod;
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(encoding.GetBytes(usernamePassword));
            request.Headers["X-PJ-Application-Key"] = this.ApplicationKey;
            request.KeepAlive = true;
            request.Accept = "application/json";
            request.MediaType = "application/json";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

            // If a payload is provided, add it to the request stream.
            if (!String.IsNullOrEmpty(requestPayload))
            {
                byte[] requestPayloadBytes = encoding.GetBytes(requestPayload);
                request.ContentLength = requestPayloadBytes.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(requestPayloadBytes, 0, requestPayloadBytes.Length);
            }

            try
            {
                using (var webResponse = await request.GetResponseAsync())
                {
                    return await webResponse.ReadAsStringAsync();
                }
            }
            catch (WebException webException)
            {
                string response = await webException.Response.ReadAsStringAsync();
                throw new PayJunctionApiException(webException.Message, 
                    JsonConvert.DeserializeObject<ErrorsCollection>(response), webException);
            }
        }
    }
}
