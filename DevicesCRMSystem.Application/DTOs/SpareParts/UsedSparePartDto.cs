using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.DTOs.SpareParts
{
    public class UsedSparePartDto
    {
        public int SparePartId { get; set; }

        public int Quantity { get; set; }
        public string PartName { get; set; }
        public int? QuantityUsed { get; set; }
    }
}
