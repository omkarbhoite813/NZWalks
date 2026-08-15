using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Region Mapping
            CreateMap<Region, RegionDto>().ReverseMap();

            CreateMap<Region , AddRegionRequestDto>().ReverseMap();

            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

            // Walk Mapping

            CreateMap<Walk , AddWalksRequestDto>().ReverseMap();

            CreateMap<Walk, WalkDto>().ReverseMap();

            CreateMap<UpdateWalkRequestDto,Walk>().ReverseMap();

            // Difficulty Mapping

            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}
