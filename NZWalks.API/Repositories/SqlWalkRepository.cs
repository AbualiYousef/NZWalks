using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class SqlWalkRepository: IWalkRepository
{
    private readonly NZWalksDbContext _context;
    public SqlWalkRepository(NZWalksDbContext context)
    {
        _context = context;
    }

    public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true,
        int pageNumber = 1, int pageSize = 1000)
    {
        //Get all walks from the database
        var walks= _context.Walks.Include("Difficulty").Include("Region").AsQueryable();
        //Filter the walks if filterOn and filterQuery are not null
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            //Filter the walks based on the filterOn and filterQuery
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(x => x.Name.Contains(filterQuery.Trim()));
            }
            else if (filterOn.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(x => x.LengthInKm == double.Parse(filterQuery.Trim()));
            }
        }
        //Sort the walks if sortBy is not null
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            //Sort the walks based on the sortBy and isAscending
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
            }
            else if (sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
            }
        }
        
        //Paginate the walks
        var skipResults= (pageNumber - 1) * pageSize;
        walks = walks.Skip(skipResults).Take(pageSize);

        //Return the list of walks
        return await walks.ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await _context.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x=>x.Id==id);
    }

    public async Task<Walk> CreateAsync(Walk walk)
    {
        await _context.Walks.AddAsync(walk);
        await _context.SaveChangesAsync();
        return walk;
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        //Check if the walk exists
        var walkToUpdate = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
        if (walkToUpdate is null)
        {
            return null;
        }
        //Update the walk
        walkToUpdate.Name = walk.Name;
        walkToUpdate.Description = walk.Description;
        walkToUpdate.LengthInKm = walk.LengthInKm;
        walkToUpdate.WalkImageUrl = walk.WalkImageUrl;
        walkToUpdate.DifficultyId = walk.DifficultyId;
        walkToUpdate.RegionId = walk.RegionId;
        //Save the changes
        await _context.SaveChangesAsync();
        //Return the updated walk
        return walkToUpdate;
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        //Get the walk to delete 
        var walkToDelete = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
        //Check if the walk exists in the database
        if (walkToDelete is null)
        {
            return null;
        }
        //Delete the walk
        _context.Walks.Remove(walkToDelete);
        //Save the changes
        await _context.SaveChangesAsync();
        //Return the deleted walk
        return walkToDelete;
    }
}