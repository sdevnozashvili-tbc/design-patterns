namespace SmartStore.Structural.Facade;

using SmartStore.Models;

// --- Complex Subsystems ---

public class InventoryService
{
    public bool CheckStock(Product product, int quantity)
    {
        Console.WriteLine($"  Inventory: Checking stock for {product.Name}...");
        return product.StockQuantity >= quantity;
    }

    public void ReserveStock(Product product, int quantity)
    {
        product.StockQuantity -= quantity;
        Console.WriteLine($"  Inventory: Reserved {quantity}x {product.Name} (Remaining: {product.StockQuantity})");
    }
}

public class PaymentService
{
    public bool ChargeCustomer(Customer customer, decimal amount)
    {
        Console.WriteLine($"  Payment: Charging {amount:C} to {customer.Name}...");
        return true;
    }
}

public class ShippingService
{
    public string CreateShipment(Order order)
    {
        var trackingNumber = $"TRACK-{order.Id:D6}";
        Console.WriteLine($"  Shipping: Created shipment {trackingNumber} to {order.ShippingAddress}");
        return trackingNumber;
    }
}

public class NotificationService
{
    public void SendOrderConfirmation(Customer customer, Order order)
    {
        Console.WriteLine($"  Email: Sent order confirmation to {customer.Email} for Order #{order.Id}");
    }
}

// --- Facade ---

public class OrderProcessingFacade
{
    private readonly InventoryService _inventory = new();
    private readonly PaymentService _payment = new();
    private readonly ShippingService _shipping = new();
    private readonly NotificationService _notification = new();

    public bool ProcessOrder(Order order)
    {
        Console.WriteLine($"\n  Processing Order #{order.Id}...");

        foreach (var item in order.Items)
        {
            if (!_inventory.CheckStock(item.Product, item.Quantity))
            {
                Console.WriteLine($"  FAILED: Insufficient stock for {item.Product.Name}");
                return false;
            }
        }

        if (!_payment.ChargeCustomer(order.Customer!, order.TotalAmount))
        {
            Console.WriteLine("  FAILED: Payment failed");
            return false;
        }

        foreach (var item in order.Items)
        {
            _inventory.ReserveStock(item.Product, item.Quantity);
        }

        var tracking = _shipping.CreateShipment(order);
        _notification.SendOrderConfirmation(order.Customer!, order);

        Console.WriteLine($"  SUCCESS: Order #{order.Id} processed! Tracking: {tracking}");
        return true;
    }
}
