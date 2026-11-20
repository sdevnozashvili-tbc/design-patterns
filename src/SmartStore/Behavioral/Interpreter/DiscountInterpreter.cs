namespace SmartStore.Behavioral.Interpreter;

// --- Expression Interface ---

public interface IDiscountExpression
{
    decimal Interpret(Dictionary<string, decimal> context);
    string Description { get; }
}

// --- Terminal Expressions ---

public class NumberExpression : IDiscountExpression
{
    private readonly decimal _value;
    public NumberExpression(decimal value) => _value = value;
    public decimal Interpret(Dictionary<string, decimal> context) => _value;
    public string Description => _value.ToString("F2");
}

public class VariableExpression : IDiscountExpression
{
    private readonly string _name;
    public VariableExpression(string name) => _name = name;

    public decimal Interpret(Dictionary<string, decimal> context)
    {
        if (context.TryGetValue(_name, out var value)) return value;
        throw new KeyNotFoundException($"Variable '{_name}' not found in context");
    }

    public string Description => _name;
}

// --- Non-Terminal Expressions ---

public class PercentDiscountExpression : IDiscountExpression
{
    private readonly IDiscountExpression _basePrice;
    private readonly IDiscountExpression _percentage;

    public PercentDiscountExpression(IDiscountExpression basePrice, IDiscountExpression percentage)
    {
        _basePrice = basePrice;
        _percentage = percentage;
    }

    public decimal Interpret(Dictionary<string, decimal> context)
    {
        var price = _basePrice.Interpret(context);
        var pct = _percentage.Interpret(context);
        return price * (pct / 100m);
    }

    public string Description => $"{_percentage.Description}% of {_basePrice.Description}";
}

public class FixedDiscountExpression : IDiscountExpression
{
    private readonly IDiscountExpression _discount;
    public FixedDiscountExpression(IDiscountExpression discount) => _discount = discount;
    public decimal Interpret(Dictionary<string, decimal> context) => _discount.Interpret(context);
    public string Description => $"Fixed discount: {_discount.Description}";
}

public class MaxExpression : IDiscountExpression
{
    private readonly IDiscountExpression _left;
    private readonly IDiscountExpression _right;

    public MaxExpression(IDiscountExpression left, IDiscountExpression right)
    {
        _left = left;
        _right = right;
    }

    public decimal Interpret(Dictionary<string, decimal> context)
    {
        return Math.Max(_left.Interpret(context), _right.Interpret(context));
    }

    public string Description => $"Max({_left.Description}, {_right.Description})";
}

public class ConditionalDiscountExpression : IDiscountExpression
{
    private readonly IDiscountExpression _condition;
    private readonly decimal _threshold;
    private readonly IDiscountExpression _discount;

    public ConditionalDiscountExpression(IDiscountExpression condition, decimal threshold, IDiscountExpression discount)
    {
        _condition = condition;
        _threshold = threshold;
        _discount = discount;
    }

    public decimal Interpret(Dictionary<string, decimal> context)
    {
        if (_condition.Interpret(context) >= _threshold)
            return _discount.Interpret(context);
        return 0;
    }

    public string Description => $"If {_condition.Description} >= {_threshold} then {_discount.Description}";
}
