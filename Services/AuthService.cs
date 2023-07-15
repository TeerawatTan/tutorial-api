using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using tutorial_api_2.Data;
using tutorial_api_2.Dtos;
using tutorial_api_2.Models;

namespace tutorial_api_2.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly SampleDbContext _context;

        public AuthService(IConfiguration configuration, SampleDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public UserLoginDto? Login(LoginDto dto)
        {
            User? user = _context.Users.Where(w => w.Username == dto.Username).FirstOrDefault();

            if (user is null)
                return null;

            if (BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                UserLoginDto tokenUser = CreateTokenUser(user.Id, user.Username);

                user.Token = tokenUser.AccessToken;

                _context.SaveChanges();

                return tokenUser;
            }
            else
            {
                return null;
            }
        }

        public SignUpDto? SignUp(SignUpDto dto)
        {
            try
            {
                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                if (!string.IsNullOrEmpty(passwordHash))
                {
                    User user = new();
                    user.Id = Guid.NewGuid();
                    user.Fullname = dto.Fullname;
                    user.Username = dto.Username;
                    user.SaltHash = Convert.ToBase64String(salt);
                    user.Password = passwordHash;
                    user.Email = dto.Email;

                    _context.Users.Add(user);
                    int resultSaved = _context.SaveChanges();
                    if (resultSaved > 0)
                        return dto;
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private UserLoginDto CreateTokenUser(Guid userId, string username)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
                new Claim(ClaimTypes.Name,username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            int expiredDays = int.Parse(_configuration.GetSection("Jwt:ExpireDays").Value);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(expiredDays),
                SigningCredentials = credentials,
                IssuedAt = DateTime.Now,
                Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                Audience = _configuration.GetSection("Jwt:Audience").Value

            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            UserLoginDto userData = new UserLoginDto()
            {
                UserId = userId,
                Username = username,
                AccessToken = tokenHandler.WriteToken(token),
                Issuer = tokenDescriptor.Issuer,
                IssueAt = String.Format("{0:r}", tokenDescriptor.IssuedAt),
                Audience = _configuration.GetSection("Jwt:Audience").Value,
                ExpiresIn = ((DateTimeOffset)tokenDescriptor.Expires).ToUnixTimeMilliseconds(),
                ExpireDate = String.Format("{0:r}", tokenDescriptor.Expires)
            };

            return userData;
        }
    }
}
