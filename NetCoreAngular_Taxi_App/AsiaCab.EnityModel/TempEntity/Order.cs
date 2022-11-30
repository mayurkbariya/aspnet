using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public string Cvcode { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OrderNo { get; set; }
        public DateTime? CpreceiveDate { get; set; }
        public string AgentName { get; set; }
        public string StatusPo { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string Price { get; set; }
        public double? Vat { get; set; }
        public double? PriceVat { get; set; }
        public string Unit { get; set; }
        public double? UnitPrice { get; set; }
        public string StockOnHold { get; set; }
        public string FavoriteCustomer { get; set; }
        public string Sppickup { get; set; }
        public string Amount { get; set; }
    }
}
