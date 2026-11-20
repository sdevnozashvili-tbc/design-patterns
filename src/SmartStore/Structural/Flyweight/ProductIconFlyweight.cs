namespace SmartStore.Structural.Flyweight;

public class ProductIcon
{
    public string IconType { get; }
    public string Color { get; }
    public string Symbol { get; }

    public ProductIcon(string iconType, string color, string symbol)
    {
        IconType = iconType;
        Color = color;
        Symbol = symbol;
        Console.WriteLine($"  Creating new icon object: {iconType} ({color}) {symbol}");
    }

    public void Render(int x, int y, string productName)
    {
        Console.WriteLine($"    {Symbol} [{IconType}/{Color}] at ({x},{y}) for '{productName}'");
    }
}

public class ProductIconFactory
{
    private readonly Dictionary<string, ProductIcon> _icons = new();

    public ProductIcon GetIcon(string iconType, string color, string symbol)
    {
        var key = $"{iconType}_{color}";
        if (!_icons.ContainsKey(key))
        {
            _icons[key] = new ProductIcon(iconType, color, symbol);
        }
        return _icons[key];
    }

    public int GetIconCount() => _icons.Count;
}
