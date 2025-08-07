using DevicesCRMSystem.Application.Enums;
using DevicesCRMSystem.Application.Extensions;
using DevicesCRMSystem.Domain.Entities.Devices;
using DevicesCRMSystem.Domain.Entities.Devices.ATM.Maintenance;
using DevicesCRMSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevicesCRMSystem.API.Controllers
{
    [ApiController]
    [Route("api/v1/devices/atm/dashboard")]
    public class AtmDashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AtmDashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("totalRequests")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTotalRequests(
                [FromQuery] DateTime? startDate,
                [FromQuery] DateTime? endDate)
        {
            DateTime end = endDate ?? DateTime.Now;
            DateTime start = startDate ?? end.AddMonths(-1);

            var result = await _context.MaintenanceRequest
                .Where(r => r.RequestedAt >= start && r.RequestedAt <= end)
                .GroupBy(r => r.Status)
                .Select(g => new
                {
                    Status = g.Key.ToMongolian().ToString(),
                    TotalRequests = g.Count()
                })
                .ToListAsync();

            return Ok(result);
        }


        [HttpGet("totalRequestsInLastYear")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTotalRequestsInLastYear()
        {
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1).AddMonths(-11); // Start of 12 months ago
            DateTime end = new DateTime(now.Year, now.Month, 1).AddMonths(1);     // Start of next month

            // Raw grouped data by Year, Month, and Status
            var rawData = await _context.MaintenanceRequest
                .Where(r => r.RequestedAt != null && r.RequestedAt >= start && r.RequestedAt < end)
                .GroupBy(r => new
                {
                    Year = r.RequestedAt.Value.Year,
                    Month = r.RequestedAt.Value.Month,
                    Status = r.Status
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.Status,
                    Count = g.Count()
                })
                .ToListAsync();

            // Month labels
            var labels = new[]
            {
        "LastTwelfthMonth", "LastEleventhMonth", "LastTenthMonth", "LastNinthMonth", "LastEighthMonth", "LastSeventhMonth",
        "LastSixthMonth", "LastFifthMonth", "LastFourthMonth", "LastThirdMonth", "LastSecondMonth", "LastFirstMonth"
    };

            // Generate months list
            var months = Enumerable.Range(0, 12)
                .Select(i =>
                {
                    var date = start.AddMonths(i);
                    return new
                    {
                        Label = labels[i],
                        Display = date.ToString("yyyy-MM"),
                        Year = date.Year,
                        Month = date.Month
                    };
                })
                .ToList();

            // Merge raw data into monthly summary with status counts
            var result = months.Select(m =>
            {
                var monthData = rawData.Where(d => d.Year == m.Year && d.Month == m.Month);
                return new
                {
                    label = m.Label,
                    display = m.Display,
                    PendingCount = monthData.Where(d => d.Status == MaintenanceStatus.Pending).Sum(d => d.Count),
                    InProgressCount = monthData.Where(d => d.Status == MaintenanceStatus.InProgress).Sum(d => d.Count),
                    CompletedCount = monthData.Where(d => d.Status == MaintenanceStatus.Completed).Sum(d => d.Count),
                    CancelledCount = monthData.Where(d => d.Status == MaintenanceStatus.Cancelled).Sum(d => d.Count),
                    TestingCount = monthData.Where(d => d.Status == MaintenanceStatus.Testing).Sum(d => d.Count)
                };
            });

            return Ok(result);
        }





        [HttpGet("totalUsedSpareParts")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTotalUsedSpareParts(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            DateTime end = endDate ?? DateTime.Now;
            DateTime start = startDate ?? end.AddMonths(-1);

            var result = await _context.MaintenanceUsedSparePart
                .Include(x => x.MaintenanceRecord)
                .Include(x => x.SparePart)
                .Where(x => x.MaintenanceRecord != null &&
                            x.MaintenanceRecord.CompletedAt >= start &&
                            x.MaintenanceRecord.CompletedAt <= end)
                .GroupBy(x => x.SparePart.PartName)
                .Select(g => new
                {
                    SparePartName = g.Key,
                    TotalQuantityUsed = g.Sum(x => x.QuantityUsed)
                })
                .ToListAsync();

            return Ok(result);
        }


        //[HttpGet("totalRegistredAtmCount")]
        ////[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<int>> GetTotalRegisteredAtm()
        //{
        //    var total = await _context.DeviceInfo
        //        .CountAsync(d => d.DeviceType == DeviceType.ATM);

        //    return Ok(total);
        //}

        

        [HttpGet("TotalRegisteredDevicesCount")]
        //[Authorize(Roles = "Admin" )]
        public async Task<ActionResult<IEnumerable<object>>> GetAllATMsCount()
        {
            var atms = await _context.DeviceInfo
                .Where(d => d.DeviceType == DeviceType.ATM)
                .Select(d => new
                {
                    d.Id,
                    Нэр = d.DeviceName,
                    СериалДугаар = d.SerialNumber,
                    Загвар = d.Model,
                    Төрөл = d.DeviceType.ToString(),
                    Төлөв = d.Status.HasValue ? d.Status.ToString() : null,
                    d.IP,
                    d.Port,
                    Байршил = d.Province + " - " + d.Location,
                    //d.Province,
                    //masterKey = d.Masterkey,
                    d.InstallationDate,
                    d.ExpiredDate,
                    d.CreatedBy,
                    d.OrgName,
                    d.AtmZone
                })
                .ToListAsync();

            var totalATMsCount = atms.Count;

            var totalKiosksCount = await _context.DeviceInfo
                .CountAsync(kiosks => kiosks.DeviceType == DeviceType.Kiosk);

            var totalOthersCount = await _context.DeviceInfo
                .CountAsync(others => others.DeviceType == DeviceType.Other);


            return Ok(new
            {
                CountATMs = totalATMsCount,
                CountKiosks = totalKiosksCount,
                CountOthers = totalOthersCount
            });
        }
    }
}
