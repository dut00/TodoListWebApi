using System.Security.Claims;
using TodoListWebApi.DTOs;
using TodoListWebApi.Entities;

namespace TodoListWebApi.Services;

public interface IUserService
{
    ClaimsPrincipal GetMyClaims();
    User? GetUser(string username);
    User? GetUser(uint userId);
    bool CreateUser(UserDTO request, out User? user);

    string CreateToken(User user);
    bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash);
}