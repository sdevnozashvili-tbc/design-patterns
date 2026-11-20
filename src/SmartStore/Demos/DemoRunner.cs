namespace SmartStore.Demos;

using SmartStore.Models;
using SmartStore.Creational.AbstractFactory;
using SmartStore.Creational.Builder;
using SmartStore.Creational.Prototype;
using SmartStore.Structural.Decorator;
using SmartStore.Structural.Adapter;
using SmartStore.Structural.Composite;
using SmartStore.Structural.Proxy;
using SmartStore.Structural.Flyweight;
using SmartStore.Structural.Facade;
using SmartStore.Structural.Bridge;
using SmartStore.Behavioral.TemplateMethod;
using SmartStore.Behavioral.Observer;
using SmartStore.Behavioral.Command;
using SmartStore.Behavioral.Iterator;
using SmartStore.Behavioral.ChainOfResponsibility;
using SmartStore.Behavioral.State;
using SmartStore.Behavioral.Visitor;
using SmartStore.Behavioral.Interpreter;
using SmartStore.Behavioral.Memento;

public static class DemoRunner
{
    private static readonly Customer SampleCustomer = new()
    {
        Id = 1, Name = "Giorgi Beridze", Email = "giorgi@example.com",
        Phone = "+995555123456", Address = "Tbilisi, Rustaveli Ave 12"
    };

    private static readonly Product Laptop = new()
    {
        Id = 1, Name = "Laptop", Category = "Electronics", Price = 1299.99m,
        StockQuantity = 50, ImagePath = "laptop.jpg", Weight = 2.1
    };

    private static readonly Product Phone = new()
    {
        Id = 2, Name = "Smartphone", Category = "Electronics", Price = 799.99m,
        StockQuantity = 100, ImagePath = "phone.jpg", Weight = 0.2
    };

    private static readonly Product TShirt = new()
    {
        Id = 3, Name = "T-Shirt", Category = "Clothing", Price = 29.99m,
        StockQuantity = 200, ImagePath = "tshirt.jpg", Weight = 0.3
    };

    // =============================================
    // 1. ABSTRACT FACTORY
    // =============================================
    public static void RunAbstractFactory()
    {
        PrintHeader("Abstract Factory", "UI Theme Factory - creates families of themed UI components");

        void RenderUI(IThemeFactory factory, string themeName)
        {
            Console.WriteLine($"\n  --- {themeName} Theme ---");
            var panel = factory.CreatePanel("Product Dashboard");
            var searchBox = factory.CreateTextBox("Search products...");
            var buyButton = factory.CreateButton("Buy Now");
            var cartButton = factory.CreateButton("Add to Cart");

            Console.WriteLine($"  {panel.Render()}");
            Console.WriteLine($"  {searchBox.Render()}");
            Console.WriteLine($"  {buyButton.Render()}");
            Console.WriteLine($"  {cartButton.Render()}");
        }

        RenderUI(new DarkThemeFactory(), "Dark");
        RenderUI(new LightThemeFactory(), "Light");
    }

    // =============================================
    // 2. BUILDER
    // =============================================
    public static void RunBuilder()
    {
        PrintHeader("Builder", "Order Builder - builds complex orders step by step");

        var director = new OrderDirector();

        Console.WriteLine("\n  --- Standard Order (via Director) ---");
        var standardOrder = director.CreateStandardOrder(new OrderBuilder(), SampleCustomer, Laptop);
        Console.WriteLine($"  {standardOrder}");

        Console.WriteLine("\n  --- Bulk Order (via Director) ---");
        var items = new List<(Product, int)> { (Laptop, 5), (Phone, 10), (TShirt, 50) };
        var bulkOrder = director.CreateBulkOrder(new OrderBuilder(), SampleCustomer, items);
        Console.WriteLine($"  {bulkOrder}");
        Console.WriteLine($"  Notes: {bulkOrder.Notes}");

        Console.WriteLine("\n  --- Custom Order (manual builder) ---");
        var customOrder = new OrderBuilder()
            .SetCustomer(SampleCustomer)
            .AddItem(Phone, 2)
            .AddItem(TShirt, 3)
            .SetShippingAddress("Batumi, Seaside Blvd 5")
            .SetPaymentMethod("PayPal")
            .AddNotes("Gift wrap requested")
            .Build();
        Console.WriteLine($"  {customOrder}");
    }

