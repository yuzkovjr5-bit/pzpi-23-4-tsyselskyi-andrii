// Інтерфейс Спостерігача
public interface IObserver
{
    void Update(string eventType, object data);
}

// Інтерфейс Суб'єкта
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string eventType, object data);
}

// Конкретний суб'єкт — сервер температурних даних
public class TemperatureSensor : ISubject
{
    private readonly List<IObserver> _observers = new();
    private double _temperature;

    public double Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            Notify("TemperatureChanged", value);
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);

    public void Notify(string eventType, object data)
    {
        foreach (var observer in _observers)
            observer.Update(eventType, data);
    }
}

// Конкретний спостерігач — дисплей
public class DisplayObserver : IObserver
{
    private readonly string _name;

    public DisplayObserver(string name) => _name = name;

    public void Update(string eventType, object data)
    {
        Console.WriteLine($"[{_name}] Подія: {eventType}, Значення: {data}");
    }
}

// Конкретний спостерігач — реєстратор
public class LoggerObserver : IObserver
{
    public void Update(string eventType, object data)
    {
        Console.WriteLine($"[LOG] {DateTime.Now:HH:mm:ss} | {eventType}: {data}");
    }
}

// Точка входу
class Program
{
    static void Main()
    {
        var sensor = new TemperatureSensor();
        var display = new DisplayObserver("Головний дисплей");
        var logger = new LoggerObserver();

        sensor.Attach(display);
        sensor.Attach(logger);

        sensor.Temperature = 22.5;
        sensor.Temperature = 25.0;

        sensor.Detach(logger);

        sensor.Temperature = 30.1;
    }
}