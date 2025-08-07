using DevicesCRMSystem.Application.DTOs.SpareParts;
using DevicesCRMSystem.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.DTOs.Maintenance
{
    public class CreateMaintenanceRecordDto
    {
        public int MaintenanceRequestId { get; set; }

        public DateTime PerformedAt { get; set; }

        public int PerformedBy { get; set; }

        public string WorkDescription { get; set; }

        public DateTime CompletedAt { get; set; }

        public List<UsedSparePartDto> UsedSpareParts { get; set; } = new();

    }

    
}
