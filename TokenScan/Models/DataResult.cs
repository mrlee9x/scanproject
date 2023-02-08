using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenScan.Models
{
    public class DataResult
    {
        public string tokenAddress { get; set; }
        public Tokenamount tokenAmount { get; set; }
        public string tokenAccount { get; set; }
        public string tokenName { get; set; }
        public string tokenIcon { get; set; }
        public int rentEpoch { get; set; }
        public int lamports { get; set; }
        public string tokenSymbol { get; set; }
    }

    public class Tokenamount
    {
        public string amount { get; set; }
        public int decimals { get; set; }
        public float uiAmount { get; set; }
        public string uiAmountString { get; set; }
    }


}