namespace SmartStore.Structural.Bridge;

// --- Implementor ---

public interface IMessageSender
{
    void Send(string recipient, string subject, string body);
}

public class EmailSender : IMessageSender
{
    public void Send(string recipient, string subject, string body)
    {
        Console.WriteLine($"  Email to {recipient}: [{subject}] {body}");
    }
}

public class SmsSender : IMessageSender
{
    public void Send(string recipient, string subject, string body)
    {
        Console.WriteLine($"  SMS to {recipient}: {body}");
    }
}

public class PushNotificationSender : IMessageSender
{
    public void Send(string recipient, string subject, string body)
    {
        Console.WriteLine($"  Push to {recipient}: {subject} - {body}");
    }
}

// --- Abstraction ---

public abstract class Notification
{
    protected IMessageSender Sender;

    protected Notification(IMessageSender sender) => Sender = sender;

    public void SetSender(IMessageSender sender) => Sender = sender;
    public abstract void Notify(string recipient);
}

public class OrderNotification : Notification
{
    private readonly int _orderId;
    private readonly string _status;

    public OrderNotification(IMessageSender sender, int orderId, string status) : base(sender)
    {
        _orderId = orderId;
        _status = status;
    }

    public override void Notify(string recipient)
    {
        Sender.Send(recipient, $"Order #{_orderId} Update",
            $"Your order #{_orderId} status: {_status}");
    }
}

public class PromotionNotification : Notification
{
    private readonly string _promoName;
    private readonly int _discountPercent;

    public PromotionNotification(IMessageSender sender, string promoName, int discountPercent) : base(sender)
    {
        _promoName = promoName;
        _discountPercent = discountPercent;
    }

    public override void Notify(string recipient)
    {
        Sender.Send(recipient, $"{_promoName}",
            $"Get {_discountPercent}% off with our {_promoName} promotion!");
    }
}
