using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Domain.Entities.Devices.ATM.Maintenance
{
    [Table("MaintenanceRecords")]
    public class Record
    {
        [Key]
        public int Id { get; set; } // Асуудлын бүртгэлийн ID

        public int? MaintenanceRequestId { get; set; } // Асуудлын хүсэлтийн ID
        public Request? MaintenanceRequest { get; set; }


        public int? PerformedBy { get; set; } // Асуудлыг үүсгэсэн хэрэглэгчийн ID



        public DateTime? CompletedAt { get; set; } // Асуудлыг шалгаж дууссан огноо
        public DateTime? PerformedAt { get; set; } // Асуудлыг шалгаж эхэлсэн огноо

        public string? WorkDescription { get; set; } // Асуудлыг оношлосон тайлбар
        public virtual ICollection<UsedSparePart>? UsedSpareParts { get; set; }
        public int? UsedSparePartsQuantity { get; set; }
    }
}
