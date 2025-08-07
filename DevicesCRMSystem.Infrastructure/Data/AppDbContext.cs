using DevicesCRMSystem.Domain.Entities.Devices;
using DevicesCRMSystem.Domain.Entities.SpareParts;
using DevicesCRMSystem.Domain.Entities.Devices.ATM.Maintenance;
//using DevicesCRMSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DevicesCRMSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DeviceInfo> DeviceInfo { get; set; }
        //public DbSet<UsersInfo> UsersInfo { get; set; }
        public DbSet<SparePartsInfo> SparePartsInfo { get; set; }  
        public DbSet<Request> MaintenanceRequest { get; set; }
        public DbSet<Record> MaintenanceRecord { get; set; }
        public DbSet<UsedSparePart> MaintenanceUsedSparePart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Store DeviceType and Status enums as strings
            modelBuilder.Entity<DeviceInfo>(entity =>
            {
                entity.Property(e => e.DeviceType)
                      .HasConversion<string>();

                entity.Property(e => e.Status)
                      .HasConversion<string>();
            });


            modelBuilder.Entity<Request>(entity =>
            {
                entity.Property(e => e.Status)
                      .HasConversion<string>();

                entity.Property(e => e.MaintenanceType)
                      .HasConversion<string>();
            });

            //modelBuilder.Entity<UsersInfo>(entity =>
            //{
            //    entity.Property(e => e.Role)
            //          .HasConversion<string>();
            //});

            

            modelBuilder.Entity<Record>()
                .HasOne(r => r.MaintenanceRequest)
                .WithMany()
                .HasForeignKey(r => r.MaintenanceRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsedSparePart>()
                .HasOne(us => us.MaintenanceRecord)
                .WithMany(r => r.UsedSpareParts)
                .HasForeignKey(us => us.MaintenanceRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsedSparePart>()
                .HasOne(us => us.SparePart)
                .WithMany()
                .HasForeignKey(us => us.SparePartId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SparePartsInfo>()
                .Property(sp => sp.RelatedToDevice)
                .HasConversion<string>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
