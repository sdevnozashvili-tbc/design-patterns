namespace SmartStore.Creational.AbstractFactory;

// --- Abstract Products ---

public interface IButton
{
    string Render();
}

public interface ITextBox
{
    string Render();
}

public interface IPanel
{
    string Render();
}

// --- Abstract Factory ---

public interface IThemeFactory
{
    IButton CreateButton(string text);
    ITextBox CreateTextBox(string placeholder);
    IPanel CreatePanel(string title);
}

// --- Dark Theme ---

public class DarkButton : IButton
{
    private readonly string _text;
    public DarkButton(string text) => _text = text;
    public string Render() => $"[Dark Button: {_text}]";
}

public class DarkTextBox : ITextBox
{
    private readonly string _placeholder;
    public DarkTextBox(string placeholder) => _placeholder = placeholder;
    public string Render() => $"[Dark TextBox: {_placeholder}]";
}

public class DarkPanel : IPanel
{
    private readonly string _title;
    public DarkPanel(string title) => _title = title;
    public string Render() => $"== Dark Panel: {_title} ==";
}

public class DarkThemeFactory : IThemeFactory
{
    public IButton CreateButton(string text) => new DarkButton(text);
    public ITextBox CreateTextBox(string placeholder) => new DarkTextBox(placeholder);
    public IPanel CreatePanel(string title) => new DarkPanel(title);
}

// --- Light Theme ---

public class LightButton : IButton
{
    private readonly string _text;
    public LightButton(string text) => _text = text;
    public string Render() => $"[Light Button: {_text}]";
}

public class LightTextBox : ITextBox
{
    private readonly string _placeholder;
    public LightTextBox(string placeholder) => _placeholder = placeholder;
    public string Render() => $"[Light TextBox: {_placeholder}]";
}

public class LightPanel : IPanel
{
    private readonly string _title;
    public LightPanel(string title) => _title = title;
    public string Render() => $"== Light Panel: {_title} ==";
}

public class LightThemeFactory : IThemeFactory
{
    public IButton CreateButton(string text) => new LightButton(text);
    public ITextBox CreateTextBox(string placeholder) => new LightTextBox(placeholder);
    public IPanel CreatePanel(string title) => new LightPanel(title);
}
