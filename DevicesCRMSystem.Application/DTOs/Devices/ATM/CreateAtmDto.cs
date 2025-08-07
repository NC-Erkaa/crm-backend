using DevicesCRMSystem.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.DTOs.Devices.ATM
{
    public class CreateAtmDto
    {
        public string? DeviceName { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public DeviceType? DeviceType { get; set; }
        public DeviceStatus? Status { get; set; }
        public string? IP { get; set; }
        public int? Port { get; set; }
        public string? Location { get; set; }
        public string? Province { get; set; }
        public int? CreatedBy { get; set; }
        public int? OrgId { get; set; }
        //public string? OrgName { get; set; }
        public string? AtmZone { get; set; }
        public DateOnly? InstallationDate { get; set; }
        //public DateOnly ExpiredDate { get; set; }
    }
}
