
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevicesCRMSystem.Application.Enums;

namespace DevicesCRMSystem.Domain.Entities.Devices.ATM.Maintenance
{
    [Table("MaintenanceRequests")]
    public class Request
    {
        [Key]
        public int Id { get; set; }
        //public DateTime? RequestedAt { get; set; }

        public int? DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public DeviceInfo? Device { get; set; }      
        public string? Description { get; set; }
        public int? RequestedBy { get; set; }

        public MaintenanceStatus Status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? RequestedAt { get; set; }

        public MaintenanceType MaintenanceType { get; set; }


    }

}
