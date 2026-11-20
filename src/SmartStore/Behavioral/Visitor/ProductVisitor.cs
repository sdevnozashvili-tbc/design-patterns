namespace SmartStore.Behavioral.Visitor;

// --- Visitable Product Types ---

public interface IVisitableProduct
{
    string Name { get; }
    decimal Price { get; }
    void Accept(IProductVisitor visitor);
}

public class Electronics : IVisitableProduct
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public double WarrantyYears { get; set; }
    public void Accept(IProductVisitor visitor) => visitor.Visit(this);
}

public class Clothing : IVisitableProduct
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Size { get; set; } = string.Empty;
    public void Accept(IProductVisitor visitor) => visitor.Visit(this);
}

public class Food : IVisitableProduct
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime ExpiryDate { get; set; }
    public void Accept(IProductVisitor visitor) => visitor.Visit(this);
}

// --- Visitor Interface ---

public interface IProductVisitor
{
    void Visit(Electronics electronics);
    void Visit(Clothing clothing);
    void Visit(Food food);
    string GetResult();
}

// --- Concrete Visitors ---

public class TaxCalculatorVisitor : IProductVisitor
{
    private decimal _totalTax;

    public void Visit(Electronics e)
    {
        var tax = e.Price * 0.20m;
        _totalTax += tax;
        Console.WriteLine($"  Electronics '{e.Name}': Tax 20% = {tax:C}");
    }

    public void Visit(Clothing c)
    {
        var tax = c.Price * 0.12m;
        _totalTax += tax;
        Console.WriteLine($"  Clothing '{c.Name}': Tax 12% = {tax:C}");
    }

    public void Visit(Food f)
    {
        var tax = f.Price * 0.05m;
        _totalTax += tax;
        Console.WriteLine($"  Food '{f.Name}': Tax 5% = {tax:C}");
    }

    public string GetResult() => $"Total Tax: {_totalTax:C}";
}

public class DiscountVisitor : IProductVisitor
{
    private decimal _totalDiscount;

    public void Visit(Electronics e)
    {
        if (e.Price > 500)
        {
            var discount = e.Price * 0.10m;
            _totalDiscount += discount;
            Console.WriteLine($"  Electronics '{e.Name}': Premium discount 10% = -{discount:C}");
        }
        else
        {
            Console.WriteLine($"  Electronics '{e.Name}': No discount (price <= $500)");
        }
    }

    public void Visit(Clothing c)
    {
        var discount = c.Price * 0.15m;
        _totalDiscount += discount;
        Console.WriteLine($"  Clothing '{c.Name}': Season sale 15% = -{discount:C}");
    }

    public void Visit(Food f)
    {
        if (f.ExpiryDate < DateTime.Now.AddDays(7))
        {
            var discount = f.Price * 0.30m;
            _totalDiscount += discount;
            Console.WriteLine($"  Food '{f.Name}': Near expiry 30% = -{discount:C}");
        }
        else
        {
            Console.WriteLine($"  Food '{f.Name}': No discount (expiry > 7 days)");
        }
    }

    public string GetResult() => $"Total Discount: -{_totalDiscount:C}";
}

public class ShippingCostVisitor : IProductVisitor
{
    private decimal _totalShipping;

    public void Visit(Electronics e)
    {
        var cost = 8.99m;
        _totalShipping += cost;
        Console.WriteLine($"  Electronics '{e.Name}': Fragile shipping = {cost:C}");
    }

    public void Visit(Clothing c)
    {
        var cost = 3.99m;
        _totalShipping += cost;
        Console.WriteLine($"  Clothing '{c.Name}': Standard shipping = {cost:C}");
    }

    public void Visit(Food f)
    {
        var cost = 12.99m;
        _totalShipping += cost;
        Console.WriteLine($"  Food '{f.Name}': Refrigerated shipping = {cost:C}");
    }

    public string GetResult() => $"Total Shipping: {_totalShipping:C}";
}
