using SmartWarehouse.Core;

namespace SmartWarehouse.Tests;


[TestClass]
public sealed class WarehouseSensorTests
{
    [TestMethod]
    public void InitCurrentTemperature_Zero()
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");

        // Act
        var actualTemperature = sensor.CurrentTemperature;

        // Assert
        Assert.AreEqual(0.0, actualTemperature);
    }

    [TestMethod]
    public void InitIsActive_False()
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");

        // Act
        var actualIsActive = sensor.IsActive;

        // Assert
        Assert.IsFalse(actualIsActive);
    }

    [TestMethod]
    public void InitIsAlertTriggered_False()
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");

        // Act
        var actualIsAlertTriggered = sensor.IsAlertTriggered;

        // Assert
        Assert.IsFalse(actualIsAlertTriggered);
    }

    [TestMethod]
    public void InitCriticalThresholdCelcius()
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");

        // Act
        var actualCriticalThresholdCelcius = sensor.CriticalThresholdCelsius;

        // Assert
        Assert.AreEqual(4.0, actualCriticalThresholdCelcius);
    }

    [TestMethod]
    public void sensorIdNotVoid()
    {
        // Arrange
        string? badId = "";

        // Act
        Action act = () => new WarehouseSensor(badId, "Sector1");

        // Assert
        Assert.ThrowsException<ArgumentException>(act);
    }

    [TestMethod]
    public void locationTagNotVoid()
    {
        // Arrange
        string? badId = "";

        // Act
        Action act = () => new WarehouseSensor("Test1", badId);

        // Assert
        Assert.ThrowsException<ArgumentException>(act);
    }

    [TestMethod]
    public void ActivateIsActive_True()
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");

        // Act
        sensor.Activate();

        // Assert
        Assert.IsTrue(sensor.IsActive);
    }

    [TestMethod]
    public void DeactivateIsActive_False_AndResetAlert()
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");
        sensor.Activate();
        sensor.RecordReading(10.0); 

        // Act
        sensor.Deactivate();

        // Assert
        Assert.IsFalse(sensor.IsActive);
        Assert.IsFalse(sensor.IsAlertTriggered);
    }

    [TestMethod]
    public void RecordReading_SensoInactiveException()
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");

        // Act
        Action act = () => sensor.RecordReading(5.0);

        // Assert
        Assert.ThrowsException<InvalidOperationException>(act);
    }

    [TestMethod]
    [DataRow(3.9, false)]  
    [DataRow(4.0, true)]   
    [DataRow(4.1, true)]   
    public void RecordReading_ShouldSetAlertStateCorrectly(double inputTemp, bool expectedAlert)
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");
        sensor.Activate(); 

        // Act
        sensor.RecordReading(inputTemp);

        // Assert
        Assert.AreEqual(expectedAlert, sensor.IsAlertTriggered);
    }

    
    [TestMethod]
    [DataRow(-50.0)]
    [DataRow(80.0)]
    public void RecordReading_WithinBoundaryValues(double temp)
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");
        sensor.Activate();

        // Act
        sensor.RecordReading(temp);

        // Assert
        Assert.AreEqual(temp, sensor.CurrentTemperature);
    }

    [TestMethod]
    [DataRow(-50.1)]
    [DataRow(80.1)]
    public void RecordReading_OutsideBoundaryValues(double temp)
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");
        sensor.Activate();

        // Act
        Action act = () => sensor.RecordReading(temp);

        // Assert
        Assert.ThrowsException<ArgumentOutOfRangeException>(act);
    }

    [TestMethod]
    public void VerifyDynamicUpdates_UpdatedThreshold()
    {
        // Arrange
        var sensor = new WarehouseSensor("Test1", "Sector1");
        sensor.Activate();

        // Act
        sensor.RecordReading(4.5);
        sensor.UpdateThreshold(5.0);
        

        // Assert
        Assert.IsFalse(sensor.IsAlertTriggered);
    }
}
