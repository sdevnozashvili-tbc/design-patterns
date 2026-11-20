namespace SmartStore.Creational.Prototype;

using SmartStore.Models;

public class ProductPrototypeRegistry
{
    private readonly Dictionary<string, Product> _prototypes = new();

    public void RegisterPrototype(string key, Product product)
    {
        _prototypes[key] = product;
    }

    public Product CreateProduct(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
            throw new KeyNotFoundException($"Prototype '{key}' not found.");

        return prototype.DeepClone();
    }

    public IEnumerable<string> GetRegisteredKeys() => _prototypes.Keys;
}
