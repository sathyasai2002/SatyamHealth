using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Numerics;
using SatyamHealthCare.Constants;
using static SatyamHealthCare.Constants.Status;

namespace SatyamHealthCare.Models
{
    public class SatyamDbContext : DbContext
    {
        public SatyamDbContext(DbContextOptions<SatyamDbContext> options) : base(options)
        { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<MedicalHistoryFile> MedicalHistoryFiles { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Test> Tests { get; set; }
        
         public DbSet<Medicine> Medicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<Admin>()
                .HasMany(a => a.Doctors)
                .WithOne(d => d.Admin)
                .HasForeignKey(d => d.AdminId);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationID);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.MedicalRecords)
                .WithOne(mr => mr.Patient)
                .HasForeignKey(mr => mr.PatientID);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.MedicalHistoryFiles)
                .WithOne(mhf => mhf.Patient)
                .HasForeignKey(mhf => mhf.PatientId);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.MedicalRecords)
                .WithOne(mr => mr.Doctor)
                .HasForeignKey(mr => mr.DoctorID);

            /*  modelBuilder.Entity<MedicalRecord>()
                  .HasOne(mr => mr.Prescription)
                  .WithMany()
                  .HasForeignKey(mr => mr.PrescriptionID);*/
            modelBuilder.Entity<Prescription>()
                  .HasOne(p => p.Medicine)
                  .WithMany(m => m.Prescriptions)
                  .HasForeignKey(p => p.MedicineID);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Test)
                .WithMany(t => t.Prescriptions)
                .HasForeignKey(p => p.TestID);

            modelBuilder.Entity<MedicalHistoryFile>()
                .HasOne(mhf => mhf.Patient)
                .WithMany(p => p.MedicalHistoryFiles)
                .HasForeignKey(mhf => mhf.PatientId);

           /* modelBuilder.Entity<Prescription>()
                .HasMany<MedicalRecord>()
                .WithOne(mr => mr.Prescription)
                .HasForeignKey(mr => mr.PrescriptionID)
                 .OnDelete(DeleteBehavior.Restrict);*/

            modelBuilder.Entity<Appointment>()
           .Property(a => a.Status)
           .HasConversion(
               v => v.ToString(), // Convert enum to string
               v => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), v)
           );

        }



    }
    }

        

    


