using DevicesCRMSystem.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.DTOs.Maintenance
{
    public class CreateMaintenanceRequestDto
    {
        public int DeviceId { get; set; }    
        public string Description { get; set; }
        public int RequestedBy { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
    }
}
