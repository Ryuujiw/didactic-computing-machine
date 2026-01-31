using System.Text.Json.Serialization;

namespace Shopify.Models;

public class ShopifyOrder
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("order_number")]
    public int OrderNumber { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("total_price")]
    public decimal? TotalPrice { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("line_items")]
    public List<ShopifyLineItem> LineItems { get; set; } = [];

    [JsonPropertyName("shipping_address")]
    public ShopifyAddress? ShippingAddress { get; set; }

    [JsonPropertyName("billing_address")]
    public ShopifyAddress? BillingAddress { get; set; }

    [JsonPropertyName("customer")]
    public ShopifyCustomer? Customer { get; set; }
    
    [JsonPropertyName("note_attributes")] 
    public ICollection<NameValuePair> NoteAttributes { get; set; } = [];
    
    [JsonPropertyName("financial_status")]
    public string? FinancialStatus { get; set; }
    
    [JsonPropertyName("total_shipping_price_set")]
    public TotalShippingCost TotalShippingCost { get; set; }
    
    [JsonPropertyName("total_line_items_price")]
    public decimal? TotalLineItemPrice { get; set; }

    [JsonPropertyName("total_discounts")] 
    public decimal? TotalDiscounts { get; set; }
}

public class NameValuePair
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public class ShopifyLineItem
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("price")]
    public string? Price { get; set; }

    [JsonPropertyName("sku")]
    public string? Sku { get; set; }
    
    [JsonPropertyName("properties")]
    public ICollection<NameValuePair> Properties { get; set; }
}

public class ShopifyAddress
{
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
}

public class ShopifyCustomer
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }
}

public class MoneyCommon
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }
}

public class TotalShippingCost
{
    [JsonPropertyName("shop_money")]
    public MoneyCommon ShopMoney { get; set; }
}