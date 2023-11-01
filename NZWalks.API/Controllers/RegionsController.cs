
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;


namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        //Fields
        private readonly NZWalksDbContext _context;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        //Constructor
        public RegionsController(NZWalksDbContext context,
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            _context= context;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region GetAllRegions
        [HttpGet]
        // [Authorize(Roles="Reader")]
        // GET: api/Regions?filterOn=name&filterQuery=Track$SortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        public async Task<IActionResult> GetAllRegions([FromQuery]string? filterOn,[FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,[FromQuery] bool? isAscending,
            [FromQuery] int pageNumber=1,[FromQuery] int pageSize=1000)
        {
             //Get all regions from the database
                var regionsDomain= await _regionRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending ?? true,pageNumber,pageSize);
                //Log the data returned
                _logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");
                //Map the regions to a list of RegionDTOs
                var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
                //Return the list of RegionDTOs
                return Ok(regionsDto);
        }//end of GetAllRegions
        #endregion
        
        #region GetRegionById
        // GET: api/Regions By Id
        [HttpGet]
        [Route("{id:guid}")]
        // [Authorize(Roles="Reader")]
        public async Task<IActionResult> GetRegionById([FromRoute]Guid id)
        {
            //Get the region from the database
            var regionDomain = await _regionRepository.GetByIdAsync(id);
            if (regionDomain is null)
            {
                return NotFound();
            }
            //Map the region to a RegionDTO
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            //Return the RegionDTO
            return Ok(regionDto);
        }//end of GetRegionById
        #endregion GetAllRegion

        #region CreateRegion
        // POST: api/Regions
        [HttpPost]
        [ValidateModel]
        // [Authorize(Roles="Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {
            //Map the AddRegionRequestDTO to a RegionDomain
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
            //Use Domain Model to create a new Region
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);
            //Save the changes to the database
            await _context.SaveChangesAsync();
            //Map the RegionDomain to a RegionDTO
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            //Return the RegionDTO
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDomainModel.Id }, regionDto);
        }//end of CreateRegion
        #endregion

        #region UpdateRegion
        // PUT: api/Regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        // [Authorize(Roles="Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id,[FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map the UpdateRegionRequestDTO to a RegionDomain
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
            //Check if the region exists
            regionDomainModel= await _regionRepository.UpdateAsync(id,regionDomainModel);
            
            if(regionDomainModel is null)
            {
                return NotFound();
            }
            //convert the RegionDomain to a RegionDTO
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
          
            //Return the RegionDTO
            return Ok(regionDto); 
        }//end of UpdateRegion
        #endregion
        
        #region DeleteRegion
        // DELETE: api/Regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        // [Authorize(Roles="Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            //Check if the region exists
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            
            if(regionDomainModel is null)
            {
                return NotFound();
            }
            
            //return deleted region to the client
            //Map the RegionDomain to a RegionDTO
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }//end of DeleteRegion
        #endregion
    }//end of class
}//end of namespace