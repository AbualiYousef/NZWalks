using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public interface IRegionRepository
{
    Task<List<Region>> GetAllAsync(string? filterOn=null,string? filterQuery=null,
        string? sortBy=null,bool isAscending=true,
        int pageNumber=1,int pageSize=1000);
    Task<Region?> GetByIdAsync(Guid id);
    Task<Region> CreateAsync(Region region);
    Task<Region?> UpdateAsync(Guid id, Region region);
    Task<Region?> DeleteAsync(Guid id);
}