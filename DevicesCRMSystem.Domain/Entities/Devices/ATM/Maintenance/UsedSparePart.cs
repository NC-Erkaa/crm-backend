using DevicesCRMSystem.Domain.Entities.SpareParts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Domain.Entities.Devices.ATM.Maintenance
{
    [Table("UsedSpareParts")]
    public class UsedSparePart
    {
        public int Id {  get; set; }

        public int? SparePartId { get; set; }
        public SparePartsInfo? SparePart { get; set; }

        public int? QuantityUsed { get; set; }
        public int? MaintenanceRecordId { get; set; }
        public Record? MaintenanceRecord { get; set; }
    }
}
