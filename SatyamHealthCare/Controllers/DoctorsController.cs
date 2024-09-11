using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.DTO;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Repos;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IDoctor doctor1;

        public DoctorsController(SatyamDbContext context, IDoctor doctor1)
        {
            _context = context;
            this.doctor1 = doctor1;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            var doctor =  await doctor1.GetAllDoctors();
            return Ok(doctor);
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await doctor1.GetDoctorById(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId)
            {
                return BadRequest();
            }
            doctor1.UpdateDoctor(doctor);

            try
            {
                await doctor1.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor([FromBody] DoctorDTO doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var doctor = new Doctor
            {
                FullName = doctorDto.FullName,
                PhoneNo = doctorDto.PhoneNo,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                Designation = doctorDto.Designation,
                Experience = doctorDto.Experience,
                SpecializationID = doctorDto.SpecializationID,
                Qualification = doctorDto.Qualification,
                ProfilePicture = doctorDto.ProfilePicture,
                AdminId = doctorDto.AdminId
            };
            await doctor1.AddDoctor(doctor);

            
            return Ok(new { message = "Doctor registered successfully", doctor });
       
          
        }

        // DELETE: api/Doctors/5
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await doctor1.GetDoctorById(id);
            if (doctor == null)
            {
                return NotFound();
            }

            await doctor1.DeleteDoctor(id);
            await doctor1.Save();

            return Ok();
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorId == id);
        }
    }
}
