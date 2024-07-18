namespace ShoppingCart.Exceptions;

public class JsonException : Exception
{
    public JsonException(string? message) : base(message)
    {
    }
}