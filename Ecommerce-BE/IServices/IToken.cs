using Domain.Models;

namespace Ecommerce_BE.IServices
{
    public interface IToken
    {
        Task<string> CreateToken(User user);
    }
}
