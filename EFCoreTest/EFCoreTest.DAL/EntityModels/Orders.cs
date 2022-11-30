using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCoreTest.DAL.EntityModels
{
    public partial class Orders
    {
        public Orders()
        {
            InverseBackorderOrder = new HashSet<Orders>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int SalespersonPersonId { get; set; }
        public int? PickedByPersonId { get; set; }
        public int ContactPersonId { get; set; }
        public int? BackorderOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string CustomerPurchaseOrderNumber { get; set; }
        public bool IsUndersupplyBackordered { get; set; }
        public string Comments { get; set; }
        public string DeliveryInstructions { get; set; }
        public string InternalComments { get; set; }
        public DateTime? PickingCompletedWhen { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime LastEditedWhen { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public Orders BackorderOrder { get; set; }
        public People ContactPerson { get; set; }
        public Customers Customer { get; set; }
        public People LastEditedByNavigation { get; set; }
        public People PickedByPerson { get; set; }
        public People SalespersonPerson { get; set; }
        public ICollection<Orders> InverseBackorderOrder { get; set; }
    }
}
