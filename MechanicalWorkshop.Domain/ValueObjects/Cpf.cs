using MechanicalWorkshop.Domain.Exceptions;

namespace MechanicalWorkshop.Domain.ValueObjects;

public class Cpf
{
    public string Value { get; }

    public Cpf(string value)
    {
        var cleaned = Clean(value);
        if (!IsValid(cleaned))
            throw new DomainException("Invalid CPF.");
        Value = cleaned;
    }

    public static bool IsValid(string cpf)
    {
        cpf = Clean(cpf);

        if (cpf.Length != 11) return false;
        if (cpf.Distinct().Count() == 1) return false;

        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += (cpf[i] - '0') * (10 - i);

        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;
        if (cpf[9] - '0' != firstDigit) return false;

        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += (cpf[i] - '0') * (11 - i);

        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;
        return cpf[10] - '0' == secondDigit;
    }

    private static string Clean(string value) =>
        new(value?.Where(char.IsDigit).ToArray() ?? []);

    public override string ToString() => Value;
}