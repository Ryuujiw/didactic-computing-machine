using Microsoft.AspNetCore.Mvc;
using Shopify.Models;

namespace Shopify.Controllers;

[ApiController]
[Route("api/webhooks/shopify")]
public class ShopifyWebhookController : ControllerBase
{
    [HttpPost("order-paid")]
    public IActionResult OrderPaid([FromBody] ShopifyOrder order)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Example usage
        Console.WriteLine($"Order {order.Id} from {order.Email}");
        Console.WriteLine($"Items: {order.LineItems.Count}");

        return Ok();
    }
}
