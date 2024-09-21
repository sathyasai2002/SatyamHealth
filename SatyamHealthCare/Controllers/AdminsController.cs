using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.DTO;
using SatyamHealthCare.Exceptions;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Repos;

namespace SatyamHealthCare.Controllers
{
    [Authorize(Roles = "Admin")]
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
            try
            {
                var result = await admin1.GetAdminById(id);

                if (result == null)
                {
                    throw new AdminNotFoundException($"Admin with ID {id} not found.");
                }

                return Ok(result);
            }
            catch (AdminNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
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

            try
            {
                admin1.UpdateAdmin(admin);
                await admin1.Save();

                return Ok(admin);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
                {
                    throw new AdminNotFoundException($"Admin with ID {id} not found.");
                }
                else
                {
                    throw new AdminUpdateException($"Concurrency issue occurred while updating Admin with ID {id}.");
                }
            }
            catch (AdminNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (AdminUpdateException ex)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { Message = ex.Message });
            }
        }
        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin([FromBody] AdminDTO adminDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var admin = new Admin
            {
                FullName = adminDTO.FullName,
                Email = adminDTO.Email,
                Password = adminDTO.Password
            };
            await admin1.AddAdmin(admin);
            await admin1.Save();



            // Ensure GetAdminById is a valid action name in your controller
            return CreatedAtAction(nameof(GetAdminById), new { id = admin.AdminId }, admin);
        }


        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            try
            {
                var admin = await admin1.GetAdminById(id);

                if (admin == null)
                {
                    throw new AdminNotFoundException($"Admin with ID {id} not found.");
                }

                bool deleted = await admin1.DeleteAdmin(id);
                if (!deleted)
                {
                    throw new AdminDeleteException($"Error occurred while deleting Admin with ID {id}.");
                }

                return NoContent();
            }
            catch (AdminNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (AdminDeleteException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }


        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
    }
}
