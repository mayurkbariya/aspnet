using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class AgentGrade
    {
        public int AgentGradeId { get; set; }
        public string AgentGradeName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public bool? ActiveFlag { get; set; }
    }
}
