using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class SqlRegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext _context;
    public SqlRegionRepository(NZWalksDbContext dbContext)
    {
        _context = dbContext;
    }
    public async Task<List<Region>> GetAllAsync(string? filterOn=null,string? filterQuery=null,
        string? sortBy=null,bool isAscending=true,
        int pageNumber=1,int pageSize=1000)
    {
        var regions = _context.Regions.AsQueryable();
        //filter the regions if filterOn and filterQuery are not null
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                regions = regions.Where(x => x.Name.Contains(filterQuery.Trim()));
            }
            else if (filterOn.Equals("Code", StringComparison.OrdinalIgnoreCase))
            {
                regions = regions.Where(x => x.Code.Contains(filterQuery.Trim()));
            }
        } 
        //sort the regions if sortBy is not null
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                regions = isAscending ? regions.OrderBy(x => x.Name) : regions.OrderByDescending(x => x.Name);
            }
            else if (sortBy.Equals("Code", StringComparison.OrdinalIgnoreCase))
            {
                regions = isAscending ? regions.OrderBy(x => x.Code) : regions.OrderByDescending(x => x.Code);
            }
        }
        //paginate the regions
        var skipResults = (pageNumber - 1) * pageSize;
        regions = regions.Skip(skipResults).Take(pageSize);
        return await regions.ToListAsync();
    }

    public async Task<Region?> GetByIdAsync(Guid id)
    {
        return await _context.Regions.FirstOrDefaultAsync(x=>x.Id==id);    
    }

    public async Task<Region> CreateAsync(Region region)
    {
        await _context.Regions.AddAsync(region);
        await _context.SaveChangesAsync();
        return region;
    }

    public async Task<Region?> UpdateAsync(Guid id, Region region)
    {
        var existingRegion = await _context.Regions.FirstOrDefaultAsync(x=>x.Id==id);
        if(existingRegion is null)
        {
            return null;
        }
        existingRegion.Code = region.Code;
        existingRegion.Name = region.Name;
        existingRegion.RegionImageUrl = region.RegionImageUrl;
        await _context.SaveChangesAsync();
        return existingRegion;
    }

    public async Task<Region> DeleteAsync(Guid id)
    {
        var existingRegion = await _context.Regions.FirstOrDefaultAsync(x=>x.Id==id);
        if(existingRegion is null)
        {
            return null;
        }
        _context.Regions.Remove(existingRegion);
        await _context.SaveChangesAsync();
        return existingRegion;
    }
}