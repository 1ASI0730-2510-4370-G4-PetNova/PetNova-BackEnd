namespace PetNova.API.Shared.Application.Services;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;

public interface ITokenService
{
    string GenerateToken(User user);
}