using DevicesCRMSystem.Application.DTOs.SpareParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.DTOs.Maintenance
{
    public class MaintenanceRecordDetailsDto
    {
        public string Status { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime? RequestedAt { get; set; }
        public string? Description { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime? PerformedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? WorkDescription { get; set; }
        public List<UsedSparePartDto> UsedSpareParts { get; set; } = new();
        //public string? TechnicianFullName { get; set; }
    }

    
}
