using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.DTOs.Devices.Kiosk
{
    public class CreateKioskDto
    {
        public string? DeviceName { get; set; }
        public string? SerialNumber { get; set; }
    }
}
