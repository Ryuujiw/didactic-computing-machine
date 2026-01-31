using Microsoft.AspNetCore.Mvc;
using Shopify.Models;

namespace Shopify.Controllers;

[ApiController]
[Route("api/webhooks/shopify")]
public class ShopifyWebhookController : ControllerBase
{
    private readonly ILogger<ShopifyWebhookController> _logger;

    public ShopifyWebhookController(ILogger<ShopifyWebhookController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost("order-paid")]
    public IActionResult OrderPaid([FromBody] ShopifyOrder order)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Get data that we need
        var deliveryDate = order.NoteAttributes.FirstOrDefault(x => x.Name == "Delivery-Date")?.Value ?? string.Empty;
        
        
        
        var items = order.LineItems.Select(x => new
        {
            Line1 = $"{x.Title} x {x.Quantity}",
            Line2 = string.Join('\n', x.Properties.Where(y => y.Name.IndexOf("Zapiet") == -1).Select(y => $"{y.Name}: {y.Value}")),
        });
        
        var deliveryNote = order.NoteAttributes.FirstOrDefault(x => x.Name == "Delivery-Note")?.Value ?? string.Empty;

        foreach (var item in items)
        {
            var orderToCreate = new Order(
                order.CreatedAt.ToString(), 
                order.BillingAddress?.Phone ?? string.Empty, 
                deliveryDate,
                order.OrderNumber.ToString(), 
                $"{item.Line1}\n{item.Line2}",
                deliveryNote, 
                order.ShippingAddress?.FirstName ?? string.Empty,
                order.ShippingAddress?.Phone ?? string.Empty,
                $"{order.ShippingAddress?.Address1}\n{order.ShippingAddress?.City}\n{order.ShippingAddress?.Zip}",
                order.FinancialStatus ?? string.Empty,
                order.TotalShippingCost.ShopMoney.Amount,
                order.TotalLineItemPrice.GetValueOrDefault(),
                order.TotalDiscounts.GetValueOrDefault(),
                order.TotalPrice.GetValueOrDefault());
            
            _logger.LogInformation("Order to send to sheets {@order}", orderToCreate);
        }

        
        // Send to google sheets

        return Ok();
    }
}
