using DevicesCRMSystem.Application.Enums;
using DevicesCRMSystem.Domain.Entities.Devices;
using DevicesCRMSystem.Domain.Entities.SpareParts;
using DevicesCRMSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevicesCRMSystem.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SparePartsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SparePartsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{deviceTypeStr}/{OrgId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllSparePartsForAtm(string deviceTypeStr, int OrgId)
        {
            if (!Enum.TryParse<DeviceType>(deviceTypeStr, true, out var deviceType))
                return BadRequest("Invalid device type");

            var spareParts = await _context.SparePartsInfo
                .Where(sp => sp.RelatedToDevice == deviceType && sp.OrgId == OrgId)
                .Select(sp => new
                {
                    sp.Id,
                    sp.PartName,
                    sp.PartNumber,
                    sp.SerialNumber,
                    sp.StockQuantity,
                    sp.UsedQuantity,
                    sp.TotalPurchasedQuantity,
                    sp.Type,
                    RelatedToDevice = sp.RelatedToDevice.ToString(),
                    sp.OrgId
                })
                .ToListAsync();

            return Ok(spareParts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetSparePartByIdForAtm(int id)
        {
            var spareParts = await _context.SparePartsInfo
                .Where(sp => sp.Id == id && sp.RelatedToDevice == DeviceType.ATM)
                .Select(sp => new
                {
                    sp.Id,
                    sp.PartName,
                    sp.PartNumber,
                    sp.SerialNumber,
                    sp.StockQuantity,
                    sp.UsedQuantity,
                    sp.TotalPurchasedQuantity,
                    sp.Type,
                    RelatedToDevice = sp.RelatedToDevice.ToString(),
                })
                .ToListAsync();
            if (spareParts == null || !spareParts.Any()) return NotFound($"{id} ID-тай {DeviceType.ATM}-ны сэлбэг олдсонгүй");
            return Ok(spareParts);
        }
        
        
        [HttpGet("ATM")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllSparePartsForAtm()
        {
            var spareParts = await _context.SparePartsInfo
                .Where(sp => sp.RelatedToDevice == DeviceType.ATM )
                .Select(sp => new
                {
                    sp.Id,
                    sp.PartName,
                    sp.PartNumber,
                    sp.SerialNumber,
                    sp.StockQuantity,
                    sp.UsedQuantity,
                    sp.TotalPurchasedQuantity,
                    sp.Type,
                    RelatedToDevice = sp.RelatedToDevice.ToString(),
                    sp.OrgId
                })
                .ToListAsync();

            return Ok(spareParts);
        }


        

        [HttpPost()]
        public async Task<ActionResult<SparePartsInfo>> CreateSparePartForAtm([FromBody] SparePartsInfo sparePart)
        {
            _context.SparePartsInfo.Add(sparePart);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSparePartByIdForAtm), new { id = sparePart.Id }, sparePart);
        }

        [HttpPatch("ATM/{id}")]
        public async Task<IActionResult> UpdateSparePartForAtm(int id, SparePartsInfo updatedSparePartForAtm)
        {
            if (id != updatedSparePartForAtm.Id)
                return BadRequest();

            var sparePart = await _context.SparePartsInfo
                .FirstOrDefaultAsync(sp => sp.Id == id && sp.RelatedToDevice == DeviceType.ATM);

            if (sparePart == null)
                return NotFound();


            if (updatedSparePartForAtm.PartName != null) sparePart.PartName = updatedSparePartForAtm.PartName;
            if (updatedSparePartForAtm.SerialNumber != null) sparePart.SerialNumber = updatedSparePartForAtm.SerialNumber;
            if (updatedSparePartForAtm.PartNumber != null) sparePart.PartNumber = updatedSparePartForAtm.PartNumber;
            if (updatedSparePartForAtm.StockQuantity != null) sparePart.StockQuantity = updatedSparePartForAtm.StockQuantity;
            if (updatedSparePartForAtm.UsedQuantity != null) sparePart.UsedQuantity = updatedSparePartForAtm.UsedQuantity;
            if (updatedSparePartForAtm.TotalPurchasedQuantity != null) sparePart.TotalPurchasedQuantity = updatedSparePartForAtm.TotalPurchasedQuantity;
            if (updatedSparePartForAtm.Type != null) sparePart.Type = updatedSparePartForAtm.Type;


            await _context.SaveChangesAsync();

            return Ok($"Updated spare part's information for ATM with ID: {id}");
        }



        [HttpDelete("ATM/{id}")]
        public async Task<IActionResult> DeleteSparePartForAtm(int id)
        {
            var sparePart = await _context.SparePartsInfo.FirstOrDefaultAsync(sp => sp.Id == id && sp.RelatedToDevice == DeviceType.ATM);

            if (sparePart == null)
                return NotFound("Spare part for ATM not found .");

            if (sparePart.RelatedToDevice != DeviceType.ATM)
                return BadRequest($"It is not an ATM. It is a {sparePart.RelatedToDevice}.");

            _context.SparePartsInfo.Remove(sparePart);
            await _context.SaveChangesAsync();

            return Ok($"Deleted spare part with {id} ID for ATM");
        }

        [HttpGet("Kiosk")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllSparePartsForKiosk()
        {
            var spareParts = await _context.SparePartsInfo
                .Where(sp => sp.RelatedToDevice == DeviceType.Kiosk)
                .Select(sp => new
                {
                    sp.Id,
                    sp.PartName,
                    sp.PartNumber,
                    sp.SerialNumber,
                    sp.StockQuantity,
                    sp.UsedQuantity,
                    sp.TotalPurchasedQuantity,
                    sp.Type,
                    v = sp.RelatedToDevice.ToString(),
                })
                .ToListAsync();

            return Ok(spareParts);
        }

        [HttpGet("Other")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllSparePartsForOther()
        {
            var spareParts = await _context.SparePartsInfo
                .Where(sp => sp.RelatedToDevice == DeviceType.Other)
                .Select(sp => new
                {
                    sp.Id,
                    sp.PartName,
                    sp.PartNumber,
                    sp.SerialNumber,
                    sp.StockQuantity,
                    sp.UsedQuantity,
                    sp.TotalPurchasedQuantity,
                    sp.Type,
                    v = sp.RelatedToDevice.ToString(),
                })
                .ToListAsync();

            return Ok(spareParts);
        }
    }
}
