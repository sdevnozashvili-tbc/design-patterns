namespace SmartStore.Models;

public class Product : ICloneable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public double Weight { get; set; }

    public object Clone() => MemberwiseClone();

    public Product DeepClone() => new()
    {
        Id = Id,
        Name = Name,
        Category = Category,
        Price = Price,
        StockQuantity = StockQuantity,
        ImagePath = ImagePath,
        Weight = Weight
    };

    public override string ToString() => $"[{Id}] {Name} - {Price:C} (Stock: {StockQuantity})";
}
