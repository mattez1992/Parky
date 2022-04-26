
namespace ParkyWeb.Repository
{
    public interface IUserRepo : IGenericRepository<UserWeb>
    {
        Task<UserWeb> Login(string url, UserLoginDtoWeb loginDto);
        Task<bool> Register(string url, UserLoginDtoWeb loginDto);
    }
}