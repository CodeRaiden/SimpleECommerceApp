//using Microsoft.EntityFrameworkCore;
//using SimpleECommerce.Application.Interfaces;
//using SimpleECommerce.Domain.Entities;
//using SimpleECommerce.Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SimpleECommerce.Infrastructure.Repositories
//{
//    public class ProductRepository : IProductRepository
//    {
//        private readonly SimpleECommerceDbContext _ctx;
//        public ProductRepository(SimpleECommerceDbContext ctx) => _ctx = ctx;
//        public Task<List<Product>> GetAllAsync() => _ctx.Products.ToListAsync();
//    }


//}


using Microsoft.EntityFrameworkCore;
using SimpleECommerce.Application.Interfaces;
using SimpleECommerce.Domain.Entities;
using SimpleECommerce.Infrastructure.Data;

namespace SimpleECommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SimpleECommerceDbContext _context;

        public ProductRepository(SimpleECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
