using Coinbase.Commerce;
using Coinbase.Commerce.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace TokenScan.Controllers
{
    public class PaymentController : ApiController
    {
        [Route("Api/Payment/Coinbase_Webhook"), HttpPost]
        public HttpResponseMessage Coinbase_Webhook()
        {
            var requestSignature = Request.Headers.GetValues("Webhook-Signature").FirstOrDefault();
            var requestContent = Request.Content.ReadAsStringAsync().Result;

            if (WebhookHelper.IsValid("sharedSecretKey", requestSignature, requestContent))
            {
                // The request is legit and an authentic message from Coinbase.
                // It's safe to deserialize the JSON body. 
                var webhook = JsonConvert.DeserializeObject<Webhook>(requestContent);

                var chargeInfo = webhook.Event.DataAs<Charge>();

                // Remember that customer ID we created back in the first example?
                // Here's were we can extract that information from the callback.
                var customerId = chargeInfo.Metadata["customerId"].ToObject<string>();

                string errMsg = "";
                
                if (webhook.Event.IsChargeFailed)
                {
                    errMsg = "failed";
                    Common.common.WritePaymentStatus(customerId,errMsg, DateTime.Now.ToString());
                }
                else if (webhook.Event.IsChargeCreated)
                {
                    // The charge was created just now.
                    // Do something with the newly created
                    // event.
                    errMsg = "Pending";
                    Common.common.WritePaymentStatus(customerId, errMsg, DateTime.Now.ToString());

                }
                else if (webhook.Event.IsChargeConfirmed)
                {
                    // The payment was confirmed.
                    // Fulfill the order!
                    errMsg = "Confirm";
                    Common.common.WritePaymentStatus(customerId, errMsg, DateTime.Now.ToString());

                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                // Some hackery going on. The Webhook message validation failed.
                // Someone is trying to spoof payment events!
                // Log the requesting IP address and HTTP body. 
                return Request.CreateResponse(HttpStatusCode.BadRequest);
                
            }
        }

    }
}
