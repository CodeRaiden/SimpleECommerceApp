namespace SimpleECommerce.Application.Dtos
{
    public record CartItemDto
    (
        Guid ProductId, string ProductName, decimal Price, int Quantity
    );
}
