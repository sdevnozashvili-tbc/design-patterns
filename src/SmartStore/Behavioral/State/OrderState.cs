namespace SmartStore.Behavioral.State;

using SmartStore.Models;

public interface IOrderState
{
    string StateName { get; }
    void Process(OrderContext context);
    void Ship(OrderContext context);
    void Deliver(OrderContext context);
    void Cancel(OrderContext context);
}

public class OrderContext
{
    public Order Order { get; }
    public IOrderState CurrentState { get; set; }

    public OrderContext(Order order)
    {
        Order = order;
        CurrentState = new NewOrderState();
    }

    public void Process() => CurrentState.Process(this);
    public void Ship() => CurrentState.Ship(this);
    public void Deliver() => CurrentState.Deliver(this);
    public void Cancel() => CurrentState.Cancel(this);

    public void TransitionTo(IOrderState state)
    {
        Console.WriteLine($"  Order #{Order.Id}: {CurrentState.StateName} -> {state.StateName}");
        CurrentState = state;
        Order.Status = state.StateName;
    }
}

public class NewOrderState : IOrderState
{
    public string StateName => "New";
    public void Process(OrderContext ctx) => ctx.TransitionTo(new ProcessingState());
    public void Ship(OrderContext ctx) => Console.WriteLine("  Cannot ship - order not yet processed");
    public void Deliver(OrderContext ctx) => Console.WriteLine("  Cannot deliver - order not yet shipped");
    public void Cancel(OrderContext ctx) => ctx.TransitionTo(new CancelledState());
}

public class ProcessingState : IOrderState
{
    public string StateName => "Processing";
    public void Process(OrderContext ctx) => Console.WriteLine("  Order is already being processed");
    public void Ship(OrderContext ctx) => ctx.TransitionTo(new ShippedState());
    public void Deliver(OrderContext ctx) => Console.WriteLine("  Cannot deliver - order not yet shipped");
    public void Cancel(OrderContext ctx) => ctx.TransitionTo(new CancelledState());
}

public class ShippedState : IOrderState
{
    public string StateName => "Shipped";
    public void Process(OrderContext ctx) => Console.WriteLine("  Order already shipped");
    public void Ship(OrderContext ctx) => Console.WriteLine("  Order already shipped");
    public void Deliver(OrderContext ctx) => ctx.TransitionTo(new DeliveredState());
    public void Cancel(OrderContext ctx) => Console.WriteLine("  Cannot cancel - order already shipped");
}

public class DeliveredState : IOrderState
{
    public string StateName => "Delivered";
    public void Process(OrderContext ctx) => Console.WriteLine("  Order already delivered");
    public void Ship(OrderContext ctx) => Console.WriteLine("  Order already delivered");
    public void Deliver(OrderContext ctx) => Console.WriteLine("  Order already delivered");
    public void Cancel(OrderContext ctx) => Console.WriteLine("  Cannot cancel - order already delivered");
}

public class CancelledState : IOrderState
{
    public string StateName => "Cancelled";
    public void Process(OrderContext ctx) => Console.WriteLine("  Cannot process - order is cancelled");
    public void Ship(OrderContext ctx) => Console.WriteLine("  Cannot ship - order is cancelled");
    public void Deliver(OrderContext ctx) => Console.WriteLine("  Cannot deliver - order is cancelled");
    public void Cancel(OrderContext ctx) => Console.WriteLine("  Order is already cancelled");
}
