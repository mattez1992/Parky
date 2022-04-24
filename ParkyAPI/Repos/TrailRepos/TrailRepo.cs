using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Repos.TrailRepos
{
    public class TrailRepo : ITrailRepo
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public TrailRepo(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _dbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateTrail(Trail newTrail)
        {
            
            _dbContext.Trails.Add(newTrail);
            return await Save();
        }

        public async Task<bool> DeleteTrail(int id)
        {
            var trailToDelete = await _dbContext.Trails.FirstOrDefaultAsync(x => x.Id == id);
            if (trailToDelete != null)
            {
                _dbContext.Trails.Remove(trailToDelete);
            }
            return await Save();
        }

        public async Task<ICollection<ReadTrailDto>> GetAllTrails()
        {
            var trailDtos = _mapper.Map<List<ReadTrailDto>>(await _dbContext.Trails.Include(x => x.NationalPark).ToListAsync());
            return trailDtos;
        }

        public async Task<ReadTrailDto> GetTrailById(int id)
        {
            var trailDto = _mapper.Map<ReadTrailDto>(await _dbContext.Trails.Include(x => x.NationalPark).FirstOrDefaultAsync(x => x.Id == id));
            return trailDto;
        }
        public async  Task<ICollection<Trail>> GetTrailsInNationalPark(int parkId)
        {
            return await _dbContext.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == parkId).ToListAsync();
        }
        public async Task<bool> TrailExists(string name)
        {
            bool exists = await _dbContext.Trails.AnyAsync(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return exists;
        }

        public async Task<bool> TrailExists(int id)
        {
            return await _dbContext.Trails.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> Save()
        {
            return (await _dbContext.SaveChangesAsync()) >= 0;
        }

        public async Task<bool> UpdateTrail(TrailUpdateDto trailDto)
        {
            var updatePark = _mapper.Map<Trail>(trailDto);
            _dbContext.Trails.Update(updatePark);
            return await Save();
        }
    }
}
