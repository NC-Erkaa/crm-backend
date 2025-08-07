using DevicesCRMSystem.Application.Enums;


namespace DevicesCRMSystem.Application.DTOs.Maintenance
{
    public class MaintenanceStatusInfo
    {
        public int Id { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string NameMn { get; set; } = string.Empty;

        public static List<MaintenanceStatusInfo> GetAllStatuses()
        {
            return new List<MaintenanceStatusInfo>
            {
                new MaintenanceStatusInfo { Id = (int)MaintenanceStatus.Pending, NameEn = "Pending", NameMn = "Хүлээгдэж байна" },
                new MaintenanceStatusInfo { Id = (int)MaintenanceStatus.InProgress, NameEn = "In Progress", NameMn = "Хийгдэж байна" },
                new MaintenanceStatusInfo { Id = (int)MaintenanceStatus.Completed, NameEn = "Completed", NameMn = "Дууссан" },
                new MaintenanceStatusInfo { Id = (int)MaintenanceStatus.Cancelled, NameEn = "Cancelled", NameMn = "Цуцлагдсан" },
                new MaintenanceStatusInfo { Id = (int)MaintenanceStatus.Testing, NameEn = "Testing", NameMn = "Тест хийгдэж байна" }
            };
        }

        public static string? GetNameMn(MaintenanceStatus status)
        {
            return GetAllStatuses().Find(s => s.Id == (int)status)?.NameMn;
        }
    }
}

