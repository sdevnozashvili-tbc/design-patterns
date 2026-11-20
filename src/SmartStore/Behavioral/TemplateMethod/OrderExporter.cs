namespace SmartStore.Behavioral.TemplateMethod;

using SmartStore.Models;

public abstract class OrderExporter
{
    // Template method - defines the algorithm skeleton
    public string Export(Order order)
    {
        var result = new System.Text.StringBuilder();
        result.AppendLine(WriteHeader());
        result.AppendLine(WriteOrderInfo(order));
        foreach (var item in order.Items)
        {
            result.AppendLine(WriteItem(item));
        }
        result.AppendLine(WriteFooter(order));
        return result.ToString();
    }

    protected abstract string WriteHeader();
    protected abstract string WriteOrderInfo(Order order);
    protected abstract string WriteItem(OrderItem item);
    protected abstract string WriteFooter(Order order);
}

public class CsvExporter : OrderExporter
{
    protected override string WriteHeader() => "Type,Field1,Field2,Field3,Field4";
    protected override string WriteOrderInfo(Order order) =>
        $"ORDER,{order.Id},{order.Customer?.Name},{order.CreatedAt:yyyy-MM-dd},{order.Status}";
    protected override string WriteItem(OrderItem item) =>
        $"ITEM,{item.Product.Name},{item.Quantity},{item.UnitPrice},{item.TotalPrice}";
    protected override string WriteFooter(Order order) =>
        $"TOTAL,{order.TotalAmount},,,";
}

public class JsonExporter : OrderExporter
{
    protected override string WriteHeader() => "{";
    protected override string WriteOrderInfo(Order order) =>
        $"  \"orderId\": {order.Id}, \"customer\": \"{order.Customer?.Name}\", \"items\": [";
    protected override string WriteItem(OrderItem item) =>
        $"    {{ \"product\": \"{item.Product.Name}\", \"qty\": {item.Quantity}, \"price\": {item.UnitPrice} }},";
    protected override string WriteFooter(Order order) =>
        $"  ], \"total\": {order.TotalAmount}\n}}";
}

public class XmlExporter : OrderExporter
{
    protected override string WriteHeader() => "<?xml version=\"1.0\"?>\n<Order>";
    protected override string WriteOrderInfo(Order order) =>
        $"  <Id>{order.Id}</Id>\n  <Customer>{order.Customer?.Name}</Customer>\n  <Items>";
    protected override string WriteItem(OrderItem item) =>
        $"    <Item name=\"{item.Product.Name}\" qty=\"{item.Quantity}\" price=\"{item.UnitPrice}\" />";
    protected override string WriteFooter(Order order) =>
        $"  </Items>\n  <Total>{order.TotalAmount}</Total>\n</Order>";
}
