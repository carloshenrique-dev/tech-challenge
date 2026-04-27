using MechanicalWorkshop.Application.Interfaces;
using MechanicalWorkshop.Domain.Interfaces.Repositories;

namespace MechanicalWorkshop.Application.UseCases.Auth;

public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token, string Name, string Email);

public class LoginUseCase
{
    private readonly IAdminUserRepository _adminUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUseCase(
        IAdminUserRepository adminUserRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _adminUserRepository = adminUserRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponse?> ExecuteAsync(LoginRequest request)
    {
        var user = await _adminUserRepository.GetByEmailAsync(request.Email);
        if (user is null) return null;

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash)) return null;

        var token = _jwtTokenGenerator.GenerateToken(user);
        return new LoginResponse(token, user.Name, user.Email);
    }
}