    // =============================================
    // 3. PROTOTYPE
    // =============================================
    public static void RunPrototype()
    {
        PrintHeader("Prototype", "Product Prototype Registry - clone product templates");

        var registry = new ProductPrototypeRegistry();
        registry.RegisterPrototype("laptop", Laptop);
        registry.RegisterPrototype("phone", Phone);
        registry.RegisterPrototype("tshirt", TShirt);

        Console.WriteLine($"\n  Registered prototypes: {string.Join(", ", registry.GetRegisteredKeys())}");

        var clonedLaptop = registry.CreateProduct("laptop");
        clonedLaptop.Id = 101;
        clonedLaptop.Name = "Gaming Laptop";
        clonedLaptop.Price = 1899.99m;

        var clonedPhone = registry.CreateProduct("phone");
        clonedPhone.Id = 102;
        clonedPhone.Name = "Budget Phone";
        clonedPhone.Price = 299.99m;

        Console.WriteLine("\n  Original products:");
        Console.WriteLine($"    {Laptop}");
        Console.WriteLine($"    {Phone}");

        Console.WriteLine("\n  Cloned & modified products:");
        Console.WriteLine($"    {clonedLaptop}");
        Console.WriteLine($"    {clonedPhone}");

        Console.WriteLine($"\n  Original laptop price unchanged: {Laptop.Price:C} (proves deep clone)");
    }

    // =============================================
    // 4. DECORATOR
    // =============================================
    public static void RunDecorator()
    {
        PrintHeader("Decorator", "Shipping Decorator - add features to shipping dynamically");

        IShippingService shipping = new StandardShipping();
        Console.WriteLine($"\n  {shipping.GetDescription()} = {shipping.CalculateCost():C}");

        shipping = new GiftWrapDecorator(shipping);
        Console.WriteLine($"  {shipping.GetDescription()} = {shipping.CalculateCost():C}");

        shipping = new InsuranceDecorator(shipping);
        Console.WriteLine($"  {shipping.GetDescription()} = {shipping.CalculateCost():C}");

        shipping = new ExpressDeliveryDecorator(shipping);
        Console.WriteLine($"  {shipping.GetDescription()} = {shipping.CalculateCost():C}");

        Console.WriteLine("\n  --- Another combination ---");
        IShippingService express = new ExpressDeliveryDecorator(new StandardShipping());
        Console.WriteLine($"  {express.GetDescription()} = {express.CalculateCost():C}");

        IShippingService insuredExpress = new InsuranceDecorator(new ExpressDeliveryDecorator(new StandardShipping()));
        Console.WriteLine($"  {insuredExpress.GetDescription()} = {insuredExpress.CalculateCost():C}");
    }

    // =============================================
    // 5. ADAPTER
    // =============================================
    public static void RunAdapter()
    {
        PrintHeader("Adapter", "Payment Adapter - adapt Stripe and PayPal APIs to unified interface");

        IPaymentProcessor stripe = new StripeAdapter();
        IPaymentProcessor paypal = new PayPalAdapter();

        Console.WriteLine("\n  --- Processing with Stripe ---");
        var stripeResult = stripe.ProcessPayment("ORD-001", 99.99m, "USD");
        Console.WriteLine($"  Result: {(stripeResult ? "Success" : "Failed")}");
        Console.WriteLine($"  Status: {stripe.GetPaymentStatus("txn_123")}");

        Console.WriteLine("\n  --- Processing with PayPal ---");
        var paypalResult = paypal.ProcessPayment("ORD-002", 149.99m, "EUR");
        Console.WriteLine($"  Result: {(paypalResult ? "Success" : "Failed")}");
        Console.WriteLine($"  Status: {paypal.GetPaymentStatus("order_456")}");
    }

    // =============================================
    // 6. COMPOSITE
    // =============================================
    public static void RunComposite()
    {
        PrintHeader("Composite", "Product Catalog - tree structure for categories and products");

        var root = new CategoryComposite("SmartStore Catalog");

        var electronics = new CategoryComposite("Electronics");
        var laptops = new CategoryComposite("Laptops");
        laptops.Add(new ProductLeaf("MacBook Pro", 2499.99m));
        laptops.Add(new ProductLeaf("ThinkPad X1", 1799.99m));
        laptops.Add(new ProductLeaf("Dell XPS 15", 1599.99m));

        var phones = new CategoryComposite("Phones");
        phones.Add(new ProductLeaf("iPhone 15", 999.99m));
        phones.Add(new ProductLeaf("Samsung Galaxy S24", 899.99m));

        electronics.Add(laptops);
        electronics.Add(phones);

        var clothing = new CategoryComposite("Clothing");
        clothing.Add(new ProductLeaf("Cotton T-Shirt", 29.99m));
        clothing.Add(new ProductLeaf("Jeans", 59.99m));
        clothing.Add(new ProductLeaf("Jacket", 129.99m));

        root.Add(electronics);
        root.Add(clothing);

        Console.WriteLine();
        root.Display();
    }

