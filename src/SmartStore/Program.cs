using SmartStore.Demos;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var demos = new Dictionary<string, (string Name, Action Run)>
{
    // Creational Patterns
    ["1"]  = ("Abstract Factory",           DemoRunner.RunAbstractFactory),
    ["2"]  = ("Builder",                    DemoRunner.RunBuilder),
    ["3"]  = ("Prototype",                  DemoRunner.RunPrototype),

    // Structural Patterns
    ["4"]  = ("Decorator",                  DemoRunner.RunDecorator),
    ["5"]  = ("Adapter",                    DemoRunner.RunAdapter),
    ["6"]  = ("Composite",                  DemoRunner.RunComposite),
    ["7"]  = ("Proxy",                      DemoRunner.RunProxy),
    ["8"]  = ("Flyweight",                  DemoRunner.RunFlyweight),
    ["9"]  = ("Facade",                     DemoRunner.RunFacade),
    ["10"] = ("Bridge",                     DemoRunner.RunBridge),

    // Behavioral Patterns
    ["11"] = ("Template Method",            DemoRunner.RunTemplateMethod),
    ["12"] = ("Observer",                   DemoRunner.RunObserver),
    ["13"] = ("Command",                    DemoRunner.RunCommand),
    ["14"] = ("Iterator",                   DemoRunner.RunIterator),
    ["15"] = ("Chain of Responsibility",    DemoRunner.RunChainOfResponsibility),
    ["16"] = ("State",                      DemoRunner.RunState),
    ["17"] = ("Visitor",                    DemoRunner.RunVisitor),
    ["18"] = ("Interpreter",               DemoRunner.RunInterpreter),
    ["19"] = ("Memento",                    DemoRunner.RunMemento),
};

while (true)
{
    Console.Clear();
    Console.WriteLine("======================================================================");
    Console.WriteLine("   SmartStore - Design Patterns Demo");
    Console.WriteLine("   E-Commerce Order Management System");
    Console.WriteLine("======================================================================");

    Console.WriteLine("\n  CREATIONAL PATTERNS:");
    Console.WriteLine("    1.  Abstract Factory    - UI Theme Factory");
    Console.WriteLine("    2.  Builder             - Order Builder");
    Console.WriteLine("    3.  Prototype           - Product Template Cloning");

    Console.WriteLine("\n  STRUCTURAL PATTERNS:");
    Console.WriteLine("    4.  Decorator           - Shipping Feature Decorator");
    Console.WriteLine("    5.  Adapter             - Payment Gateway Adapter");
    Console.WriteLine("    6.  Composite           - Product Catalog Tree");
    Console.WriteLine("    7.  Proxy               - Lazy Image Loading Proxy");
    Console.WriteLine("    8.  Flyweight           - Shared Product Icons");
    Console.WriteLine("    9.  Facade              - Order Processing Facade");
    Console.WriteLine("    10. Bridge              - Notification Bridge");

    Console.WriteLine("\n  BEHAVIORAL PATTERNS:");
    Console.WriteLine("    11. Template Method     - Order Export Formats");
    Console.WriteLine("    12. Observer            - Order Status Notifications");
    Console.WriteLine("    13. Command             - Cart Operations with Undo");
    Console.WriteLine("    14. Iterator            - Product Collection Iterator");
    Console.WriteLine("    15. Chain of Resp.      - Order Validation Chain");
    Console.WriteLine("    16. State               - Order State Machine");
    Console.WriteLine("    17. Visitor             - Product Tax/Discount Calculator");
    Console.WriteLine("    18. Interpreter         - Discount Rule Interpreter");
    Console.WriteLine("    19. Memento             - Cart State Save/Restore");

    Console.WriteLine("\n    0.  Run ALL demos");
    Console.WriteLine("    Q.  Quit");

    Console.Write("\n  Select a pattern (0-19 or Q): ");
    var input = Console.ReadLine()?.Trim().ToUpper();

    if (input == "Q") break;

    if (input == "0")
    {
        Console.Clear();
        foreach (var (_, (name, run)) in demos.OrderBy(d => int.Parse(d.Key)))
        {
            run();
            Console.WriteLine();
        }
        Console.Write("\n  Press any key to return to menu...");
        Console.ReadKey();
    }
    else if (input != null && demos.TryGetValue(input, out var demo))
    {
        Console.Clear();
        demo.Run();
        Console.Write("\n  Press any key to return to menu...");
        Console.ReadKey();
    }
}
