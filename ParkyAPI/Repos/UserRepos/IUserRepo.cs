
using ParkyAPI.DTOS;

namespace ParkyAPI.Repos.UserRepos
{
    public interface IUserRepo
    {
        Task<UserReadDto> Login(string username, string password);
        Task<UserReadDto> Register(string username, string password);
        Task<bool> UserExists(string userName);
    }
}