    // =============================================
    // 7. PROXY
    // =============================================
    public static void RunProxy()
    {
        PrintHeader("Proxy", "Product Image Proxy - lazy-load heavy images on demand");

        Console.WriteLine("\n  Creating image proxies (no loading yet)...");
        var image1 = new ProductImageProxy("laptop_4k.png");
        var image2 = new ProductImageProxy("phone_hd.png");
        var image3 = new ProductImageProxy("tshirt_hd.png");

        Console.WriteLine($"  Image 1 info: {image1.GetInfo()}");
        Console.WriteLine($"  Image 2 info: {image2.GetInfo()}");
        Console.WriteLine($"  Image 3 info: {image3.GetInfo()}");

        Console.WriteLine("\n  Displaying image 1 (triggers loading):");
        image1.Display();

        Console.WriteLine("\n  Displaying image 1 again (already loaded):");
        image1.Display();

        Console.WriteLine("\n  Displaying image 2 (triggers loading):");
        image2.Display();

        Console.WriteLine($"\n  Image 3 still not loaded: {image3.GetInfo()}");
    }

    // =============================================
    // 8. FLYWEIGHT
    // =============================================
    public static void RunFlyweight()
    {
        PrintHeader("Flyweight", "Product Icon Factory - share icon objects across products");

        var factory = new ProductIconFactory();

        Console.WriteLine("\n  Creating icons for products (notice shared objects):");
        var icon1 = factory.GetIcon("electronics", "blue", "[E]");
        var icon2 = factory.GetIcon("clothing", "green", "[C]");
        var icon3 = factory.GetIcon("electronics", "blue", "[E]"); // Reused!
        var icon4 = factory.GetIcon("food", "red", "[F]");
        var icon5 = factory.GetIcon("clothing", "green", "[C]"); // Reused!
        var icon6 = factory.GetIcon("electronics", "blue", "[E]"); // Reused!

        Console.WriteLine($"\n  Total unique icons created: {factory.GetIconCount()} (out of 6 requests)");

        Console.WriteLine("\n  Rendering icons with extrinsic state (position + product name):");
        icon1.Render(10, 20, "Laptop");
        icon2.Render(30, 20, "T-Shirt");
        icon3.Render(50, 20, "Tablet");
        icon4.Render(10, 40, "Apple");
        icon5.Render(30, 40, "Jeans");
        icon6.Render(50, 40, "Headphones");
    }

    // =============================================
    // 9. FACADE
    // =============================================
    public static void RunFacade()
    {
        PrintHeader("Facade", "Order Processing Facade - simplifies complex order processing workflow");

        var order = new OrderBuilder()
            .SetCustomer(SampleCustomer)
            .AddItem(Laptop, 1)
            .AddItem(TShirt, 2)
            .SetShippingAddress(SampleCustomer.Address)
            .SetPaymentMethod("Credit Card")
            .Build();

        Console.WriteLine($"\n  Order created: {order}");

        var facade = new OrderProcessingFacade();
        facade.ProcessOrder(order);
    }

    // =============================================
    // 10. BRIDGE
    // =============================================
    public static void RunBridge()
    {
        PrintHeader("Bridge", "Notification Bridge - decouple message type from delivery channel");

        var emailSender = new EmailSender();
        var smsSender = new SmsSender();
        var pushSender = new PushNotificationSender();

        Console.WriteLine("\n  --- Order Notification via different channels ---");
        var orderNotif = new OrderNotification(emailSender, 42, "Shipped");
        orderNotif.Notify("giorgi@example.com");

        orderNotif.SetSender(smsSender);
        orderNotif.Notify("+995555123456");

        orderNotif.SetSender(pushSender);
        orderNotif.Notify("giorgi_device_token");

        Console.WriteLine("\n  --- Promotion Notification via different channels ---");
        var promoNotif = new PromotionNotification(emailSender, "Summer Sale", 25);
        promoNotif.Notify("giorgi@example.com");

        promoNotif.SetSender(smsSender);
        promoNotif.Notify("+995555123456");
    }

