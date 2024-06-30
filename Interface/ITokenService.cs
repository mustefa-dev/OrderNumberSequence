using OrderNumberSequence.DATA.DTOs.User;

namespace OrderNumberSequence.Interface
{
    public interface ITokenService
    {
        string CreateToken(TokenDTO user);
    }
}