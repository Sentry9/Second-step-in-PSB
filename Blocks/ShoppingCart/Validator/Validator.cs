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
                if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                  return false;
                }
            }
            
            var order = JsonSerializer.Deserialize<Order>(json);

            if (order.orderId < 0)
            {
                return false;
            }
            if (order.orderSum < 0)
            {
                return false;
            }
            if (order.orderWeight < 0)
            {
                return false;
            }
            if (!ValidateDate(order.DeliveryDate))
            {
                return false;
            }
            if (order.productsCount == null || order.products == null)
            {
                return false;
            }
            foreach (var productId in order.productsCount.Keys)
            {
                if (!order.products.ContainsKey(productId))
                {
                    return false;
                }
            }

            return true;
        }
        catch (JsonException ex)
        {
            throw new Exception("Ошибка при чтении JSON файла: " + ex.Message);
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