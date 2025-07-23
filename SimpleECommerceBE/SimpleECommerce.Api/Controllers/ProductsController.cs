using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleECommerce.Application.Dtos;
using SimpleECommerce.Application.Interfaces;
using SimpleECommerce.Application.IService;
using SimpleECommerce.Domain.Entities;

//namespace SimpleECommerce.Api.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController] 
//    public class ProductsController : ControllerBase
//    {
//        private readonly IProductRepository _repo;
//        public ProductsController(IProductRepository r) => _repo = r;

//        [HttpGet]
//        public async Task<IActionResult> GetAll() =>
//          Ok(await _repo.GetAllAsync());
//    }
//}


namespace SimpleECommerce.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET: /api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products);
        }

        // GET: /api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: /api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _repository.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: /api/products/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest("Product ID mismatch.");

            try
            {
                await _repository.UpdateAsync(product);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: /api/products/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}