    // =============================================
    // 11. TEMPLATE METHOD
    // =============================================
    public static void RunTemplateMethod()
    {
        PrintHeader("Template Method", "Order Exporter - export orders in different formats using same algorithm");

        var order = new OrderBuilder()
            .SetCustomer(SampleCustomer)
            .AddItem(Laptop, 1)
            .AddItem(Phone, 2)
            .SetShippingAddress(SampleCustomer.Address)
            .SetPaymentMethod("Credit Card")
            .Build();

        OrderExporter[] exporters = [new CsvExporter(), new JsonExporter(), new XmlExporter()];
        string[] names = ["CSV", "JSON", "XML"];

        for (int i = 0; i < exporters.Length; i++)
        {
            Console.WriteLine($"\n  --- {names[i]} Export ---");
            Console.WriteLine(exporters[i].Export(order));
        }
    }

    // =============================================
    // 12. OBSERVER
    // =============================================
    public static void RunObserver()
    {
        PrintHeader("Observer", "Order Status Observer - notify multiple systems on status changes");

        var order = new OrderBuilder()
            .SetCustomer(SampleCustomer)
            .AddItem(Laptop, 1)
            .SetShippingAddress(SampleCustomer.Address)
            .SetPaymentMethod("Credit Card")
            .Build();

        var observable = new ObservableOrder(order);
        var customerNotifier = new CustomerNotifier();
        var inventoryObserver = new InventoryObserver();
        var analyticsTracker = new AnalyticsTracker();

        observable.Attach(customerNotifier);
        observable.Attach(inventoryObserver);
        observable.Attach(analyticsTracker);

        observable.ChangeStatus("Processing");
        observable.ChangeStatus("Shipped");
        observable.ChangeStatus("Delivered");
    }

    // =============================================
    // 13. COMMAND
    // =============================================
    public static void RunCommand()
    {
        PrintHeader("Command", "Cart Commands - shopping cart operations with undo support");

        var cart = new ShoppingCart();
        var invoker = new CartCommandInvoker();

        Console.WriteLine("\n  --- Adding items ---");
        invoker.ExecuteCommand(new AddToCartCommand(cart, Laptop, 1));
        invoker.ExecuteCommand(new AddToCartCommand(cart, Phone, 2));
        invoker.ExecuteCommand(new AddToCartCommand(cart, TShirt, 3));
        cart.Display();

        Console.WriteLine("\n  --- Removing 1 phone ---");
        invoker.ExecuteCommand(new RemoveFromCartCommand(cart, Phone, 1));
        cart.Display();

        Console.WriteLine("\n  --- Undo last operation (restore phone) ---");
        invoker.Undo();
        cart.Display();

        Console.WriteLine("\n  --- Undo again (restore t-shirts) ---");
        invoker.Undo();
        cart.Display();
    }

    // =============================================
    // 14. ITERATOR
    // =============================================
    public static void RunIterator()
    {
        PrintHeader("Iterator", "Product Iterator - iterate over products with different strategies");

        var collection = new ProductCollection();
        collection.Add(new Product { Id = 1, Name = "Laptop", Category = "Electronics", Price = 1299.99m });
        collection.Add(new Product { Id = 2, Name = "T-Shirt", Category = "Clothing", Price = 29.99m });
        collection.Add(new Product { Id = 3, Name = "Headphones", Category = "Electronics", Price = 199.99m });
        collection.Add(new Product { Id = 4, Name = "Jeans", Category = "Clothing", Price = 59.99m });
        collection.Add(new Product { Id = 5, Name = "Tablet", Category = "Electronics", Price = 599.99m });
        collection.Add(new Product { Id = 6, Name = "Jacket", Category = "Clothing", Price = 129.99m });

        Console.WriteLine("\n  --- All Products ---");
        var allIterator = collection.CreateIterator();
        while (allIterator.HasNext())
            Console.WriteLine($"    {allIterator.Next()}");

        Console.WriteLine("\n  --- Electronics Only (Filtered) ---");
        var electronicsIterator = collection.CreateFilteredIterator(p => p.Category == "Electronics");
        while (electronicsIterator.HasNext())
            Console.WriteLine($"    {electronicsIterator.Next()}");

        Console.WriteLine("\n  --- Sorted by Price (Ascending) ---");
        var sortedIterator = collection.CreateSortedIterator(p => p.Price);
        while (sortedIterator.HasNext())
            Console.WriteLine($"    {sortedIterator.Next()}");
    }

