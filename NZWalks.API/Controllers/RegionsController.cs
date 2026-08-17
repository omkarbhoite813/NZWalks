using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using System.Linq.Expressions;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        //Constructor Dependency Injection
        public RegionsController(NZWalksDbContext dbContext ,IRegionRepository regionRepository ,IMapper mapper , ILogger<RegionsController> logger )
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        // GET : Get Method 
        [HttpGet]
        //[Authorize(Roles ="Reader")]
        public async  Task<IActionResult> GetAll()
        {
            //----Logger
            //logger.LogInformation("GetAll method called in RegionsController"); 
            //-----Get Data from Database - Domain Model
            var regionsDomain = await regionRepository.GetAllAsync();

            //-----Map Domain Model to DTOs
            /*
            var RegionDto = new List<RegionDto>(regionsDomain.Select(region => new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            }));
            */

            var regionDto = mapper.Map<List<RegionDto>>(regionsDomain);

            //logger.LogInformation($"GetAll method completed in RegionsController with data :{JsonSerializer.Serialize(regionsDomain)} ");
            //-----Return DTOs
            return Ok(regionDto);
        }


        // GET : Get Method by Id
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //-----Get Data from Database - Domain Model 
            //var region = dbContext.Regions.Find(id);
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null) return NotFound();

            //-----Map Domain Model to DTOs
            /*
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };


            //-----Return DTOs
            return Ok(regionDto);
            */
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        //POST : Create Method
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //-----Validate the Request
            //if(ModelState.IsValid == false) return BadRequest(ModelState);

            //------Map Dto to Domain Model
            /*
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };
            */
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
            //------Save to Database
            regionDomainModel =  await regionRepository.CreateAsync(regionDomainModel);

            //-----Map Domain Model to DTOs
            /*
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            */
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);


            return CreatedAtAction(nameof(GetById) , new {id = regionDto.Id}, regionDto );
        }


        //Delete : Delete Method
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //-----Get DAta from Database 
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null) return NotFound();


            //-----Map Domain Model to DTOs
            /*
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            //-----Return DTOs
            return Ok(regionDto);
            */
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }


        //PUT : Update Method
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id , [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //-----Validate the Request
            //if (ModelState.IsValid == false) return BadRequest(ModelState);

            /*
            var regionDomainModel = new Region
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            };*/
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //----Check if region exists in database
            regionDomainModel  = await regionRepository.UpdateAsync(id , regionDomainModel);
            if(regionDomainModel == null) return NotFound();

            //Conver Domain Model to DTO
            /*
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
            */
            return Ok(mapper.Map<RegionDto>(regionDomainModel));

        }
    }
}
