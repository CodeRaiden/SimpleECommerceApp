using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleECommerce.Application.Dtos
{
    public record AddCartItemDto
    (Guid ProductId, int Quantity);
}
