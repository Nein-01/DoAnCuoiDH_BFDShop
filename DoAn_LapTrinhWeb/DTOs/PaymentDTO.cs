using System;
using System.ComponentModel.DataAnnotations;

namespace DoAn_LapTrinhWeb.DTOs
{
    public class OrderInfo
    {
        public long OrderId { get; set; }
        public double Amount { get; set; }
        public string OrderDesc { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public long PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }
    }
}