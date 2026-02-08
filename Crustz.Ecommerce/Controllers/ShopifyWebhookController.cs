using System.Globalization;
using Crustz.Ecommerce.Helpers;
using Crustz.Ecommerce.Models;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Mvc;

namespace Crustz.Ecommerce.Controllers;

[Route("api/webhooks/shopify")]
public class ShopifyWebhookController : ControllerBase
{
    private readonly ILogger<ShopifyWebhookController> _logger;
    private readonly SheetsService _sheets;
    private const string SpreadsheetId = "1brdeEFztd0ymjLER-lV2BZQD4TnWjk0OQowz1HLINDs";

    public ShopifyWebhookController(ILogger<ShopifyWebhookController> logger, SheetsService sheets)
    {
        _logger = logger;
        _sheets = sheets;
    }
    
    // POST api/webhooks/shopify
    [HttpPost("order-paid")]
    public async Task Post([FromBody] ShopifyOrder order)
    {
        _logger.LogInformation("Order paid webhook received. Order Id {orderId}, order: {@order}", order.Id, order);
        
        var ordersToCreate = new List<Order>();

        try
        {
            // Get data that we need
            var deliveryDate = order.NoteAttributes.FirstOrDefault(x => x.Name == "Delivery-Date")?.Value ?? string.Empty;

            var items = order.LineItems.Select(x => new
            {
                Line1 = $"{x.Title} x {x.Quantity}",
                Line2 = string.Join('\n', x.Properties.Where(y => y.Name.IndexOf("Zapiet") == -1).Select(y => $"{y.Name}: {y.Value}")),
            });
            
            var deliveryNote = order.NoteAttributes.FirstOrDefault(x => x.Name == "Delivery-Note")?.Value ?? string.Empty;
            DateTime.TryParseExact(deliveryDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime deliveryDateParsed);
            
            var deliveryDateSanitized = deliveryDateParsed.ToString("dd MMMM");
            var zone = ZoneHelper.ToZone(order.ShippingLines?.FirstOrDefault()?.Code ?? string.Empty);

            var billingContact = (order.BillingAddress?.Phone ?? order.ContactEmail) ?? string.Empty;
             
            foreach (var item in items)
            {
                var orderToCreate = new Order(
                    order.CreatedAt.ToString("yyyy-MM-ddT:hh:mm.sssZ"), 
                    billingContact,
                    $"{zone}\n{deliveryDateSanitized}",
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
                
                ordersToCreate.Add(orderToCreate);
            }
            
            var rows = ordersToCreate.Select(x => GoogleSheetsHelper.ToRow(x)).ToList();
            var body = new ValueRange
            {
                Values = rows
            };
            
            _logger.LogInformation("Writing order {orderId} to google sheets, number of rows: {count}", order.Id, rows.Count);

            var request = _sheets.Spreadsheets.Values.Append(body, SpreadsheetId, "Sheet1!A1");
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            await request.ExecuteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured when processing order {orderId}", order.Id);
            throw;
        }
    }
}