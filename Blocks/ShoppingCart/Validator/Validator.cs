using System.Text.Json;
using ShoppingCart.Models;
using System.Text.RegularExpressions;
using JsonException = ShoppingCart.Exceptions.JsonException;

namespace ShoppingCart.Validator;

public class Validator : IValidator
{
    public bool ValidateInputInt(string input, out int id)
    {
        return int.TryParse(input, out id);
    }

    public bool ValidateInputDouble(string input, out double sum)
    {
        return double.TryParse(input, out sum) && sum > 0;
    }

    public bool ValidateInputDate(string input, out DateOnly date)
    {
        return DateOnly.TryParse(input, out date) && ValidateDate(date);
    }

    public bool ValidateInputCount(string input, out int count)
    {
        return int.TryParse(input, out count) && count > 0;
    }

    public bool ValidateOrderJson(string json)
    {
        try
        {
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                JsonElement root = doc.RootElement;
                if (!root.TryGetProperty("orderId", out _)) return false;
                if (!root.TryGetProperty("orderSum", out _)) return false;
                if (!root.TryGetProperty("orderWeight", out _)) return false;
                if (!root.TryGetProperty("productsCount", out _)) return false;
                if (!root.TryGetProperty("DeliveryDate", out _)) return false;
                if (!root.TryGetProperty("products", out _)) return false;
                return true;
            }
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public bool ValidatePath(string path)
    {
        return !string.IsNullOrEmpty(path) && File.Exists(path) && path.EndsWith(".json");
    }

    public bool ValidateDate(DateOnly date)
    {
        return date > DateOnly.MinValue && date < DateOnly.MaxValue;
    }
}