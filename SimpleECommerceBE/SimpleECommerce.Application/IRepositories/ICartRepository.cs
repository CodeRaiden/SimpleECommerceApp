using SimpleECommerce.Application.Dtos;
using SimpleECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleECommerce.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<List<CartItemDto>> GetWithProductAsync(string userId);
        Task<List<CartItem>> GetAsync(string userId);
        Task AddOrUpdateAsync(string userId, CartItem item);
        Task RemoveAsync(string userId, Guid productId);
    }
}
