using ParkyAPI.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Repos.NationalParkRepos
{
    public interface INationalParkRepo
    {
        Task<ICollection<NationalParkDto>> GetAllParks();
        Task<NationalParkDto> GetParkById(int id);
        Task<bool> NationalParkExists(string name);
        Task<bool> NationalParkExists(int id);
        Task<bool> CreateNationalPark(NationalParkDto park);
        Task<bool> UpdateNationalPark(NationalParkDto park);
        Task<bool> DeleteNationalPark(int id);
        Task<bool> Save();
    }
}
