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
        public async Task<ActionResult<CelestialBody>> PostCelestialBody(CelestialBody celestialBody)
        {
            _context.CelestialBodies.Add(celestialBody);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCelestialBody", new { id = celestialBody.Id }, celestialBody);
        }
    }
}