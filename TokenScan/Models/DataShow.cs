using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenScan.Models
{
    public class DataShow
    {
        public string wallet { get; set; }
        public List<DataResult>  dataResult { get; set; }
    }
}