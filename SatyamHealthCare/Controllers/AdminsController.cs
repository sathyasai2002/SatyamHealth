using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Repos;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IAdmin admin1;

        public AdminsController(SatyamDbContext context, IAdmin admin1)
        {
            _context = context;
            this.admin1 = admin1;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
           var reuslt= await admin1.GetAllAdmins();
            return Ok(reuslt);
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdminById(int id)
        {
            var result = await admin1.GetAdminById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(int id, [FromBody] Admin admin)
        {
            if (id != admin.AdminId)
            {
                return BadRequest();
            }

            // _context.Entry(admin).State = EntityState.Modified;
            admin1.UpdateAdmin(admin);

            try
            {
                await admin1.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(admin);
        }
        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin([FromBody] Admin admin)
        {
            if (admin == null)
            {
                return BadRequest("Admin cannot be null.");
            }

            try
            {
                await admin1.AddAdmin(admin); // Ensure this method is awaited
                await admin1.Save(); // Ensure this method is awaited
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the database.");
            }

            // Ensure GetAdminById is a valid action name in your controller
            return CreatedAtAction(nameof(GetAdminById), new { id = admin.AdminId }, admin);
        }


        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await admin1.GetAdminById(id);
            if (admin == null)
            {
                return NotFound();
            }

            bool deleted = await admin1.DeleteAdmin(id);
            if (!deleted)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the admin.");
            }

            return NoContent();
        }


        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
    }
}
