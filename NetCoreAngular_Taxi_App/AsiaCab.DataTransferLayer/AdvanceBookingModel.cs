using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaCab.DataTransferLayer
{
    public class AdvanceBookingModel
    {
        public Guid AdvanceBookingId { get; set; }
        public Guid? AdminId { get; set; }
        public string BookingType { get; set; }
        public string CarCaller { get; set; }
        public string JobStatus { get; set; }
        public DateTime? TravelTime { get; set; }
        public string PassengerContactNumber { get; set; }
        public int? Province { get; set; }
        public int? TaxiType { get; set; }
        public decimal? ServiceCharge { get; set; }
        public string PositionToReceive { get; set; }
        public string Destination { get; set; }
        public string NumberCar { get; set; }
        public string MachineNumber { get; set; }
        public string DetailsOfTheReception { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? ActiveFlag { get; set; }
    }
    public class AdvanceBookingRequest
    {
        public Guid? AdminId { get; set; }
        public string JobStatus { get; set; }
        public string BookingType { get; set; }
        public string CarCaller { get; set; }
        public DateTime? TravelTime { get; set; }
        public string PassengerContactNumber { get; set; }
        public int? Province { get; set; }
        public int? TaxiType { get; set; }
        public decimal? ServiceCharge { get; set; }
        public string PositionToReceive { get; set; }
        public string Destination { get; set; }
        public string NumberCar { get; set; }
        public string MachineNumber { get; set; }
        public string DetailsOfTheReception { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? ActiveFlag { get; set; }
    }
    public class AdvanceBookingResponse
    {
        public Guid AdvanceBookingId { get; set; }
        public Guid? AdminId { get; set; }
        public string BookingType { get; set; }
        public string CarCaller { get; set; }
        public string JobStatus { get; set; }
        public DateTime? TravelTime { get; set; }
        public string PassengerContactNumber { get; set; }
        public int? Province { get; set; }
        public int? TaxiType { get; set; }
        public decimal? ServiceCharge { get; set; }
        public string PositionToReceive { get; set; }
        public string Destination { get; set; }
        public string NumberCar { get; set; }
        public string MachineNumber { get; set; }
        public string DetailsOfTheReception { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? ActiveFlag { get; set; }
    }
}
