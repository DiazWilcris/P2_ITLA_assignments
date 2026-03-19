using StarAtlas.API.Models.Dtos;
using StarAtlas.Application.Dtos;  
using StarAtlas.Application.Responses; 
using StarAtlas.Domain.Entities;
using StarAtlas.Infrastructure.Repositories;

namespace StarAtlas.Application.Services
{
    public class BodyTypeService
    {
        private readonly UnitOfWork _unitOfWork;

        public BodyTypeService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<BodyTypeDto>>> GetAllBodyTypesAsync()
        {
            var types = await _unitOfWork.BodyTypeRepository.GetAllAsync();

            var dtos = types.Select(t => new BodyTypeDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();

            return new ApiResponse<IEnumerable<BodyTypeDto>>(dtos, "Body types retrieved successfully.");
        }

        public async Task<ApiResponse<BodyTypeDto>> CreateBodyTypeAsync(BodyTypeDto dto)
        {
            var bodyType = new BodyType
            {
                Name = dto.Name
            };

            await _unitOfWork.BodyTypeRepository.AddAsync(bodyType);
            await _unitOfWork.CompleteAsync();

            dto.Id = bodyType.Id; 

            return new ApiResponse<BodyTypeDto>(dto, "Body type created successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateBodyTypeAsync(int id, BodyTypeDto dto)
        {
            if (id != dto.Id)
            {
                return new ApiResponse<bool>("The URL ID does not match the request body ID.");
            }

            var existingType = await _unitOfWork.BodyTypeRepository.GetByIdAsync(id);
            if (existingType == null)
            {
                return new ApiResponse<bool>($"Body type with ID {id} not found.");
            }

            existingType.Name = dto.Name;
            _unitOfWork.BodyTypeRepository.Update(existingType);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<bool>(true, "Body type updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteBodyTypeAsync(int id)
        {
            var bodyType = await _unitOfWork.BodyTypeRepository.GetByIdAsync(id);
            if (bodyType == null)
            {
                return new ApiResponse<bool>($"Body type with ID {id} not found.");
            }

            var allBodies = await _unitOfWork.CelestialBodyRepository.GetAllAsync();
            var isUsed = allBodies.Any(c => c.BodyTypeId == id);

            if (isUsed)
            {
                return new ApiResponse<bool>("Cannot delete this type because it is assigned to existing Celestial Bodies.");
            }

            _unitOfWork.BodyTypeRepository.Delete(bodyType);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<bool>(true, "Body type deleted successfully.");
        }
    }
}