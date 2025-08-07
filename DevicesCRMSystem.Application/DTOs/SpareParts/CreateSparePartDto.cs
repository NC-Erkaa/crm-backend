using DevicesCRMSystem.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.DTOs.SpareParts
{
    public class CreateSparePartDto
    {
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public string SerialNumber { get; set; }
        public SparePartType Type { get; set; }
        public DeviceType ReleatedToDevice { get; set; }
        public int OrgId { get; set; }

    }
}
