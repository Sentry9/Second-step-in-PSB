using ShoppingCart.Models;

namespace ShoppingCart.Validator;

public interface IValidator
{
    bool ValidateInputInt(string input, out int id);
    bool ValidateInputDouble(string input, out double sum);
    bool ValidateInputDate(string input, out DateOnly date);
    bool ValidateInputCount(string input, out int count);
    bool ValidateOrderJson(string path);
    bool ValidatePath(string path);
    bool ValidateDate(DateOnly date);
}