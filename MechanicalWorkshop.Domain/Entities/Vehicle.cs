namespace MechanicalWorkshop.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; private set; }
    public string LicensePlate { get; private set; } = null!;
    public string Brand { get; private set; } = null!;
    public string Model { get; private set; } = null!;
    public int Year { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Vehicle() { }

    public Vehicle(string licensePlate, string brand, string model, int year, Guid customerId)
    {
        Id = Guid.NewGuid();
        LicensePlate = licensePlate.ToUpper().Trim();
        Brand = brand;
        Model = model;
        Year = year;
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string brand, string model, int year)
    {
        Brand = brand;
        Model = model;
        Year = year;
        UpdatedAt = DateTime.UtcNow;
    }
}