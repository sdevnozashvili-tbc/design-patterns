namespace SmartStore.Behavioral.Observer;

using SmartStore.Models;

public interface IOrderObserver
{
    void OnOrderStatusChanged(Order order, string oldStatus, string newStatus);
}

public class ObservableOrder
{
    private readonly Order _order;
    private readonly List<IOrderObserver> _observers = new();

    public ObservableOrder(Order order) => _order = order;
    public Order Order => _order;

    public void Attach(IOrderObserver observer) => _observers.Add(observer);
    public void Detach(IOrderObserver observer) => _observers.Remove(observer);

    public void ChangeStatus(string newStatus)
    {
        var oldStatus = _order.Status;
        _order.Status = newStatus;
        Console.WriteLine($"\n  Order #{_order.Id} status: {oldStatus} -> {newStatus}");
        foreach (var observer in _observers)
        {
            observer.OnOrderStatusChanged(_order, oldStatus, newStatus);
        }
    }
}

public class CustomerNotifier : IOrderObserver
{
    public void OnOrderStatusChanged(Order order, string oldStatus, string newStatus)
    {
        Console.WriteLine($"  CustomerNotifier: Notifying {order.Customer?.Name} about status change to '{newStatus}'");
    }
}

public class InventoryObserver : IOrderObserver
{
    public void OnOrderStatusChanged(Order order, string oldStatus, string newStatus)
    {
        if (newStatus == "Cancelled")
            Console.WriteLine($"  InventoryObserver: Restoring stock for cancelled Order #{order.Id}");
        else if (newStatus == "Shipped")
            Console.WriteLine($"  InventoryObserver: Confirming stock deduction for Order #{order.Id}");
    }
}

public class AnalyticsTracker : IOrderObserver
{
    public void OnOrderStatusChanged(Order order, string oldStatus, string newStatus)
    {
        Console.WriteLine($"  AnalyticsTracker: Recording Order #{order.Id} transition: {oldStatus} -> {newStatus}");
    }
}
