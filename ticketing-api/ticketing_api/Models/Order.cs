using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Sieve.Attributes;
using ticketing_api.Data;
using ticketing_api.Infrastructure;

namespace ticketing_api.Models
{
    public class Order : IAuditable, ISoftDeletable
    {
        [Sieve(CanSort = true, CanFilter = true)]
        [Key] public int Id { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        [Required] public DateTime OrderDate { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        [Required] public DateTime RequestDate { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        [Required] public string RequestTime { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public byte OrderStatusId { get; set; } = 1; //AppConstants.OrderStatus.Open;

        [Sieve(CanSort = true, CanFilter = true)]
        public short JobTypeId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        [Required] public short CustomerId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string CustomerNotes { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        [Required] public int RigLocationId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string RigLocationNotes { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string WellName { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public int WellId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string WellCode { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public int DriverId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public int TruckId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string AFEPO { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string PointOfContactName { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string PointOfContactNumber { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public int MarketId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public int LoadOriginId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public int SalesRepId { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        [Required] public string OrderDescription { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string WellDirection { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        [MaxLength(1000, ErrorMessage = "Notes cannot be longer than 1000 characters.")]
        public string InternalNotes { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public bool ShippingPaperNA { get; set; }

        public bool ShippingPaperExists { get; set; }

        public bool TicketImageExists { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public string SpecialHandling { get; set; }

        [Sieve(CanSort = true, CanFilter = true)]
        public DateTime DeliveredDate { get; set; }

        public bool IsEnabled { get; set; } = true;

        [IgnoreDataMember] public bool IsDeleted { get; set; } = false;
    }
}