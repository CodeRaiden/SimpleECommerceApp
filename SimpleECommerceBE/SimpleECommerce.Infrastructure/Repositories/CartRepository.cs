using Microsoft.EntityFrameworkCore;
using SimpleECommerce.Application.Dtos;
using SimpleECommerce.Application.Interfaces;
using SimpleECommerce.Domain.Entities;
using SimpleECommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleECommerce.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly SimpleECommerceDbContext _ctx;
        public CartRepository(SimpleECommerceDbContext ctx) => _ctx = ctx;
        public async Task<List<CartItem>> GetAsync(string userId)
        {
            return await _ctx.CartItems
              .Where(ci => ci.UserId == userId)
              .Select(ci => new CartItem { ProductId = ci.ProductId, Quantity = ci.Quantity })
              .ToListAsync();
        }

        public async Task<List<CartItemDto>> GetWithProductAsync(string userId)
        {
            return await _ctx.CartItems
                .Where(ci => ci.UserId == userId)
                .Include(ci => ci.Product)
                .Select(ci => new CartItemDto(
                    ci.ProductId,
                    ci.Product.Name,
                    ci.Product.Price,
                    ci.Quantity
                ))
                .ToListAsync();
        }

        public async Task AddOrUpdateAsync(string userId, CartItem item)
        {
            var ent = await _ctx.CartItems.FindAsync(userId, item.ProductId);
            if (ent is null)
            {
                _ctx.CartItems.Add(new CartItemEntity { UserId = userId, ProductId = item.ProductId, Quantity = item.Quantity });
            }
            else
            {
                ent.Quantity = item.Quantity;
            }
            await _ctx.SaveChangesAsync();
        }
        public async Task RemoveAsync(string userId, Guid productId)
        {
            var ent = await _ctx.CartItems.FindAsync(userId, productId);
            if (ent != null)
            {
                _ctx.CartItems.Remove(ent);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
