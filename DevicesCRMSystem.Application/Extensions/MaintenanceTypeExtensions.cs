using DevicesCRMSystem.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.Extensions
{
    public static class MaintenanceTypeExtensions
    {
        public static string ToMn(this MaintenanceType type)
        {
            return type switch
            {
                MaintenanceType.Hardware => "Төхөөрөмжийн алдаа",
                MaintenanceType.Software => "Програм хангамжийн алдаа",
                MaintenanceType.Firmware => "Фирмвэр алдаа",
                MaintenanceType.OS => "Үйлдлийн системийн алдаа",
                MaintenanceType.Network => "Сүлжээний алдаа",
                MaintenanceType.Other => "Бусад алдаа",
                _ => "Тодорхойгүй"
            };
        }

        public static string ToEn(this MaintenanceType type)
        {
            return type.ToString();
        }
    }
}
