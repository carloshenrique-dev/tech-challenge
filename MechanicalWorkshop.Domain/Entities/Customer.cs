namespace MechanicalWorkshop.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Document { get; private set; } = null!;
    public string DocumentType { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Vehicle> Vehicles => _vehicles.AsReadOnly();
    private readonly List<Vehicle> _vehicles = [];

    protected Customer() { }

    public Customer(string name, string document, string documentType, string email, string phone)
    {
        Id = Guid.NewGuid();
        Name = name;
        Document = document;
        DocumentType = documentType;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }
}