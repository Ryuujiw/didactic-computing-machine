namespace Crustz.Ecommerce.Models;

public record Order(
    string OrderNumber,
    string BillingContact,
    string DeliveryDate,
    string OrderRef,
    string Items,
    string Remarks,
    string RecipientName,
    string RecipientContact,
    string Address,
    string Payment,
    decimal DeliveryFee,
    decimal OrderTotal,
    decimal Discount,
    decimal GrandTotal);
    