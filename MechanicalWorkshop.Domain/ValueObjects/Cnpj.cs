using MechanicalWorkshop.Domain.Exceptions;

namespace MechanicalWorkshop.Domain.ValueObjects;

public class Cnpj
{
    public string Value { get; }

    public Cnpj(string value)
    {
        var cleaned = Clean(value);
        if (!IsValid(cleaned))
            throw new DomainException("Invalid CNPJ.");
        Value = cleaned;
    }

    public static bool IsValid(string cnpj)
    {
        cnpj = Clean(cnpj);

        if (cnpj.Length != 14) return false;
        if (cnpj.Distinct().Count() == 1) return false;

        int[] multipliers1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multipliers2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        var sum = 0;
        for (var i = 0; i < 12; i++)
            sum += (cnpj[i] - '0') * multipliers1[i];

        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;
        if (cnpj[12] - '0' != firstDigit) return false;

        sum = 0;
        for (var i = 0; i < 13; i++)
            sum += (cnpj[i] - '0') * multipliers2[i];

        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;
        return cnpj[13] - '0' == secondDigit;
    }

    private static string Clean(string value) =>
        new(value?.Where(char.IsDigit).ToArray() ?? []);

    public override string ToString() => Value;
}