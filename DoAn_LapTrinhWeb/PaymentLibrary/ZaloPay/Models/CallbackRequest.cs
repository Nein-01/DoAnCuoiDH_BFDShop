using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.ZaloPay.Models
{
    public class CallbackRequest
    {
        public string Data { get; set; }
        public string Mac { get; set; }
    }
}