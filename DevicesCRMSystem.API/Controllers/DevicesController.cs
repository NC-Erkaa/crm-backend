using DevicesCRMSystem.Application.DTOs.Devices.ATM;
using DevicesCRMSystem.Application.DTOs.Devices.Kiosk;
using DevicesCRMSystem.Application.Enums;
using DevicesCRMSystem.Domain.Entities.Devices;
using DevicesCRMSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace DevicesCRMSystem.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DevicesController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<object>>> GetAllDevices()
        //{
        //    var devices = await _context.DeviceInfo
        //        .Select(d => new
        //        {
        //            d.Id,
        //            d.DeviceName,
        //            d.SerialNumber,
        //            d.Model,
        //            DeviceType = d.DeviceType.ToString(),
        //            Status = d.Status.HasValue ? d.Status.ToString() : null,
        //            d.IP,
        //            d.Port,
        //            d.Location,
        //            d.Province,
        //            d.Masterkey,
        //            d.InstallationDate,
        //            d.ExpiredDate,
        //            d.OwnerId,
        //            d.OrgName
        //        })
        //        .ToListAsync();

        //    return Ok(devices);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetDeviceById(int id)
        {
            var devices = await _context.DeviceInfo
                .Where(d => d.Id == id)
                .Select(d => new
                {
                    d.Id,
                    d.DeviceName,
                    d.SerialNumber,
                    d.Model,
                    DeviceType = d.DeviceType.ToString(),
                    Status = d.Status.HasValue ? d.Status.ToString() : null,
                    d.IP,
                    d.Port,
                    d.Location,
                    d.Province,
                    //d.Masterkey,
                    d.InstallationDate,
                    d.ExpiredDate,
                    d.CreatedBy,
                    d.OrgName,
                    d.OrgId
                })
                .ToListAsync();

            if (devices == null || !devices.Any()) return NotFound($"{id} ID-тай төхөөрөмж олдсонгүй");
            return Ok(devices);
        }


        [HttpPost("ATM")]
        public async Task<ActionResult<DeviceInfo>> CreateATM([FromBody] CreateAtmDto dto)
        {
            var device = new DeviceInfo
            {
                DeviceName = dto.DeviceName,
                SerialNumber = dto.SerialNumber,
                Model = dto.Model,
                DeviceType = dto.DeviceType,
                Status = dto.Status,
                IP = dto.IP,
                Port = dto.Port,
                Location = dto.Location,
                Province = dto.Province,
                CreatedBy = dto.CreatedBy,
                InstallationDate = dto.InstallationDate,
                AtmZone = dto.AtmZone,
                OrgId = dto.OrgId
      
            };

            if (!string.IsNullOrEmpty(device.SerialNumber))
            {
                if (device.SerialNumber.StartsWith("430"))
                {
                    device.ExpiredDate = new DateOnly(2022, 6, 22);
                }
                else if (device.SerialNumber.StartsWith("493"))
                {
                    device.ExpiredDate = new DateOnly(2023, 5, 31);
                }
                else if (device.SerialNumber.StartsWith("561"))
                {
                    device.ExpiredDate = new DateOnly(2024, 5, 31);
                }
                else if (device.SerialNumber.StartsWith("622"))
                {
                    device.ExpiredDate = new DateOnly(2025, 6, 12);
                }
                else if (device.SerialNumber.StartsWith("684"))
                {
                    device.ExpiredDate = new DateOnly(2026, 6, 28);
                }
                else if (device.SerialNumber.StartsWith("686"))
                {
                    device.ExpiredDate = new DateOnly(2026, 6, 28);
                }
                else if (device.SerialNumber.StartsWith("999"))
                {
                    device.ExpiredDate = new DateOnly(2027, 7, 3);
                }

            }
            _context.DeviceInfo.Add(device);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
        }
        
        
        




        [HttpGet("ATM")]
        //[Authorize(Roles = "Admin" )]
        public async Task<ActionResult<IEnumerable<object>>> GetAllATMs()
        {
            var atms = await _context.DeviceInfo
                .Where(d => d.DeviceType == DeviceType.ATM)
                .Select(d => new
                {
                    d.Id,
                    d.DeviceName,
                    d.SerialNumber,
                    d.Model,
                    DeviceType = d.DeviceType.ToString(),
                    Status = d.Status.HasValue ? d.Status.ToString() : null,
                    d.IP,
                    d.Port,
                    DetailedLocation = d.Province + " - " + d.Location,
                    //d.Province,
                    //masterKey = d.Masterkey,
                    d.InstallationDate,
                    d.ExpiredDate,
                    d.CreatedBy,
                    d.OrgName, //get it by orgId
                    d.AtmZone,
                    d.OrgId
                })
                .ToListAsync();

            return Ok(atms);
        }

        


        [HttpPatch("ATM/{id}")]
        public async Task<IActionResult> UpdateATM(int id, DeviceInfo updatedATM)
        {
            if (id != updatedATM.Id)
                return BadRequest();

            var atm = await _context.DeviceInfo
                .FirstOrDefaultAsync(d => d.Id == id && d.DeviceType == DeviceType.ATM);

            if (atm == null)
                return NotFound();

            
            if (updatedATM.DeviceName != null) atm.DeviceName = updatedATM.DeviceName;
            if (updatedATM.SerialNumber != null) atm.SerialNumber = updatedATM.SerialNumber;
            if (updatedATM.Model != null) atm.Model = updatedATM.Model;
            if (updatedATM.Status != null) atm.Status = updatedATM.Status;
            if (updatedATM.IP != null) atm.IP = updatedATM.IP;
            if (updatedATM.Port != null) atm.Port = updatedATM.Port;
            if (updatedATM.Province != null) atm.Province = updatedATM.Province;
            if (updatedATM.Location != null) atm.Location = updatedATM.Location;
            if (updatedATM.InstallationDate != null) atm.InstallationDate = updatedATM.InstallationDate;
            if (updatedATM.ExpiredDate != null) atm.ExpiredDate = updatedATM.ExpiredDate;
            if (updatedATM.CreatedBy != null) atm.CreatedBy = updatedATM.CreatedBy;
            //if (updatedATM.OrgName != null) atm.OrgName = updatedATM.OrgName;
            if (updatedATM.AtmZone != null) atm.AtmZone = updatedATM.AtmZone;
            if (updatedATM.OrgId != null) atm.OrgId = updatedATM.OrgId;

            await _context.SaveChangesAsync();

            return Ok($"Updated information for ATM with ID: {id}");
        }



        [HttpDelete("ATM/{id}")]
        public async Task<IActionResult> DeleteATM(int id)
        {
            var device = await _context.DeviceInfo.FirstOrDefaultAsync(d => d.Id == id);

            if (device == null)
                return NotFound("ATM not found.");

            if (device.DeviceType != DeviceType.ATM)
                return BadRequest($"It is not an ATM. It is a {device.DeviceType}.");

            _context.DeviceInfo.Remove(device);
            await _context.SaveChangesAsync();

            return Ok($"Deleted ATM with {id} ID");
        }


        [HttpGet("Kiosk")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllKiosks()
        {
            var kiosks = await _context.DeviceInfo
                .Where(d => d.DeviceType == DeviceType.Kiosk)
                .Select(d => new
                {
                    d.Id,
                    Нэр = d.DeviceName,
                    d.SerialNumber,
                    Загвар = d.Model,
                    ТөхөөрөмжийнТөлөв = d.DeviceType.ToString(),
                    Status = d.Status.HasValue ? d.Status.ToString() : null,
                    d.IP,
                    d.Port,
                    d.Location,
                    d.Province,
                    //d.Masterkey,
                    d.InstallationDate,
                    d.ExpiredDate,
                    d.CreatedBy,
                    БайгууллагынНэр = d.OrgName
                })
                .ToListAsync();

            return Ok(kiosks);
        }


        [HttpPost("Kiosk")]
        public async Task<ActionResult<DeviceInfo>> CreateKiosk([FromBody] CreateKioskDto dto)
        {
            var device = new DeviceInfo
            {
                DeviceName = dto.DeviceName,
                SerialNumber = dto.SerialNumber,
                //Model = dto.Model,
                //DeviceType = dto.DeviceType,
                //Status = dto.Status,
                //IP = dto.IP,
                //Port = dto.Port,
                //Location = dto.Location,
                //Province = dto.Province,
                //CreatedBy = dto.CreatedBy,
                //InstallationDate = dto.InstallationDate,
                //AtmZone = dto.AtmZone,
                //OrgId = dto.OrgId

            };

            
            _context.DeviceInfo.Add(device);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
        }


        [HttpGet("OtherDevices")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllOtherDevices()
        {
            var otherDevices = await _context.DeviceInfo
                .Where(d => d.DeviceType == DeviceType.Other)
                .Select(d => new
                {
                    d.Id,
                    d.DeviceName,
                    d.SerialNumber,
                    d.Model,
                    DeviceType = d.DeviceType.ToString(),
                    Status = d.Status.HasValue ? d.Status.ToString() : null,
                    d.IP,
                    d.Port,
                    d.Location,
                    d.Province,
                    //d.Masterkey,
                    d.InstallationDate,
                    d.ExpiredDate,
                    d.CreatedBy,
                    d.OrgName
                })
                .ToListAsync();

            return Ok(otherDevices);
        }



    }
}
