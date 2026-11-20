namespace SmartStore.Creational.Builder;

using SmartStore.Models;

public interface IOrderBuilder
{
    IOrderBuilder SetCustomer(Customer customer);
    IOrderBuilder AddItem(Product product, int quantity);
    IOrderBuilder SetShippingAddress(string address);
    IOrderBuilder SetPaymentMethod(string method);
    IOrderBuilder AddNotes(string notes);
    Order Build();
}

public class OrderBuilder : IOrderBuilder
{
    private readonly Order _order = new();
    private static int _nextId = 1;

    public IOrderBuilder SetCustomer(Customer customer)
    {
        _order.Customer = customer;
        return this;
    }

    public IOrderBuilder AddItem(Product product, int quantity)
    {
        _order.Items.Add(new OrderItem
        {
            Product = product,
            Quantity = quantity,
            UnitPrice = product.Price
        });
        return this;
    }

    public IOrderBuilder SetShippingAddress(string address)
    {
        _order.ShippingAddress = address;
        return this;
    }

    public IOrderBuilder SetPaymentMethod(string method)
    {
        _order.PaymentMethod = method;
        return this;
    }

    public IOrderBuilder AddNotes(string notes)
    {
        _order.Notes = notes;
        return this;
    }

    public Order Build()
    {
        _order.Id = _nextId++;
        _order.TotalAmount = _order.Items.Sum(i => i.TotalPrice);
        _order.CreatedAt = DateTime.Now;
        return _order;
    }
}

public class OrderDirector
{
    public Order CreateStandardOrder(IOrderBuilder builder, Customer customer, Product product)
    {
        return builder
            .SetCustomer(customer)
            .AddItem(product, 1)
            .SetShippingAddress(customer.Address)
            .SetPaymentMethod("Credit Card")
            .Build();
    }

    public Order CreateBulkOrder(IOrderBuilder builder, Customer customer, List<(Product Product, int Qty)> items)
    {
        builder.SetCustomer(customer)
            .SetShippingAddress(customer.Address)
            .SetPaymentMethod("Bank Transfer")
            .AddNotes("Bulk order - priority processing");

        foreach (var (product, qty) in items)
        {
            builder.AddItem(product, qty);
        }

        return builder.Build();
    }
}
