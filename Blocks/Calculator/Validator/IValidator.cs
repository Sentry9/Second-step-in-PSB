namespace Calculator.Validator;

public interface IValidator
{
    bool ValidateDouble(string input, out double num);
    bool ValidateOperation(string operation);
    bool ValidateMode(string mode);
    bool ValidateLogControl(string answer);
    bool ValidateInt(string input, out int num);
    bool ValidateStart(string mode);
    bool ValidateLen3(string[] input);
    bool ValidateGenerator(string input);
}