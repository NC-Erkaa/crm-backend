using DevicesCRMSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevicesCRMSystem.API.Controllers
{
    [ApiController]
    [Route("api/v1/devices/atm/scheduledMaintenance")]
    public class AtmScheduledMaintenanceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AtmScheduledMaintenanceController(AppDbContext context)
        {
            _context = context;
        }

        // ATM Service
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllMaintenanceRequests()
        {
            var requests = await _context.MaintenanceRequest

                .Include(r => r.Device)
                .Select(r => new
                {
                    r.Id,
                    r.RequestedBy,
                    r.DeviceId,
                    DeviceName = r.Device != null ? r.Device.DeviceName : null,
                    r.Description,
                    r.Status,
                    r.RequestedAt,
                    r.MaintenanceType
                })
                .ToListAsync();

            return Ok(requests);
        }
    }
}
