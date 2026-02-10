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
    }
}