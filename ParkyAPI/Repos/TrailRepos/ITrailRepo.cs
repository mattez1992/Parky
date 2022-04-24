using ParkyAPI.DTOS;

namespace ParkyAPI.Repos.TrailRepos
{
    public interface ITrailRepo
    {
        Task<bool> CreateTrail(Trail trailDto);
        Task<bool> DeleteTrail(int id);
        Task<ICollection<ReadTrailDto>> GetAllTrails();
        Task<ReadTrailDto> GetTrailById(int id);
        Task<ICollection<Trail>> GetTrailsInNationalPark(int parkId);
        Task<bool> Save();
        Task<bool> TrailExists(int id);
        Task<bool> TrailExists(string name);
        Task<bool> UpdateTrail(TrailUpdateDto trailDto);
    }
}