using DevicesCRMSystem.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Application.Extensions
{
    public static class MaintenanceStatusExtensions
    {
        public static string ToMongolian(this MaintenanceStatus status)
        {
            return status switch
            {
                MaintenanceStatus.Pending => "Хүлээгдэж байгаа",
                MaintenanceStatus.InProgress => "Хийгдэж байгаа",
                MaintenanceStatus.Completed => "Гүйцэтгэсэн",
                MaintenanceStatus.Cancelled => "Цуцлагдсан",
                MaintenanceStatus.Testing => "Тест хийгдэж байгаа",
                _ => "Тодорхойгүй"
            };
        }
    }
}
