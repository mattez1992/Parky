
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.DTOS;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ParkyAPI.Repos.UserRepos
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserRepo(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<UserReadDto> Login(string username, string password)
        {
            var user = await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            else
            {
                user.Token = CreateToken(user);
                UserReadDto userReadDto = new()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role,
                    Token = user.Token,
                };
                return userReadDto;
            }
        }
        public async Task<UserReadDto> Register(string username, string password)
        {
            try
            {
                if (await UserExists(username))
                {
                    return null;
                }
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                User newUser = new()
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                _dbContext.Users.Add(newUser);
                await _dbContext.SaveChangesAsync();
                UserReadDto userReadDto = new()
                {
                    Id = newUser.Id,
                    Username = newUser.Username,
                    Role = newUser.Role,
                    Token = CreateToken(newUser),
                };
                return userReadDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> UserExists(string userName)
        {
            var userExists = await _dbContext.Users.AnyAsync(x => x.Username.ToLower().Equals(userName.ToLower()));
            return userExists;
        }
        #region Auth Methods
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash =
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        #endregion
    }
}
