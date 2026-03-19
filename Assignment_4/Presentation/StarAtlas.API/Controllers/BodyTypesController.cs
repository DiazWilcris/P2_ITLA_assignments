using Microsoft.AspNetCore.Mvc;
using StarAtlas.API.Models.Dtos;
using StarAtlas.Application.Services;

namespace StarAtlas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BodyTypesController : ControllerBase
    {
        private readonly BodyTypeService _bodyTypeService;

        public BodyTypesController(BodyTypeService bodyTypeService)
        {
            _bodyTypeService = bodyTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBodyTypes()
        {
            var response = await _bodyTypeService.GetAllBodyTypesAsync();

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostBodyType(BodyTypeDto dto)
        {
            var response = await _bodyTypeService.CreateBodyTypeAsync(dto);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetBodyTypes), new { id = response.Data?.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBodyType(int id, BodyTypeDto dto)
        {
            var response = await _bodyTypeService.UpdateBodyTypeAsync(id, dto);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBodyType(int id)
        {
            var response = await _bodyTypeService.DeleteBodyTypeAsync(id);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}