using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]

    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            // Kann man so machen, da wir nur ein Feld haben!
            var walkDifficultiesDomain = await walkDifficultyRepository.GetAllAsync();

            // Convert Domain to DTOs
            var walkDifficultiesDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultiesDomain);

            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // Convert Domain to DTOs
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(
            Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)

       
        {
            // Validate the Request
            if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };

            // Call repository
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            // Convert Domian to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // Return response
            return CreatedAtAction(nameof(GetWalkDifficultyById),
                new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody]
                Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Validate the Request
            if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain Model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code
            };

            // Call repository to update
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            // If Null Then NotFound
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code,
            };
            // Alternative durch Mapping
            //var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // Return Ok response
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            // Get walkDifficulty from database
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

            // If null NotFound
            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }
            // Convert response back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code,
            };
            // Alternative durch Mapping
            //var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // return Ok response
            return Ok(walkDifficultyDTO);
        }

        #region private methods

        public bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if( addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest),
                   $"{nameof(addWalkDifficultyRequest)} Cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
                    $"{nameof(addWalkDifficultyRequest.Code)} is required. ");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        public bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                   $"{nameof(updateWalkDifficultyRequest)} Cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                    $"{nameof(updateWalkDifficultyRequest.Code)} is required. ");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion


    }
}