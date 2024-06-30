using OrderNumberSequence.Entities;

namespace OrderNumberSequence.DATA.DTOs.User;

public class TokenDTO
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public UserRole? Role { get; set; }
}