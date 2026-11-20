namespace SmartStore.Structural.Proxy;

public interface IProductImage
{
    void Display();
    string GetInfo();
}

public class RealProductImage : IProductImage
{
    private readonly string _filename;
    private readonly byte[] _imageData;

    public RealProductImage(string filename)
    {
        _filename = filename;
        _imageData = LoadFromDisk(filename);
    }

    private byte[] LoadFromDisk(string filename)
    {
        Console.WriteLine($"  Loading heavy image from disk: {filename}...");
        Thread.Sleep(300);
        return new byte[1024 * 1024]; // Simulate 1MB image
    }

    public void Display()
    {
        Console.WriteLine($"  Displaying image: {_filename} ({_imageData.Length / 1024}KB)");
    }

    public string GetInfo() => $"{_filename} ({_imageData.Length / 1024}KB)";
}

public class ProductImageProxy : IProductImage
{
    private readonly string _filename;
    private RealProductImage? _realImage;

    public ProductImageProxy(string filename)
    {
        _filename = filename;
    }

    public void Display()
    {
        if (_realImage == null)
        {
            Console.WriteLine($"  Proxy: Loading image on first access...");
            _realImage = new RealProductImage(_filename);
        }
        _realImage.Display();
    }

    public string GetInfo()
    {
        if (_realImage == null)
            return $"{_filename} (not yet loaded)";
        return _realImage.GetInfo();
    }
}
