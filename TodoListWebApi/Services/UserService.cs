using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoListWebApi.Data;
using TodoListWebApi.DTOs;
using TodoListWebApi.Entities;

namespace TodoListWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public UserService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _context = context;
            _context.Database.EnsureCreated();
        }

        public ClaimsPrincipal GetMyClaims()
        {
            ClaimsPrincipal result = null;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User;
            }
            return result;
        }

        public User? GetUser(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username);
        }

        public User? GetUser(uint userId)
        {
            return _context.Users.SingleOrDefault(u => u.Id == userId);
        }

        public bool CreateUser(UserDTO request, out User? user)
        {
            user = null;
            if (_context.Users.Any(u => u.Username == request.Username))
                return false;

            CreatePasswordHash(request.Password, out byte[] passwordSalt, out byte[] passwordHash);

            user = new User()
            {
                Username = request.Username,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            //string rawPasswordSalt = string.Join(",", user.PasswordSalt);
            //string rawPasswordHash = string.Join(",", user.PasswordHash);

            return true;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretAppKey").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            passwordSalt = RandomNumberGenerator.GetBytes(8);

            using (var hmac = new HMACSHA256(passwordSalt))
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
