using System.Text.RegularExpressions;
using MechanicalWorkshop.Domain.Exceptions;

namespace MechanicalWorkshop.Domain.ValueObjects;

public partial class LicensePlate
{
    public string Value { get; }

    public LicensePlate(string value)
    {
        var cleaned = value?.ToUpper().Trim() ?? string.Empty;
        if (!IsValid(cleaned))
            throw new DomainException("Invalid license plate. Expected format: ABC1D23 (Mercosul) or ABC1234 (old format).");
        Value = cleaned;
    }

    public static bool IsValid(string plate)
    {
        plate = plate?.ToUpper().Trim() ?? string.Empty;
        return OldFormatRegex().IsMatch(plate) || MercosulFormatRegex().IsMatch(plate);
    }

    [GeneratedRegex(@"^[A-Z]{3}\d{4}$")]
    private static partial Regex OldFormatRegex();

    [GeneratedRegex(@"^[A-Z]{3}\d[A-Z]\d{2}$")]
    private static partial Regex MercosulFormatRegex();

    public override string ToString() => Value;
}