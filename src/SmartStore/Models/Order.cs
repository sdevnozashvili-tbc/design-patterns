namespace SmartStore.Models;

public class Order
{
    public int Id { get; set; }
    public Customer? Customer { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public string ShippingAddress { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Status { get; set; } = "New";
    public string? Notes { get; set; }

    public override string ToString() =>
        $"Order #{Id} | Customer: {Customer?.Name} | Items: {Items.Count} | Total: {TotalAmount:C} | Status: {Status}";
}

public class OrderItem
{
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
}
