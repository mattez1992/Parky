using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Repos.NationalParkRepos
{
    public class NationalParkRepo : INationalParkRepo
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public NationalParkRepo(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateNationalPark(NationalParkDto park)
        {
            var newPark = _mapper.Map<NationalPark>(park);
            _dbContext.NationalParks.Add(newPark);
            return await Save();
        }

        public async Task<bool> DeleteNationalPark(int id)
        {
            var parkToDelete = await _dbContext.NationalParks.FirstOrDefaultAsync(x => x.Id == id);
            if (parkToDelete != null)
            {
                _dbContext.NationalParks.Remove(parkToDelete);
            }
            return await Save();
        }

        public async Task<ICollection<NationalParkDto>> GetAllParks()
        {
            var parkDtos = _mapper.Map<List<NationalParkDto>>(await _dbContext.NationalParks.ToListAsync());
            return parkDtos;
        }

        public async Task<NationalParkDto> GetParkById(int id)
        {
            var parkDto = _mapper.Map<NationalParkDto>(await _dbContext.NationalParks.FirstOrDefaultAsync(x => x.Id == id));
            return parkDto;
        }

        public async Task<bool> NationalParkExists(string name)
        {
            bool exists = await _dbContext.NationalParks.AnyAsync(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return exists;
        }

        public async Task<bool> NationalParkExists(int id)
        {
            return await _dbContext.NationalParks.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> Save()
        {
            return (await _dbContext.SaveChangesAsync()) >= 0;
        }

        public async Task<bool> UpdateNationalPark(NationalParkDto park)
        {
            var updatePark = _mapper.Map<NationalPark>(park);
            _dbContext.NationalParks.Update(updatePark);
            return await Save();
        }
    }
}
