namespace SmartStore.Structural.Decorator;

public interface IShippingService
{
    string GetDescription();
    decimal CalculateCost();
}

public class StandardShipping : IShippingService
{
    public string GetDescription() => "Standard Shipping";
    public decimal CalculateCost() => 5.00m;
}

public abstract class ShippingDecorator : IShippingService
{
    protected readonly IShippingService _inner;
    protected ShippingDecorator(IShippingService inner) => _inner = inner;
    public abstract string GetDescription();
    public abstract decimal CalculateCost();
}

public class GiftWrapDecorator : ShippingDecorator
{
    public GiftWrapDecorator(IShippingService inner) : base(inner) { }
    public override string GetDescription() => _inner.GetDescription() + " + Gift Wrap";
    public override decimal CalculateCost() => _inner.CalculateCost() + 3.50m;
}

public class InsuranceDecorator : ShippingDecorator
{
    public InsuranceDecorator(IShippingService inner) : base(inner) { }
    public override string GetDescription() => _inner.GetDescription() + " + Insurance";
    public override decimal CalculateCost() => _inner.CalculateCost() + 7.00m;
}

public class ExpressDeliveryDecorator : ShippingDecorator
{
    public ExpressDeliveryDecorator(IShippingService inner) : base(inner) { }
    public override string GetDescription() => _inner.GetDescription() + " + Express";
    public override decimal CalculateCost() => _inner.CalculateCost() + 15.00m;
}
