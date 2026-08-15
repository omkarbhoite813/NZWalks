using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Data.Common;

namespace NZWalks.API.Repository
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }
        public async Task<List<Walk>> GetAllAsync()
        {
            //return await dbContext.Walks.ToListAsync();
            return await dbContext.Walks.
                Include("Difficulty")
                .Include("Region").
                ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var WalkDto = await dbContext.Walks.
                Include("Difficulty").
                Include("Region").
                FirstOrDefaultAsync(x => x.Id == id);

            if (WalkDto == null)
            {
                return null;
            }
            return WalkDto;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x=> x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUri = walk.WalkImageUri;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var WalkDomain = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(WalkDomain == null) return null;

            dbContext.Walks.Remove(WalkDomain);
            await dbContext.SaveChangesAsync();
            return WalkDomain;
        }


    }
}
