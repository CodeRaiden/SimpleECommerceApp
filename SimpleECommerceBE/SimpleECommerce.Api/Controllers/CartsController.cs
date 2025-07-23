using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleECommerce.Application.Dtos;
using SimpleECommerce.Application.IService;

namespace SimpleECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _svc;
        public CartController(ICartService svc) => _svc = svc;

        string GetUserId() => User.Identity?.Name ?? "demo-user";

        [HttpGet]
        public async Task<IActionResult> Get() =>
          Ok(await _svc.GetCartAsync(GetUserId()));

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate([FromBody] AddCartItemDto item)
        {
            await _svc.AddOrUpdateAsync(GetUserId(), item);
            return NoContent();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(Guid productId)
        {
            await _svc.RemoveAsync(GetUserId(), productId);
            return NoContent();
        }
    }
}