    // =============================================
    // 15. CHAIN OF RESPONSIBILITY
    // =============================================
    public static void RunChainOfResponsibility()
    {
        PrintHeader("Chain of Responsibility", "Order Validation Chain - validate orders through handler chain");

        var stockHandler = new StockValidationHandler();
        stockHandler
            .SetNext(new PaymentValidationHandler())
            .SetNext(new FraudCheckHandler())
            .SetNext(new AddressValidationHandler());

        Console.WriteLine("\n  --- Valid Order ---");
        var validOrder = new OrderBuilder()
            .SetCustomer(SampleCustomer)
            .AddItem(Laptop, 1)
            .SetShippingAddress(SampleCustomer.Address)
            .SetPaymentMethod("Credit Card")
            .Build();

        var result1 = stockHandler.Handle(validOrder);
        Console.WriteLine($"  Result: {(result1.IsValid ? "PASSED" : $"FAILED at {result1.HandlerName}: {result1.Message}")}");

        Console.WriteLine("\n  --- Order without payment method ---");
        var noPayment = new OrderBuilder()
            .SetCustomer(SampleCustomer)
            .AddItem(Laptop, 1)
            .SetShippingAddress(SampleCustomer.Address)
            .Build();

        var result2 = stockHandler.Handle(noPayment);
        Console.WriteLine($"  Result: {(result2.IsValid ? "PASSED" : $"FAILED at {result2.HandlerName}: {result2.Message}")}");

        Console.WriteLine("\n  --- Suspicious high-value order ---");
        var expensiveProduct = new Product { Id = 99, Name = "Diamond Ring", Price = 5500m, StockQuantity = 10 };
        var fraudOrder = new OrderBuilder()
            .SetCustomer(SampleCustomer)
            .AddItem(expensiveProduct, 3)
            .SetShippingAddress(SampleCustomer.Address)
            .SetPaymentMethod("Credit Card")
            .Build();

        var result3 = stockHandler.Handle(fraudOrder);
        Console.WriteLine($"  Result: {(result3.IsValid ? "PASSED" : $"FAILED at {result3.HandlerName}: {result3.Message}")}");
    }

    // =============================================
    // 16. STATE
    // =============================================
    public static void RunState()
    {
        PrintHeader("State", "Order State Machine - manage order lifecycle transitions");

        var order = new OrderBuilder()
            .SetCustomer(SampleCustomer)
            .AddItem(Laptop, 1)
            .SetShippingAddress(SampleCustomer.Address)
            .SetPaymentMethod("Credit Card")
            .Build();

        var context = new OrderContext(order);

        Console.WriteLine($"\n  Current state: {context.CurrentState.StateName}");

        Console.WriteLine("\n  --- Normal flow: New -> Processing -> Shipped -> Delivered ---");
        context.Process();
        context.Ship();
        context.Deliver();

        Console.WriteLine($"\n  Try to cancel delivered order:");
        context.Cancel();

        Console.WriteLine("\n  --- Cancellation flow ---");
        var order2 = new OrderBuilder()
            .SetCustomer(SampleCustomer)
            .AddItem(Phone, 1)
            .SetShippingAddress(SampleCustomer.Address)
            .SetPaymentMethod("Credit Card")
            .Build();

        var context2 = new OrderContext(order2);
        context2.Process();
        Console.WriteLine("  Cancelling during processing...");
        context2.Cancel();

        Console.WriteLine($"  Try to ship cancelled order:");
        context2.Ship();
    }

    // =============================================
    // 17. VISITOR
    // =============================================
    public static void RunVisitor()
    {
        PrintHeader("Visitor", "Product Visitor - apply different calculations to product types");

        var products = new List<IVisitableProduct>
        {
            new Electronics { Name = "Laptop", Price = 1299.99m, WarrantyYears = 2 },
            new Electronics { Name = "Headphones", Price = 199.99m, WarrantyYears = 1 },
            new Clothing { Name = "T-Shirt", Price = 29.99m, Size = "M" },
            new Clothing { Name = "Jacket", Price = 129.99m, Size = "L" },
            new Food { Name = "Organic Coffee", Price = 15.99m, ExpiryDate = DateTime.Now.AddDays(3) },
            new Food { Name = "Chocolate", Price = 8.99m, ExpiryDate = DateTime.Now.AddMonths(6) }
        };

        Console.WriteLine("\n  --- Tax Calculation ---");
        var taxVisitor = new TaxCalculatorVisitor();
        foreach (var p in products) p.Accept(taxVisitor);
        Console.WriteLine($"  >> {taxVisitor.GetResult()}");

        Console.WriteLine("\n  --- Discount Calculation ---");
        var discountVisitor = new DiscountVisitor();
        foreach (var p in products) p.Accept(discountVisitor);
        Console.WriteLine($"  >> {discountVisitor.GetResult()}");

        Console.WriteLine("\n  --- Shipping Cost Calculation ---");
        var shippingVisitor = new ShippingCostVisitor();
        foreach (var p in products) p.Accept(shippingVisitor);
        Console.WriteLine($"  >> {shippingVisitor.GetResult()}");
    }

