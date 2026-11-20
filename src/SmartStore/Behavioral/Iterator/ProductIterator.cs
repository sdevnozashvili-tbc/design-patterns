namespace SmartStore.Behavioral.Iterator;

using SmartStore.Models;

public interface IProductIterator
{
    bool HasNext();
    Product Next();
    void Reset();
}

public interface IProductCollection
{
    IProductIterator CreateIterator();
    IProductIterator CreateFilteredIterator(Func<Product, bool> predicate);
    IProductIterator CreateSortedIterator(Func<Product, object> keySelector);
    void Add(Product product);
    int Count { get; }
}

public class ProductCollection : IProductCollection
{
    private readonly List<Product> _products = new();

    public void Add(Product product) => _products.Add(product);
    public int Count => _products.Count;

    public IProductIterator CreateIterator() => new ProductIterator(_products);

    public IProductIterator CreateFilteredIterator(Func<Product, bool> predicate) =>
        new ProductIterator(_products.Where(predicate).ToList());

    public IProductIterator CreateSortedIterator(Func<Product, object> keySelector) =>
        new ProductIterator(_products.OrderBy(keySelector).ToList());
}

public class ProductIterator : IProductIterator
{
    private readonly List<Product> _products;
    private int _position;

    public ProductIterator(List<Product> products)
    {
        _products = products;
        _position = 0;
    }

    public bool HasNext() => _position < _products.Count;

    public Product Next()
    {
        if (!HasNext()) throw new InvalidOperationException("No more products.");
        return _products[_position++];
    }

    public void Reset() => _position = 0;
}
