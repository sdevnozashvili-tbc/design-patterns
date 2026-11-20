namespace SmartStore.Behavioral.Memento;

// --- Memento ---

public class CartMemento
{
    public Dictionary<int, (string Name, decimal Price, int Quantity)> Items { get; }
    public DateTime SavedAt { get; }
    public string Description { get; }

    public CartMemento(Dictionary<int, (string Name, decimal Price, int Quantity)> items, string description)
    {
        Items = new Dictionary<int, (string, decimal, int)>(items);
        SavedAt = DateTime.Now;
        Description = description;
    }
}

// --- Originator ---

public class MementoCart
{
    private readonly Dictionary<int, (string Name, decimal Price, int Quantity)> _items = new();

    public void AddItem(int productId, string name, decimal price, int quantity)
    {
        if (_items.ContainsKey(productId))
        {
            var current = _items[productId];
            _items[productId] = (name, price, current.Quantity + quantity);
        }
        else
        {
            _items[productId] = (name, price, quantity);
        }
    }

    public void RemoveItem(int productId)
    {
        _items.Remove(productId);
    }

    public void Clear() => _items.Clear();

    public CartMemento Save(string description)
    {
        return new CartMemento(new Dictionary<int, (string, decimal, int)>(_items), description);
    }

    public void Restore(CartMemento memento)
    {
        _items.Clear();
        foreach (var (key, value) in memento.Items)
        {
            _items[key] = value;
        }
    }

    public void Display()
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("  Cart is empty");
            return;
        }
        Console.WriteLine("  Cart Contents:");
        decimal total = 0;
        foreach (var (id, (name, price, qty)) in _items)
        {
            var lineTotal = price * qty;
            Console.WriteLine($"    [{id}] {name} x{qty} = {lineTotal:C}");
            total += lineTotal;
        }
        Console.WriteLine($"    Total: {total:C}");
    }
}

// --- Caretaker ---

public class CartHistory
{
    private readonly Stack<CartMemento> _mementos = new();

    public void Save(CartMemento memento)
    {
        _mementos.Push(memento);
        Console.WriteLine($"  Saved: {memento.Description} (at {memento.SavedAt:HH:mm:ss})");
    }

    public CartMemento? Undo()
    {
        if (_mementos.Count == 0) return null;
        var memento = _mementos.Pop();
        Console.WriteLine($"  Restoring: {memento.Description}");
        return memento;
    }

    public int Count => _mementos.Count;

    public void ShowHistory()
    {
        Console.WriteLine($"  Cart History ({_mementos.Count} saves):");
        foreach (var m in _mementos.Reverse())
        {
            Console.WriteLine($"    - {m.Description} (at {m.SavedAt:HH:mm:ss})");
        }
    }
}
