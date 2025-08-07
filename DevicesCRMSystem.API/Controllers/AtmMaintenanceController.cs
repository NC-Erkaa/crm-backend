using DevicesCRMSystem.Application.DTOs;
using DevicesCRMSystem.Application.DTOs.Maintenance;
using DevicesCRMSystem.Application.DTOs.SpareParts;
using DevicesCRMSystem.Application.Enums;
using DevicesCRMSystem.Application.Extensions;
using DevicesCRMSystem.Domain.Entities.Devices;
using DevicesCRMSystem.Domain.Entities.Devices.ATM.Maintenance;
using DevicesCRMSystem.Domain.Entities.SpareParts;
using DevicesCRMSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevicesCRMSystem.API.Controllers
{
    [ApiController]
    [Route("api/v1/devices/atm/maintenance")]

    public class ATMMaintenanceController : ControllerBase
    {

        private readonly AppDbContext _context;

        public ATMMaintenanceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("maintenanceType")]
        public IActionResult GetAllMaintenanceTypes()
        {
            var result = Enum.GetValues(typeof(MaintenanceType))
                .Cast<MaintenanceType>()
                .Select(e => new
                {
                    value = (int)e,
                    typeMn = e.ToMn(),
                    typeEn = e.ToEn()
                });

            return Ok(result);
        }


        //[HttpGet("all")]
        //public async Task<ActionResult<IEnumerable<object>>> GetAllMaintenanceRequests()
        //{
        //    var requests = await _context.MaintenanceRequest

        //        .Include(r => r.Device)
        //        .Select(r => new
        //        {
        //            r.Id,
        //            r.RequestedBy,
        //            r.DeviceId,
        //            DeviceName = r.Device != null ? r.Device.DeviceName : null,
        //            r.Description,
        //            r.Status,
        //            r.RequestedAt,
        //            r.MaintenanceType
        //        })
        //        .ToListAsync();

        //    return Ok(requests);
        //}


        [HttpGet("Requests")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllAtmMaintenanceRequests()
        {
            var requests = await _context.MaintenanceRequest
            .Include(r => r.Device)
            //.Include(r => r.RequestedByUser) // or .RequestedByUser
            .Select(r => new
            {
                r.Id, 
                Status = r.Status.ToMongolian(),
                //FullName = r.RequestedByUser != null ? r.RequestedByUser.Fullname : null,
                RequestedBy = 0,
                DeviceName = r.Device != null ? r.Device.DeviceName : null,
                r.Description,                
                r.RequestedAt,
                MaintenanceType = r.MaintenanceType.ToMn()
            })
            .ToListAsync();

            return Ok(requests);

        }

        [HttpGet("Requests/{id}")]
        public async Task<ActionResult<Request>> GetAtmMaintenanceRequestById(int id)
        {
            var requests = await _context.MaintenanceRequest
            .Where(d => d.Id == id)
            .Include(r => r.Device)
            //.Include(r => r.RequestedByUser)
            .Select(r => new
            {
                r.Id,
                Status = r.Status.ToMongolian(),
                //Username = r.RequestedByUser != null ? r.RequestedByUser.Fullname : null,
                DeviceName = r.Device != null ? r.Device.DeviceName : null,
                r.Description,
                r.RequestedAt,
                MaintenanceType = r.MaintenanceType.ToMn()
            })
            .ToListAsync();

            if (requests == null)
                return NotFound();

            return Ok(requests);
        }


        

        // create new maintenance request by device id
        [HttpPost("Requests")]
        public async Task<ActionResult<object>> CreateAtmMaintenanceRequest([FromBody] CreateMaintenanceRequestDto requestDto)
        {
            if (requestDto == null || requestDto.DeviceId <= 0)
                return BadRequest("Invalid request.");

            // Look up device by name
            var device = await _context.DeviceInfo
                .FirstOrDefaultAsync(d => d.Id == requestDto.DeviceId);

            if (device == null)
                return NotFound($"Device with name '{requestDto.DeviceId}' not found.");

            // Create request
            var maintenanceRequest = new Request
            {
                DeviceId = device.Id,
                Description = requestDto.Description,
                //RequestedBy = requestDto.RequestedBy,
                MaintenanceType = requestDto.MaintenanceType,
                
            };

            _context.MaintenanceRequest.Add(maintenanceRequest);
            await _context.SaveChangesAsync();

            // Return full response object
            var result = await _context.MaintenanceRequest
                .Include(r => r.Device)
                //.Include(r => r.RequestedByUser)
                .Where(r => r.Id == maintenanceRequest.Id)
                .Select(r => new
                {
                    r.Id,
                    Status = r.Status.ToMongolian(),
                    //FullName = r.RequestedByUser.Fullname,
                    DeviceName = r.Device.DeviceName,
                    r.Description,
                    r.RequestedAt,
                    MaintenanceType = r.MaintenanceType.ToMn()
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetAtmMaintenanceRequestById), new { id = maintenanceRequest.Id }, result);
        }


        [HttpDelete("Requests/{id}")]
        [Authorize(Roles = "ADMIN,MODERATOR,USER")]
        public async Task<IActionResult> DeleteAtmMaintenanceRequest(int id)
        {
            var request = await _context.MaintenanceRequest.FindAsync(id);
            if (request == null)
                return NotFound();

            _context.MaintenanceRequest.Remove(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{id} ID-тай засвар үйлчилгээний хүсэлтийг амжилттай устгалаа." });
        }


        [HttpGet("Records")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllRecords()
        {
            var records = await _context.MaintenanceRecord
                .Include(mr => mr.MaintenanceRequest)
                    .ThenInclude(req => req.Device)
                .Include(mr => mr.MaintenanceRequest)
                    
                
                .Include(mr => mr.UsedSpareParts)
                    .ThenInclude(us => us.SparePart)
                .Select(mr => new
                {
                    Status = mr.MaintenanceRequest != null
                        ? mr.MaintenanceRequest.Status.ToMongolian()
                        : "Unknown",

                    // Device Info - use conditional operator instead of null-conditional
                    DeviceName = mr.MaintenanceRequest != null && mr.MaintenanceRequest.Device != null
                        ? mr.MaintenanceRequest.Device.DeviceName
                        : null,

                    Province = mr.MaintenanceRequest != null && mr.MaintenanceRequest.Device != null
                        ? mr.MaintenanceRequest.Device.Province
                        : null,

                    Location = mr.MaintenanceRequest != null && mr.MaintenanceRequest.Device != null
                        ? mr.MaintenanceRequest.Device.Location
                        : null,

                    // Request Info
                    RequestedAt = mr.MaintenanceRequest != null
                        ? mr.MaintenanceRequest.RequestedAt
                        : (DateTime?)null,

                    Description = mr.MaintenanceRequest != null
                        ? mr.MaintenanceRequest.Description
                        : null,

                    //RequestedBy = mr.MaintenanceRequest != null && mr.MaintenanceRequest.RequestedByUser != null
                    //    ? mr.MaintenanceRequest.RequestedByUser.Fullname
                    //    : null,

                    // Maintenance Info
                    PerformedAt = mr.PerformedAt,
                    CompletedAt = mr.CompletedAt,
                    WorkDescription = mr.WorkDescription,

                    // Spare Parts Info (now a list)
                    UsedSpareParts = mr.UsedSpareParts.Select(us => new
                    {
                        PartName = us.SparePart != null ? us.SparePart.PartName : null,
                        QuantityUsed = us.QuantityUsed
                    }),

                    // Technician Info
                    //TechnicianFullName = mr.TechnicianByUser != null ? mr.TechnicianByUser.Fullname : null
                })
                .ToListAsync();

            return Ok(records);
        }



        //[HttpGet("Recordss")]
        //public async Task<ActionResult<IEnumerable<object>>> GetAllRecordss()
        //{
        //    var atms = await _context.MaintenanceRecord

        //        .Select(d => new
        //        {
        //            d.Id,
        //            d.MaintenanceRequestId,
        //            d.CompletedAt,
        //            d.PerformedAt,
        //            d.PerformedBy,
        //            d.WorkDescription,
        //            d.UsedSparePart,
        //            d.UsedSparePartsQuantity
        //        })
        //        .ToListAsync();

        //    return Ok(atms);
        //}


        [HttpGet("Records/{id}")]
        public async Task<ActionResult<MaintenanceRecordDetailsDto>> GetById(int id)
        {
            var record = await _context.MaintenanceRecord
                .Where(r => r.Id == id)
                .Include(r => r.MaintenanceRequest)
                    .ThenInclude(req => req.Device)
                .Include(r => r.MaintenanceRequest)
                    //.ThenInclude(req => req.RequestedByUser)
                //.Include(r => r.TechnicianByUser)
                .Include(r => r.UsedSpareParts)
                    .ThenInclude(us => us.SparePart)
                .Select(r => new MaintenanceRecordDetailsDto
                {
                    Status = r.MaintenanceRequest.Status.ToString(),
                    DeviceName = r.MaintenanceRequest.Device!.DeviceName,
                    Province = r.MaintenanceRequest.Device.Province ?? "",
                    Location = r.MaintenanceRequest.Device.Location ?? "",
                    RequestedAt = r.MaintenanceRequest.RequestedAt,
                    Description = r.MaintenanceRequest.Description,
                    
                    PerformedAt = r.PerformedAt,
                    CompletedAt = r.CompletedAt,
                    WorkDescription = r.WorkDescription,
                    UsedSpareParts = r.UsedSpareParts.Select(us => new UsedSparePartDto
                    {
                        PartName = us.SparePart!.PartName,
                        QuantityUsed = us.QuantityUsed
                    }).ToList(),
                    //TechnicianFullName = r.TechnicianByUser != null ? r.TechnicianByUser.FullName : null
                })
                .FirstOrDefaultAsync();

            if (record == null)
                return NotFound();

            return Ok(record);
        }

        

        [HttpPost("Records")]
        public async Task<ActionResult<object>> CreateAtmMaintenanceRecord([FromBody] CreateMaintenanceRecordDto recordDto)
        {
            if (recordDto == null || recordDto.MaintenanceRequestId <= 0)
                return BadRequest("Invalid request.");

            var record = new Record
            {
                MaintenanceRequestId = recordDto.MaintenanceRequestId,
                PerformedAt = recordDto.PerformedAt,
                PerformedBy = recordDto.PerformedBy,
                WorkDescription = recordDto.WorkDescription,
                CompletedAt = recordDto.CompletedAt
            };

            _context.Set<Record>().Add(record);

            // Save here to generate record.Id for foreign key relations
            await _context.SaveChangesAsync();

            if (recordDto.UsedSpareParts != null && recordDto.UsedSpareParts.Any())
            {
                foreach (var part in recordDto.UsedSpareParts)
                {
                    var usedPart = new UsedSparePart
                    {
                        MaintenanceRecordId = record.Id,  // Link by Id after saving
                        SparePartId = part.SparePartId,
                        QuantityUsed = part.Quantity
                    };
                    _context.Set<UsedSparePart>().Add(usedPart);
                }

                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }





        // Export report between startDate and endDate
        [HttpGet("Report")]
        public async Task<ActionResult<IEnumerable<object>>> GetRecordsBetweenDates(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            DateTime end = endDate ?? DateTime.Now;
            DateTime start = startDate ?? end.AddMonths(-12);

            var records = await _context.MaintenanceRecord
                .Where(mr => mr.PerformedAt >= start && mr.PerformedAt <= end)
                .Include(mr => mr.MaintenanceRequest)
                    .ThenInclude(req => req.Device)
                //.Include(mr => mr.MaintenanceRequest)
                //    .ThenInclude(req => req.RequestedByUser)
                //.Include(mr => mr.RequestedByUser)
                //.Include(mr => mr.TechnicianByUser)
                .Include(mr => mr.UsedSpareParts) // assuming navigation to UsedSparePart
                .Select(mr => new
                {
                    Id = mr.Id,
                    Status = mr.MaintenanceRequest != null
                                ? mr.MaintenanceRequest.Status.ToMongolian()
                                : "Unknown",

                    // Device Info
                    DeviceName = mr.MaintenanceRequest.Device.DeviceName,
                    Байршил = mr.MaintenanceRequest.Device.Province + " - " + mr.MaintenanceRequest.Device.Location,
                    //Location = mr.MaintenanceRequest.Device.Location,

                    // Request Info
                    RequestedAt = mr.MaintenanceRequest.RequestedAt,
                    Description = mr.MaintenanceRequest.Description,
                    RequestedBy = mr.MaintenanceRequest.RequestedBy,

                    // Maintenance Info
                    PerformedAt = mr.PerformedAt,
                    CompletedAt = mr.CompletedAt,
                    WorkDescription = mr.WorkDescription,
                    UsedSparePart = mr.UsedSpareParts.Select(us => new
                    {
                        PartName = us.SparePart != null ? us.SparePart.PartName : null,
                        QuantityUsed = us.QuantityUsed
                    }).ToList(),
                    UsedSparePartsQuantity = mr.UsedSparePartsQuantity,

                    // Technician Info
                    //ГүйцэтгэсэнМэргэжилтэн = mr.TechnicianByUser.Fullname
                })
                .ToListAsync();

            return Ok(records);
        }


    }
}
