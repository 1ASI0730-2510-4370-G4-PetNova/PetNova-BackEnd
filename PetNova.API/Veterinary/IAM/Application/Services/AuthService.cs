using Microsoft.AspNetCore.Identity;
using PetNova.API.Shared.Domain.Repository;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;
using PetNova.API.Veterinary.IAM.Interface.DTOs;

namespace PetNova.API.Veterinary.IAM.Application.Services;

public class AuthService
{
    private readonly IRepository<User, Guid> _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        IRepository<User, Guid> userRepository,
        IPasswordHasher<User> passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> RegisterAsync(RegisterDTO registerDto)
    {
        var existingUser = (await _userRepository.ListAsync())
            .FirstOrDefault(u => u.Username == registerDto.Username || u.Email == registerDto.Email);
        
        if (existingUser != null) return null;

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            Role = registerDto.Role ?? "User"
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);
        
        await _userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();
        
        return user;
    }

    public async Task<User?> AuthenticateAsync(LoginDTO loginDto)
    {
        var user = (await _userRepository.ListAsync())
            .FirstOrDefault(u => u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail);
        
        if (user == null) return null;

        var result = _passwordHasher.VerifyHashedPassword(
            user, user.PasswordHash, loginDto.Password);
        
        return result == PasswordVerificationResult.Success ? user : null;
    }

    public async Task<IEnumerable<User>> ListUsersAsync()
    {
        return await _userRepository.ListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.FindByIdAsync(id);
    }
}