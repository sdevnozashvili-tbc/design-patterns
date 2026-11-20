namespace SmartStore.Behavioral.ChainOfResponsibility;

using SmartStore.Models;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public string HandlerName { get; set; } = string.Empty;

    public static ValidationResult Success() => new() { IsValid = true, Message = "Valid" };
    public static ValidationResult Failure(string handler, string message) =>
        new() { IsValid = false, HandlerName = handler, Message = message };
}

public abstract class OrderValidationHandler
{
    private OrderValidationHandler? _next;

    public OrderValidationHandler SetNext(OrderValidationHandler next)
    {
        _next = next;
        return next;
    }

    public virtual ValidationResult Handle(Order order)
    {
        var result = Validate(order);
        if (!result.IsValid)
            return result;

        if (_next != null)
            return _next.Handle(order);

        return ValidationResult.Success();
    }

    protected abstract ValidationResult Validate(Order order);
}

public class StockValidationHandler : OrderValidationHandler
{
    protected override ValidationResult Validate(Order order)
    {
        Console.WriteLine("  StockValidation: Checking product availability...");
        foreach (var item in order.Items)
        {
            if (item.Product.StockQuantity < item.Quantity)
            {
                return ValidationResult.Failure("StockValidation",
                    $"Insufficient stock for {item.Product.Name}. Available: {item.Product.StockQuantity}, Requested: {item.Quantity}");
            }
        }
        Console.WriteLine("  StockValidation: All items in stock");
        return ValidationResult.Success();
    }
}

public class PaymentValidationHandler : OrderValidationHandler
{
    protected override ValidationResult Validate(Order order)
    {
        Console.WriteLine("  PaymentValidation: Verifying payment method...");
        if (string.IsNullOrEmpty(order.PaymentMethod))
        {
            return ValidationResult.Failure("PaymentValidation", "No payment method specified");
        }
        Console.WriteLine($"  PaymentValidation: Payment method '{order.PaymentMethod}' is valid");
        return ValidationResult.Success();
    }
}

public class FraudCheckHandler : OrderValidationHandler
{
    protected override ValidationResult Validate(Order order)
    {
        Console.WriteLine("  FraudCheck: Analyzing order for suspicious activity...");
        if (order.TotalAmount > 10000)
        {
            return ValidationResult.Failure("FraudCheck",
                $"Order amount {order.TotalAmount:C} exceeds fraud threshold. Manual review required.");
        }
        Console.WriteLine("  FraudCheck: No suspicious activity detected");
        return ValidationResult.Success();
    }
}

public class AddressValidationHandler : OrderValidationHandler
{
    protected override ValidationResult Validate(Order order)
    {
        Console.WriteLine("  AddressValidation: Verifying shipping address...");
        if (string.IsNullOrWhiteSpace(order.ShippingAddress))
        {
            return ValidationResult.Failure("AddressValidation", "Shipping address is required");
        }
        Console.WriteLine($"  AddressValidation: Address '{order.ShippingAddress}' is valid");
        return ValidationResult.Success();
    }
}
