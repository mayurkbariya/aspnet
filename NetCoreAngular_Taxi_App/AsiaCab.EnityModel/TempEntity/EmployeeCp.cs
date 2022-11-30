using System;
using System.Collections.Generic;

namespace AsiaCab.EnityModel.TempEntity
{
    public partial class EmployeeCp
    {
        public int EmployeeId { get; set; }
        public int? SalaryId { get; set; }
        public int UserId { get; set; }
        public int? MemberId { get; set; }
        public int? SupervisorId { get; set; }
        public DateTime? StartWorkDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public bool? ActiveFlag { get; set; }
    }
}
