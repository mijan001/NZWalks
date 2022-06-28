using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }


        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await
                 nZWalksDbContext.Walks
                 .Include(x => x.Region)
                 .Include(x => x.WalkDifficulty)
                 .ToListAsync();
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            // Assign New ID
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);

            if(existingWalk == null)
            {
                return null;
            }

            nZWalksDbContext.Walks.Remove(existingWalk);
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalk;


        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await
                nZWalksDbContext.Walks
               .Include(x => x.Region)
               .Include(x => x.WalkDifficulty)
               .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            //var existingWalk = await nZWalksDbContext.Walks
            //    .Include(x => x.Region)
            //    .Include(x => x.WalkDifficulty)
            //    .FirstOrDefaultAsync(x => x.Id == id);

            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);

            //if(existingWalk == null)
            //{
            //    return null;
            //}

            //existingWalk.Name = walk.Name;
            //existingWalk.Length = walk.Length;
            //existingWalk.RegionId = walk.RegionId;
            //existingWalk.WalkDifficultyId = walk.WalkDifficultyId;

            if (existingWalk != null)
            {
                existingWalk.Name = walk.Name;
                existingWalk.Length = walk.Length;
                existingWalk.RegionId = walk.RegionId;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;

                await nZWalksDbContext.SaveChangesAsync();
                return existingWalk;
            }

            //await nZWalksDbContext.SaveChangesAsync();
            //return existingWalk;
            return null;

        }

       
    }
}
