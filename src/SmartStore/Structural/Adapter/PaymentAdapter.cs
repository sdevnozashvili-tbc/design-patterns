namespace SmartStore.Structural.Adapter;

// Target interface
public interface IPaymentProcessor
{
    bool ProcessPayment(string orderId, decimal amount, string currency);
    string GetPaymentStatus(string transactionId);
}

// Adaptee 1 - Stripe-like API
public class StripeApi
{
    public string CreateCharge(long amountInCents, string currencyCode, string description)
    {
        Console.WriteLine($"  [Stripe] Creating charge: {amountInCents} cents ({currencyCode}) - {description}");
        return $"stripe_txn_{Guid.NewGuid().ToString()[..8]}";
    }

    public string RetrieveCharge(string chargeId) => "succeeded";
}

// Adaptee 2 - PayPal-like API
public class PayPalApi
{
    public string CreateOrder(double total, string currency)
    {
        Console.WriteLine($"  [PayPal] Creating order: {total} {currency}");
        return $"paypal_order_{Guid.NewGuid().ToString()[..8]}";
    }

    public bool CaptureOrder(string orderId)
    {
        Console.WriteLine($"  [PayPal] Capturing order: {orderId}");
        return true;
    }

    public string GetOrderStatus(string orderId) => "COMPLETED";
}

// Adapter for Stripe
public class StripeAdapter : IPaymentProcessor
{
    private readonly StripeApi _stripe = new();

    public bool ProcessPayment(string orderId, decimal amount, string currency)
    {
        var amountInCents = (long)(amount * 100);
        var txnId = _stripe.CreateCharge(amountInCents, currency, $"Order: {orderId}");
        return !string.IsNullOrEmpty(txnId);
    }

    public string GetPaymentStatus(string transactionId) => _stripe.RetrieveCharge(transactionId);
}

// Adapter for PayPal
public class PayPalAdapter : IPaymentProcessor
{
    private readonly PayPalApi _paypal = new();

    public bool ProcessPayment(string orderId, decimal amount, string currency)
    {
        var ppOrderId = _paypal.CreateOrder((double)amount, currency);
        return _paypal.CaptureOrder(ppOrderId);
    }

    public string GetPaymentStatus(string transactionId) => _paypal.GetOrderStatus(transactionId);
}
