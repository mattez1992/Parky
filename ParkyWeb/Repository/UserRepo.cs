using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkyWeb.Repository
{
    public class UserRepo : GenericRepository<UserWeb>, IUserRepo
    {
        public UserRepo(HttpClient client) : base(client)
        {

        }

        public async Task<UserWeb> Login(string url, UserLoginDtoWeb loginDto)
        {
            var response = await _client.PostAsJsonAsync(url, loginDto);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<UserWeb>();

            else
                return new UserWeb();
        }
        public async Task<bool> Register(string url, UserLoginDtoWeb loginDto)
        {
            if (loginDto == null)
            {
                return false;
            }
            var response = await _client.PostAsJsonAsync(url, loginDto);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
