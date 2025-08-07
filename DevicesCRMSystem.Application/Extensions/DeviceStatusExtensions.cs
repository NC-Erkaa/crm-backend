using DevicesCRMSystem.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.Extensions
{
    public static class DeviceStatusExtensions
    {
        public static string ToMongolian(this DeviceStatus status)
        {
            return status switch
            {
                DeviceStatus.Active => "Идэвхитэй",
                DeviceStatus.Inactive => "Идэвхигүй",
                DeviceStatus.Maintenance => "Засвартай",
                DeviceStatus.Retired => "Актлагдсан",
                _ => "Тодорхойгүй"
            };
        }
    }
}
