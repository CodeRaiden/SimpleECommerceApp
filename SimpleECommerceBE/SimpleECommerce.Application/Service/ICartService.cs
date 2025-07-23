using SimpleECommerce.Application.Dtos;

namespace SimpleECommerce.Application.IService
{
    public interface ICartService
    {
        
        Task<List<CartItemDto>> GetCartAsync(string userId);
        Task AddOrUpdateAsync(string userId, AddCartItemDto item);
        Task RemoveAsync(string userId, Guid productId);
    }
}
