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

// Registers a new user asynchronously
public async Task<User?> RegisterAsync(RegisterDTO registerDto)
{
    // Check if a user with the same username or email already exists
    var existingUser = (await _userRepository.ListAsync())
        .FirstOrDefault(u => u.Username == registerDto.Username || u.Email == registerDto.Email);
    
    // If the user already exists, return null
    if (existingUser != null) return null;

    // Create a new User object from the registration data
    var user = new User
    {
        Username = registerDto.Username,
        Email = registerDto.Email,
        Role = registerDto.Role ?? "User" // Default to "User" role if not provided
    };

    // Hash the user's password before saving
    user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);
    
    // Add the new user to the repository
    await _userRepository.AddAsync(user);

    // Commit changes to the unit of work (database transaction)
    await _unitOfWork.CompleteAsync();
    
    // Return the created user object
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
