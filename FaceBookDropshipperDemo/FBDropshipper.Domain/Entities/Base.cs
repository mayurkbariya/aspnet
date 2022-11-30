using System;
using FBDropshipper.Domain.Interfaces;

namespace FBDropshipper.Domain.Entities
{
    public class Base : IBase
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}