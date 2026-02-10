using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarAtlas.API.Data;
using StarAtlas.API.Models.Entities;
using StarAtlas.API.Models.Dtos;

namespace StarAtlas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CelestialBodiesController : ControllerBase
    {
        private readonly StarAtlasContext _context;

        public CelestialBodiesController(StarAtlasContext context)
        {
            _context = context;
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<CelestialBodyDto>> GetCelestialBody(int id)
        {
            var body = await _context.CelestialBodies
                                     .Include(c => c.BodyType)
                                     .FirstOrDefaultAsync(b => b.Id == id);

            if (body == null)
            {
                return NotFound();
            }

            var dto = new CelestialBodyDto
            {
                Id = body.Id,
                Name = body.Name,
                DistanceLightYears = body.DistanceLightYears,
                Type = body.BodyType?.Name ?? "Unknown"
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CelestialBody>> PostCelestialBody(CreateCelestialBodyDto dto)
        {
            var celestialBody = new CelestialBody
            {
                Name = dto.Name,
                Description = dto.Description,
                DistanceLightYears = dto.DistanceLightYears,
                DiscoveryDate = dto.DiscoveryDate,
                BodyTypeId = dto.BodyTypeId
            };

            _context.CelestialBodies.Add(celestialBody);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCelestialBody", new { id = celestialBody.Id }, celestialBody);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCelestialBody(int id, UpdateCelestialBodyDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo del mensaje.");
            }

            var existingBody = await _context.CelestialBodies.FindAsync(id);
            if (existingBody == null)
            {
                return NotFound($"No se encontró el astro con ID {id}");
            }

            existingBody.Name = dto.Name;
            existingBody.Description = dto.Description;
            existingBody.DistanceLightYears = dto.DistanceLightYears;
            existingBody.DiscoveryDate = dto.DiscoveryDate;
            existingBody.BodyTypeId = dto.BodyTypeId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCelestialBody(int id)
        {
            var celestialBody = await _context.CelestialBodies.FindAsync(id);
            if (celestialBody == null)
            {
                return NotFound($"Celestial Body with ID {id} not found.");
            }

            _context.CelestialBodies.Remove(celestialBody);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        private bool CelestialBodyExists(int id)
        {
            return _context.CelestialBodies.Any(e => e.Id == id);
        }
    }
}
