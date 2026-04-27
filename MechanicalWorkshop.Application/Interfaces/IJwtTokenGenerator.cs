using MechanicalWorkshop.Domain.Entities;

namespace MechanicalWorkshop.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(AdminUser user);
}