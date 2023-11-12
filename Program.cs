using System;
using System.Collections.Generic;

// Observer interface
public interface IObserver
{
    void Update(float temperature, float humidity, float pressure);
}

// Display interface
public interface IDisplayElement
{
    void Display();
}

// Subject interface
public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}

// Concrete implementation of WeatherData (Subject)
public class WeatherData : ISubject
{
    private List<IObserver> observers;
    private float temperature;
    private float humidity;
    private float pressure;

    public WeatherData()
    {
        observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperature, humidity, pressure);
        }
    }

    public void SetMeasurements(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        this.pressure = pressure;
        MeasurementsChanged();
    }

    private void MeasurementsChanged()
    {
        NotifyObservers();
    }
}

// Concrete implementation of CurrentConditionsDisplay (Observer, Display)
public class CurrentConditionsDisplay : IObserver, IDisplayElement
{
    private float temperature;
    private float humidity;

    public void Update(float temperature, float humidity, float pressure)
    {
        this.temperature = temperature;
        this.humidity = humidity;
        Display();
    }

    public void Display()
    {
        Console.WriteLine($"Current Conditions: {temperature}F degrees and {humidity}% humidity");
    }
}

// Concrete implementation of StatisticsDisplay (Observer, Display)
public class StatisticsDisplay : IObserver, IDisplayElement
{
    private float temperatureSum;
    private int numReadings;

    public void Update(float temperature, float humidity, float pressure)
    {
        temperatureSum += temperature;
        numReadings++;
        Display();
    }

    public void Display()
    {
        float averageTemperature = temperatureSum / numReadings;
        Console.WriteLine($"Average Temperature: {averageTemperature}F");
    }
}

// Concrete implementation of ForecastDisplay (Observer, Display)
public class ForecastDisplay : IObserver, IDisplayElement
{
    private float lastPressure;
    private float currentPressure;

    public void Update(float temperature, float humidity, float pressure)
    {
        lastPressure = currentPressure;
        currentPressure = pressure;
        Display();
    }

    public void Display()
    {
        if (currentPressure > lastPressure)
        {
            Console.WriteLine("Forecast: Improving weather on the way!");
        }
        else if (currentPressure == lastPressure)
        {
            Console.WriteLine("Forecast: More of the same");
        }
        else
        {
            Console.WriteLine("Forecast: Watch out for cooler, rainy weather");
        }
    }
}

// Main program as a proof-of-concept
class Program
{
    static void Main(string[] args)
    {
        // Create WeatherData (Subject)
        WeatherData weatherData = new WeatherData();

        // Create and register observers (displays)
        CurrentConditionsDisplay currentDisplay = new CurrentConditionsDisplay();
        StatisticsDisplay statisticsDisplay = new StatisticsDisplay();
        ForecastDisplay forecastDisplay = new ForecastDisplay();

        weatherData.RegisterObserver(currentDisplay);
        weatherData.RegisterObserver(statisticsDisplay);
        weatherData.RegisterObserver(forecastDisplay);

        // Set static measurements for demonstration
        weatherData.SetMeasurements(80, 65, 30.4f);
        weatherData.SetMeasurements(82, 70, 29.2f);
        weatherData.SetMeasurements(78, 90, 29.2f);
    }
}
