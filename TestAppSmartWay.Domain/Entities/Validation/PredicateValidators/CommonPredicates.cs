using System.Text.RegularExpressions;

namespace TestAppSmartWay.Domain.Entities.Validation.PredicateValidators;

public static class CommonPredicates
{
    public static bool ValidatePhone(string phone)
    {
        return Regex.IsMatch(phone, @"^\+\d{1,3}\d{9,15}$");
    }

    public static bool ValidatePassportNumber(string number)
    {
        return Regex.IsMatch(number, "^\\d{4} \\d{6}$\n");
    }
}