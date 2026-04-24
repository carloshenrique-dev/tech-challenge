namespace MechanicalWorkshop.Domain.Entities;

public class Part
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public int MinimumStock { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Part() { }

    public Part(string name, string description, decimal unitPrice, int stockQuantity, int minimumStock)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        UnitPrice = unitPrice;
        StockQuantity = stockQuantity;
        MinimumStock = minimumStock;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, decimal unitPrice, int minimumStock)
    {
        Name = name;
        Description = description;
        UnitPrice = unitPrice;
        MinimumStock = minimumStock;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (quantity > StockQuantity) throw new InvalidOperationException("Insufficient stock.");
        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsLowStock() => StockQuantity <= MinimumStock;
}