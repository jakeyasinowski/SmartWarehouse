namespace SmartWarehouse.Core;

public class WarehouseSensor
{
    public string SensorId { get; private set; }
    public string LocationTag { get; set; }
    public double CurrentTemperature { get; set; } = 0.0;
    public bool IsActive { get; set; } = false;
    public bool IsAlertTriggered { get; private set; } = false;
    public double CriticalThresholdCelsius { get; set; } = 4.0;
}

public WarehouseSensor(string sensorId, string locationTag)
{
    if (string.IsNullOrWhiteSpace(sensorId))
        throw new ArgumentException("SensorId cannot be null or empty.");

    if (string.IsNullOrWhiteSpace(locationTag))
        throw new ArgumentException("LocationTag cannot be null or empty.");

    SensorId = sensorId;
    LocationTag = locationTag;

    CurrentTemperature = 0.0;
    IsActive = false;
    IsAlertTriggered = false;
    CriticalThresholdCelsius = 4.0;
}

public void Activate()
{
    IsActive = true;
}

public void Deactivate()
{
    IsActive = false;
    IsAlertTriggered = false;
}

public void RecordReading(double newTemperature)
{
    if (IsActive == false)
        throw new InvalidOperationException("Sensor is not active");

    if (newTemperature < -50.0 || newTemperature > 80.0)
        throw new ArgumentOutOfRangeException("Temperature is out of range");

    CurrentTemperature = newTemperature;

    if (newTemperature >= CriticalThresholdCelsius)
        IsAlertTriggered = true;
    else
        IsAlertTriggered = false;
}

public void UpdateThreshold(double newThreshold)
{
    if (newThreshold < -30.0 || newThreshold > 50.0)
        throw new ArgumentOutOfRangeException("Threshold is out of range");

    CriticalThresholdCelsius = newThreshold;

    if (CurrentTemperature >= CriticalThresholdCelsius)
        IsAlertTriggered = true;
    else
        IsAlertTriggered = false;
}