namespace SmartStore.Behavioral.Command;

using SmartStore.Models;

public interface ICartCommand
{
    void Execute();
    void Undo();
    string Description { get; }
}

public class ShoppingCart
{
    private readonly Dictionary<int, (Product Product, int Quantity)> _items = new();

    public void AddItem(Product product, int quantity)
    {
        if (_items.ContainsKey(product.Id))
        {
            var current = _items[product.Id];
            _items[product.Id] = (product, current.Quantity + quantity);
        }
        else
        {
            _items[product.Id] = (product, quantity);
        }
    }

    public void RemoveItem(int productId, int quantity)
    {
        if (_items.ContainsKey(productId))
        {
            var current = _items[productId];
            var newQty = current.Quantity - quantity;
            if (newQty <= 0)
                _items.Remove(productId);
            else
                _items[productId] = (current.Product, newQty);
        }
    }

    public void Display()
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("  Cart is empty");
            return;
        }
        Console.WriteLine("  Shopping Cart:");
        decimal total = 0;
        foreach (var (_, (product, qty)) in _items)
        {
            var lineTotal = product.Price * qty;
            Console.WriteLine($"    - {product.Name} x{qty} = {lineTotal:C}");
            total += lineTotal;
        }
        Console.WriteLine($"    Total: {total:C}");
    }

    public Dictionary<int, (Product Product, int Quantity)> GetItems() => new(_items);
}

public class AddToCartCommand : ICartCommand
{
    private readonly ShoppingCart _cart;
    private readonly Product _product;
    private readonly int _quantity;

    public AddToCartCommand(ShoppingCart cart, Product product, int quantity)
    {
        _cart = cart;
        _product = product;
        _quantity = quantity;
    }

    public string Description => $"Add {_quantity}x {_product.Name}";
    public void Execute() => _cart.AddItem(_product, _quantity);
    public void Undo() => _cart.RemoveItem(_product.Id, _quantity);
}

public class RemoveFromCartCommand : ICartCommand
{
    private readonly ShoppingCart _cart;
    private readonly Product _product;
    private readonly int _quantity;

    public RemoveFromCartCommand(ShoppingCart cart, Product product, int quantity)
    {
        _cart = cart;
        _product = product;
        _quantity = quantity;
    }

    public string Description => $"Remove {_quantity}x {_product.Name}";
    public void Execute() => _cart.RemoveItem(_product.Id, _quantity);
    public void Undo() => _cart.AddItem(_product, _quantity);
}

public class CartCommandInvoker
{
    private readonly Stack<ICartCommand> _history = new();

    public void ExecuteCommand(ICartCommand command)
    {
        command.Execute();
        _history.Push(command);
        Console.WriteLine($"  Executed: {command.Description}");
    }

    public void Undo()
    {
        if (_history.Count == 0)
        {
            Console.WriteLine("  Nothing to undo");
            return;
        }
        var command = _history.Pop();
        command.Undo();
        Console.WriteLine($"  Undone: {command.Description}");
    }

    public int HistoryCount => _history.Count;
}
