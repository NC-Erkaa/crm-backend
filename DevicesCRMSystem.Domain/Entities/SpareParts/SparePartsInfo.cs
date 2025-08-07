using DevicesCRMSystem.Application.Enums;
using DevicesCRMSystem.Domain.Entities.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Domain.Entities.SpareParts
{
    [Table("spareParts")]
    public class SparePartsInfo
    {
        public int Id { get; set; }
        public string? PartName { get; set; }
        public string? PartNumber { get; set; }
        public string? SerialNumber { get; set; }
        public int? StockQuantity { get; set; }
        public int? UsedQuantity { get; set; }
        public int? TotalPurchasedQuantity { get; set; }
        public string? Type { get; set; }
        public DeviceType RelatedToDevice { get; set; }
        public int? OrgId { get; set; }

    }
}
