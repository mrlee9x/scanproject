using Coinbase.Commerce;
using Coinbase.Commerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TokenScan.Controllers
{
    public class CallCoinbaseController : Controller
    {
        // GET: CallCoinbase
        public ActionResult Billing()
        {
            return View();
        }

        public async Task<ActionResult> CallCoinBaseAsync(string customerid)
        {
            var commerceApi = new CommerceApi("apikey");

            // Create a unique identifier to associate
            // the customer in your system with the
            // crypto payment they are about to make.
            // Normally, this is a unique ID for your
            // customer inside your database.
            var charge = new CreateCharge
            {
                Name = "Scan Premium",
                Description = "Provided by TokenScan",
                PricingType = PricingType.FixedPrice,
                LocalPrice = new Money { Amount = 3.00m, Currency = "USD" },
                Metadata = {
                             {"customerId", customerid}

                           },

            };
            var response = await commerceApi.CreateChargeAsync(charge);

            // Check for any errors
            if (response.HasError())
            {
                // Coinbase says something is wrong. Log the error 
                // and report back to the user an error has occurred.
                return RedirectToAction("Billing");
 
            }

            // else, send the user to the hosted checkout page at Coinbase.
            return Redirect(response.Data.HostedUrl);

        }
        
    }
}