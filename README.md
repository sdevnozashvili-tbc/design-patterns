# Design Patterns - SmartStore (E-Commerce Order Management System)

პრაქტიკული პროექტი, რომელშიც 19 Design Pattern არის გამოყენებული რეალურ E-Commerce კონტექსტში.

## პროექტის გაშვება

```bash
cd src/SmartStore
dotnet run
```

## Design Patterns

### Creational Patterns
| # | Pattern | გამოყენება პროექტში |
|---|---------|---------------------|
| 1 | **Abstract Factory** | UI Theme Factory — ქმნის Dark/Light თემის UI კომპონენტების ოჯახებს |
| 2 | **Builder** | Order Builder — რთული შეკვეთის ნაბიჯ-ნაბიჯ აგება |
| 3 | **Prototype** | Product Prototype Registry — პროდუქტის შაბლონების კლონირება |

### Structural Patterns
| # | Pattern | გამოყენება პროექტში |
|---|---------|---------------------|
| 4 | **Decorator** | Shipping Decorator — მიწოდების სერვისზე ფუნქციების დინამიურად დამატება |
| 5 | **Adapter** | Payment Adapter — Stripe/PayPal API-ების ერთიან ინტერფეისზე ადაპტაცია |
| 6 | **Composite** | Product Catalog — კატეგორიებისა და პროდუქტების ხის სტრუქტურა |
| 7 | **Proxy** | Product Image Proxy — მძიმე სურათების ლაზი ჩატვირთვა |
| 8 | **Flyweight** | Product Icon Factory — გაზიარებული icon ობიექტები |
| 9 | **Facade** | Order Processing Facade — რთული შეკვეთის დამუშავების გამარტივება |
| 10 | **Bridge** | Notification Bridge — შეტყობინების ტიპისა და არხის გამიჯვნა |

### Behavioral Patterns
| # | Pattern | გამოყენება პროექტში |
|---|---------|---------------------|
| 11 | **Template Method** | Order Exporter — შეკვეთის CSV/JSON/XML ფორმატში ექსპორტი |
| 12 | **Observer** | Order Status Observer — შეკვეთის სტატუსის ცვლილების შეტყობინებები |
| 13 | **Command** | Cart Commands — კალათის ოპერაციები Undo-ს მხარდაჭერით |
| 14 | **Iterator** | Product Iterator — პროდუქტების კოლექციის სხვადასხვა სტრატეგიით იტერაცია |
| 15 | **Chain of Responsibility** | Order Validation — შეკვეთის ვალიდაციის ჯაჭვი |
| 16 | **State** | Order State Machine — შეკვეთის სასიცოცხლო ციკლის მართვა |
| 17 | **Visitor** | Product Visitor — გადასახადის/ფასდაკლების/მიტანის კალკულაცია |
| 18 | **Interpreter** | Discount Interpreter — ფასდაკლების წესების ინტერპრეტაცია |
| 19 | **Memento** | Cart Memento — კალათის მდგომარეობის შენახვა და აღდგენა |

## პროექტის სტრუქტურა

```
src/SmartStore/
├── Program.cs                              # მენიუ და entry point
├── Models/                                 # საერთო მოდელები
│   ├── Product.cs
│   ├── Order.cs
│   └── Customer.cs
├── Creational/                             # Creational Patterns
│   ├── AbstractFactory/ThemeFactory.cs
│   ├── Builder/OrderBuilder.cs
│   └── Prototype/ProductPrototype.cs
├── Structural/                             # Structural Patterns
│   ├── Decorator/ShippingDecorator.cs
│   ├── Adapter/PaymentAdapter.cs
│   ├── Composite/ProductCatalog.cs
│   ├── Proxy/ProductImageProxy.cs
│   ├── Flyweight/ProductIconFlyweight.cs
│   ├── Facade/OrderProcessingFacade.cs
│   └── Bridge/NotificationBridge.cs
├── Behavioral/                             # Behavioral Patterns
│   ├── TemplateMethod/OrderExporter.cs
│   ├── Observer/OrderObserver.cs
│   ├── Command/CartCommand.cs
│   ├── Iterator/ProductIterator.cs
│   ├── ChainOfResponsibility/OrderValidation.cs
│   ├── State/OrderState.cs
│   ├── Visitor/ProductVisitor.cs
│   ├── Interpreter/DiscountInterpreter.cs
│   └── Memento/CartMemento.cs
└── Demos/                                  # დემო runner
    └── DemoRunner.cs
```