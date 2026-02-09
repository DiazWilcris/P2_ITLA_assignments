using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarAtlas.API.Data;
using StarAtlas.API.Models.Entities;
using StarAtlas.API.Models.Dtos;

namespace StarAtlas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        private readonly StarAtlasContext _context;

        public ObservationsController(StarAtlasContext context)
        {
            _context = context;
        }

        [HttpGet("star/{celestialBodyId}")]
        public async Task<ActionResult<IEnumerable<ObservationDto>>> GetObservationsByBody(int celestialBodyId)
        {
            var observations = await _context.Observations
                                             .Include(o => o.CelestialBody) 
                                             .Where(o => o.CelestialBodyId == celestialBodyId)
                                             .OrderByDescending(o => o.ObservationDate) 
                                             .Select(o => new ObservationDto
                                             {
                                                 Id = o.Id,
                                                 Date = o.ObservationDate,
                                                 Location = o.Location ?? "Unknown", 
                                                 Note = o.PersonalNote,
                                                 CelestialBodyName = o.CelestialBody != null ? o.CelestialBody.Name : "Unknown"
                                             })
                                             .ToListAsync();

            if (!observations.Any())
            {
                return NotFound("No observations found for this celestial body.");
            }

            return Ok(observations);
        }

        [HttpPost]
        public async Task<ActionResult<ObservationDto>> CreateObservation(CreateObservationDto dto)
        {
            var bodyExists = await _context.CelestialBodies.AnyAsync(b => b.Id == dto.CelestialBodyId);
            if (!bodyExists)
            {
                return BadRequest("Celestial Body ID not found.");
            }

            var observation = new Observation
            {
                CelestialBodyId = dto.CelestialBodyId,
                PersonalNote = dto.PersonalNote,
                Location = dto.Location,
                ObservationDate = DateTime.Now
            };

            _context.Observations.Add(observation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetObservationsByBody),
                new { celestialBodyId = observation.CelestialBodyId },
                new { Message = "Observation recorded successfully!", Id = observation.Id });
        }
    }
}