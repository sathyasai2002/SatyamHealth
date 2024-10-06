using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Linq;

namespace SatyamHealthCare.Repos
{
    public class PrescriptionService : IPrescription
    {
        private readonly SatyamDbContext _context;
        private readonly IConverter _converter;

        public PrescriptionService(SatyamDbContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public async Task AddPrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync()
        {
            return await _context.Prescriptions
                .Include(p => p.PrescriptionMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .Include(p => p.Appointment)
                .Include(p => p.PrescriptionTests)
                    .ThenInclude(pt => pt.Test)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prescription>> GetPrescriptionsByAppointmentIdAsync(int appointmentId)
        {
            var prescriptions = await _context.Prescriptions
                .Where(p => p.AppointmentId == appointmentId)
                .Include(p => p.PrescriptionMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .Include(p => p.PrescriptionTests)
                    .ThenInclude(pt => pt.Test)
                .ToListAsync();

            return prescriptions;
        }

        public async Task<Prescription> GetPrescriptionByIdAsync(int id)
        {
            return await _context.Prescriptions
                .Include(p => p.PrescriptionMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .Include(p => p.Appointment)
                .Include(p => p.PrescriptionTests)
                    .ThenInclude(pt => pt.Test)
                .FirstOrDefaultAsync(p => p.PrescriptionID == id);
        }

    }
}
