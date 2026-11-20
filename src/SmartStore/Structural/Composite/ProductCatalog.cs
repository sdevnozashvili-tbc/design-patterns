namespace SmartStore.Structural.Composite;

public interface ICatalogComponent
{
    string Name { get; }
    void Display(int indent = 0);
    decimal GetTotalPrice();
    int GetItemCount();
}

public class ProductLeaf : ICatalogComponent
{
    public string Name { get; }
    public decimal Price { get; }

    public ProductLeaf(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}  Product: {Name} - {Price:C}");
    }

    public decimal GetTotalPrice() => Price;
    public int GetItemCount() => 1;
}

public class CategoryComposite : ICatalogComponent
{
    public string Name { get; }
    private readonly List<ICatalogComponent> _children = new();

    public CategoryComposite(string name) => Name = name;

    public void Add(ICatalogComponent component) => _children.Add(component);
    public void Remove(ICatalogComponent component) => _children.Remove(component);

    public void Display(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}  [{Name}] ({GetItemCount()} items, Total: {GetTotalPrice():C})");
        foreach (var child in _children)
        {
            child.Display(indent + 3);
        }
    }

    public decimal GetTotalPrice() => _children.Sum(c => c.GetTotalPrice());
    public int GetItemCount() => _children.Sum(c => c.GetItemCount());
}
