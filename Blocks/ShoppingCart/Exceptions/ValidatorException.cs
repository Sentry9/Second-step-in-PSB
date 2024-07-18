namespace ShoppingCart.Exceptions;

public class ValidatorException : Exception
{
    public ValidatorException(string? message) : base(message)
    {
    }
}