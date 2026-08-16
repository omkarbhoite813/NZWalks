using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper , IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            // Validate the incoming request
            //if(ModelState.IsValid == false) return BadRequest(ModelState);
            
            // Dto to model
            var walkDomainMode =  mapper.Map<Walk>(addWalksRequestDto);
            await walkRepository.CreateAsync(walkDomainMode);
            return Ok(mapper.Map<WalkDto>(walkDomainMode));
        }


        //api/walks?filterOn=name&filterQuery=Track&sortBy=name&isAssending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetWalk([FromQuery] string? filterOn , [FromQuery] string? filterQuery , 
            [FromQuery] string? sortOn , [FromQuery] bool? isAssending , 
            [FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 10 )
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortOn, isAssending ?? true , pageNumber, pageSize);
            
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }


        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Validate the incoming request
            //if(ModelState.IsValid==false) return BadRequest(ModelState);

            // DTO To model 
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);
            if(walkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


    }
}
