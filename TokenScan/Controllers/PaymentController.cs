using Coinbase.Commerce;
using Coinbase.Commerce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TokenScan.Controllers
{
    public class PaymentController : ApiController
    {
        [Route("Api/Payment/GetOne")]
        [HttpGet]
        public string GetOne(string mes)
        {
            return mes;
        }

    }
}
