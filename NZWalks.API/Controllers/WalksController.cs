using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        #region GetAllWalks
        // GET: api/Walks?filterOn=name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        // [Authorize(Roles="Reader")]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn,[FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,[FromQuery] bool? isAscending,
            [FromQuery] int pageNumber=1,[FromQuery] int pageSize=1000)
        {
                //Get all walks from the database
                var walksDomain = await _walkRepository.GetAllAsync(filterOn,filterQuery
                    ,sortBy,isAscending ?? true,pageNumber,pageSize);
                //Map the walks to a list of WalkDTOs
                var walksDto = _mapper.Map<List<WalkDto>>(walksDomain);
                //Return the list of WalkDTOs
                return Ok(walksDto);
        }//End of GetAllWalks
        #endregion

        #region GetWalkById
        // GET: api/Walks By Id
        [HttpGet]
        [Route("{id:guid}")]
        // [Authorize(Roles="Reader")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
               //Get the walk from the database
                var walkDomain = await _walkRepository.GetByIdAsync(id);
                if (walkDomain is null)
                {
                    return NotFound();
                }
                //Map the walkDomain to a WalkDTO
                var walkDto = _mapper.Map<WalkDto>(walkDomain);
                //Return the WalkDTO
                return Ok(walkDto);
        }//End of GetWalkById
        #endregion

        #region CreateWalk
        //Create Walk
        //Post: api/Walks
        [HttpPost]
        [ValidateModel]
        // [Authorize(Roles="Writer")]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map the DTO to the domain model
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
            //Create the walk
            await _walkRepository.CreateAsync(walkDomainModel);
            //Map the domain model back to a DTO
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
            //Return the DTO
            return Ok(walkDto);
        }//End of CreateWalk
        #endregion
        
        #region UpdateWalk
        //Update Walk
        //Put: api/Walks/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        // [Authorize(Roles="Writer")]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id,UpdateWalkRequestDto updateWalkRequestDto) {
            //Map the DTO to the domain model
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);
            //Update the walk
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);
            //check if the walk exists after the update
            if (walkDomainModel is null)
            {
                return NotFound();
            }
            //Map the domain model back to a DTO
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
            //Return the DTO
            return Ok(walkDto);
        }//End of UpdateWalk
        #endregion
        
        #region DeleteWalk
        //Delete Walk
        //Delete: api/Walks/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        // [Authorize(Roles="Writer")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            //Check if the walk exists
            var walkDomainModel = await _walkRepository.DeleteAsync(id);
            
            if (walkDomainModel is null)
            {
                return NotFound();
            }
            //Map the domain model back to a DTO
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
            //Return the DTO
            return Ok(walkDto);
        }//End of DeleteWalk
        #endregion
        
    }//End of class
}//End of namespace
