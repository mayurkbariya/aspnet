using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class News
    {
        public int NewsId { get; set; }
        public string NewsTopics { get; set; }
        public string NewsDescription { get; set; }
        public string NewsImage { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? ActiveFlag { get; set; }
    }
}
