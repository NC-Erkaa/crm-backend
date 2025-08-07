
using DevicesCRMSystem.Application.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DevicesCRMSystem.Domain.Entities.Devices
{
    [Table("devices")]
    public class DeviceInfo
    {
        [Key]

        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? DeviceName { get; set; }

        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        [MaxLength(100)]
        public string? Model { get; set; }

        [Required]
        public DeviceType? DeviceType { get; set; }

        public DeviceStatus? Status { get; set; }

        [MaxLength(45)]
        public string? IP { get; set; }

        public int? Port { get; set; }

        [MaxLength(45)]
        public string? Location { get; set; }

        [MaxLength(45)]
        public string? Province { get; set; }

        //[MaxLength(45)]
        //public string? Masterkey { get; set; }

        public DateOnly? InstallationDate { get; set; }

        public DateOnly? ExpiredDate { get; set; }

        public int? CreatedBy { get; set; }

        [MaxLength(45)]
        public string? OrgName { get; set; }

        public int? OrgId { get; set; }
        public string? AtmZone { get; set; }
    }

    

    
}
