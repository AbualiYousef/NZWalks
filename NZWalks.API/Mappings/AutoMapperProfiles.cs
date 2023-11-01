using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Region,RegionDto>().ReverseMap();
        CreateMap<Walk,WalkDto>().ReverseMap();
        CreateMap<Difficulty,DifficultyDto>().ReverseMap();
        CreateMap<AddRegionRequestDto,Region>().ReverseMap();
        CreateMap<UpdateRegionRequestDto,Region>().ReverseMap();
        CreateMap<AddWalkRequestDto,Walk>().ReverseMap();
        CreateMap<UpdateWalkRequestDto,Walk>().ReverseMap();

    }
}