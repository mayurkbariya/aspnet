using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SapproductCode { get; set; }
        public int? SegmentId { get; set; }
        public int? FlavorId { get; set; }
        public int? SizeId { get; set; }
        public int? ProductPrice { get; set; }
        public int? AgentPrice { get; set; }
        public int? Spprice { get; set; }
        public double? Vat { get; set; }
        public string ProductPicture { get; set; }
        public int? SellingPoint { get; set; }
        public int? PackingSize { get; set; }
        public int? ProductOrderNumber { get; set; }
        public bool? ProductStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public bool ActiveFlag { get; set; }
    }
}