    // =============================================
    // 18. INTERPRETER
    // =============================================
    public static void RunInterpreter()
    {
        PrintHeader("Interpreter", "Discount Rule Interpreter - parse and evaluate discount expressions");

        var context = new Dictionary<string, decimal>
        {
            { "orderTotal", 250.00m },
            { "itemCount", 5 },
            { "customerLoyaltyYears", 3 }
        };

        Console.WriteLine("  Context:");
        foreach (var (key, value) in context)
            Console.WriteLine($"    {key} = {value}");

        // Rule 1: 10% of order total
        var rule1 = new PercentDiscountExpression(
            new VariableExpression("orderTotal"),
            new NumberExpression(10));
        Console.WriteLine($"\n  Rule: {rule1.Description}");
        Console.WriteLine($"  Result: {rule1.Interpret(context):C}");

        // Rule 2: Fixed $15 discount
        var rule2 = new FixedDiscountExpression(new NumberExpression(15));
        Console.WriteLine($"\n  Rule: {rule2.Description}");
        Console.WriteLine($"  Result: {rule2.Interpret(context):C}");

        // Rule 3: Best of percentage or fixed discount
        var rule3 = new MaxExpression(rule1, rule2);
        Console.WriteLine($"\n  Rule: {rule3.Description}");
        Console.WriteLine($"  Result: {rule3.Interpret(context):C}");

        // Rule 4: If orderTotal >= 200, give 15% discount
        var rule4 = new ConditionalDiscountExpression(
            new VariableExpression("orderTotal"),
            200,
            new PercentDiscountExpression(
                new VariableExpression("orderTotal"),
                new NumberExpression(15)));
        Console.WriteLine($"\n  Rule: {rule4.Description}");
        Console.WriteLine($"  Result: {rule4.Interpret(context):C}");

        // Rule 5: If loyalty >= 2 years, additional $10
        var rule5 = new ConditionalDiscountExpression(
            new VariableExpression("customerLoyaltyYears"),
            2,
            new FixedDiscountExpression(new NumberExpression(10)));
        Console.WriteLine($"\n  Rule: {rule5.Description}");
        Console.WriteLine($"  Result: {rule5.Interpret(context):C}");
    }

    // =============================================
    // 19. MEMENTO
    // =============================================
    public static void RunMemento()
    {
        PrintHeader("Memento", "Cart Memento - save and restore shopping cart state");

        var cart = new MementoCart();
        var history = new CartHistory();

        Console.WriteLine("\n  --- Adding items and saving states ---");
        cart.AddItem(1, "Laptop", 1299.99m, 1);
        history.Save(cart.Save("Added laptop"));
        cart.Display();

        cart.AddItem(2, "Smartphone", 799.99m, 2);
        history.Save(cart.Save("Added 2 smartphones"));
        cart.Display();

        cart.AddItem(3, "T-Shirt", 29.99m, 5);
        history.Save(cart.Save("Added 5 t-shirts"));
        cart.Display();

        Console.WriteLine("\n  --- Removing laptop ---");
        cart.RemoveItem(1);
        cart.Display();

        Console.WriteLine("\n  --- Undo (restore to before laptop removal) ---");
        var memento = history.Undo();
        if (memento != null) cart.Restore(memento);
        cart.Display();

        Console.WriteLine("\n  --- Undo again (restore to before t-shirts) ---");
        memento = history.Undo();
        if (memento != null) cart.Restore(memento);
        cart.Display();

        Console.WriteLine("\n  --- History ---");
        history.ShowHistory();
    }

    // =============================================
    // HELPERS
    // =============================================
    private static void PrintHeader(string patternName, string description)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 70));
        Console.WriteLine($"  {patternName}");
        Console.WriteLine($"  {description}");
        Console.WriteLine(new string('=', 70));
    }
}
