using ClinicQueueDataBaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicQueueDataBaseImplement
{
    public class ClinicQueueDataBase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=ClinicQueueDataBase;Username=postgres;Password=postgres");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<ElectronicQueue> ElectronicQueues { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
    }
}
