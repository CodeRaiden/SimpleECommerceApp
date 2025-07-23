using SimpleECommerce.Application.Dtos;
using SimpleECommerce.Application.Interfaces;
using SimpleECommerce.Application.IService;
using SimpleECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleECommerce.Infrastructure.Service
{
    // ECommerce.Application/CartService.cs
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        public CartService(ICartRepository cartRepo) => _cartRepo = cartRepo;

        public async Task<List<CartItemDto>> GetCartAsync(string userId)
        {
            return await _cartRepo.GetWithProductAsync(userId);
        }

        public Task AddOrUpdateAsync(string userId, AddCartItemDto item) =>
            _cartRepo.AddOrUpdateAsync(userId, new CartItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });

        public Task RemoveAsync(string userId, Guid productId) =>
            _cartRepo.RemoveAsync(userId, productId);
    }
}
