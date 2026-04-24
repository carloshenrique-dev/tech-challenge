namespace MechanicalWorkshop.Domain.Entities;

public class Service
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal Price { get; private set; }
    public int EstimatedMinutes { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Service() { }

    public Service(string name, string description, decimal price, int estimatedMinutes)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        EstimatedMinutes = estimatedMinutes;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, decimal price, int estimatedMinutes)
    {
        Name = name;
        Description = description;
        Price = price;
        EstimatedMinutes = estimatedMinutes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}