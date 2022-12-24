using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using RestaurantAPI.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantAPI.Services;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;
    private readonly RestaurantDbContext _dbContext;

    public AccountService(IMapper mapper, 
                          IPasswordHasher<User> passwordHasher, 
                          AuthenticationSettings authenticationSettings, 
                          RestaurantDbContext dbContext)
    {
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _dbContext = dbContext;
    }

    public async Task RegisterUserAsync(RegisterUserDTO registerUserDto)
    {
        var user = _mapper.Map<User>(registerUserDto);

        user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDto.Password);

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<string> LoginAsync(LoginDTO loginDto)
    {
        var user = await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email)
            ?? throw new BadRequestException("Invalid username or password...");

        var password = _passwordHasher
            .VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

        if (password == PasswordVerificationResult.Failed)
            throw new BadRequestException("Invalid username or password...");

        return GenerateJwtForUser(user);
    }

    private string GenerateJwtForUser(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{ user.FirstName } { user.LastName }"),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("DateOfBirth", user.DateOfBirth is null 
                ? string.Empty 
                : user.DateOfBirth.Value.ToString("yyyy-MM-dd"))
        };

        if (!string.IsNullOrEmpty(user.Nationality))
            claims.Add(new Claim("Nationality", user.Nationality));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(
            _authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}
