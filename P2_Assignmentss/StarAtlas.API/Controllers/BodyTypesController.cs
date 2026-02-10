using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarAtlas.API.Data;
using StarAtlas.API.Models.Entities;
using StarAtlas.API.Models.Dtos;

namespace StarAtlas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BodyTypesController : ControllerBase
    {
        private readonly StarAtlasContext _context;

        public BodyTypesController(StarAtlasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BodyTypeDto>>> GetBodyTypes()
        {
            var types = await _context.BodyTypes.ToListAsync();

            var dtos = types.Select(t => new BodyTypeDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<BodyType>> PostBodyType(BodyType bodyType)
        {
            _context.BodyTypes.Add(bodyType);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBodyTypes", new { id = bodyType.Id }, bodyType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBodyType(int id, BodyType bodyType)
        {
            if (id != bodyType.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the body.");
            }

            _context.Entry(bodyType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BodyTypeExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBodyType(int id)
        {
            var bodyType = await _context.BodyTypes.FindAsync(id);
            if (bodyType == null)
            {
                return NotFound();
            }

            var isUsed = await _context.CelestialBodies.AnyAsync(c => c.BodyTypeId == id);
            if (isUsed)
            {
                return BadRequest("Cannot delete this type because it is assigned to existing Celestial Bodies. Delete the stars first.");
            }

            _context.BodyTypes.Remove(bodyType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BodyTypeExists(int id)
        {
            return _context.BodyTypes.Any(e => e.Id == id);
        }
    }
}