using Logger;

namespace Calculator.Validator;

public class Validator : IValidator
{
    private readonly ILogger _logger;
    public Validator(ILogger logger)
    {
        _logger = logger;
    }
    public bool ValidateDouble(string input, out double num)
    {
        if (double.TryParse(input, out num))
        {
            return true;
        }
        else
        {
            _logger.Write("Invalid number input.\n");
            return false;
        }
    }

    public bool ValidateStart(string mode)
    {
        if (mode == "1" || mode == "2" || mode == "e")
        {
            return true;
        }
        else
        {
            _logger.Write("Invalid input.\n");
            return false;
        }
    }
    public bool ValidateInt(string input, out int num)
    {
        if (int.TryParse(input, out num))
        {
            return true;
        }
        else
        {
            _logger.Write("Invalid numbers input.\n");
            return false;
        }
    }

    public bool ValidateLen3(string[] input)
    {
        if (input.Length != 3)
        {
            _logger.Write("Invalid form of input\n");
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool ValidateOperation(string operation)
    {
        if (operation == "+" || operation == "-" || operation == "*" || operation == "/")
        {
            return true;
        }
        else
        {
            _logger.Write("Invalid operation input.\n");
            return false;
        }
    }

    public bool ValidateMode(string mode)
    {
        if (mode == "1" || mode == "2")
        {
            return true;
        }
        else
        {
            _logger.Write("Invalid mode input.\n");
            return false;
        }
    }

    public bool ValidateLogControl(string answer)
    {
        if (answer == "y" || answer == "n")
        {
            return true;
        }
        else
        {
            _logger.Write("Invalid input. Please enter 'y' or 'n'.\n");
            return false;
        }
    }

    public bool ValidateGenerator(string input)
    {
        if (input == "1" || input == "2" || input == "+" || input == "-")
        {
            return true;
        }
        else
        {
            _logger.Write("Invalid input\n");
            return false;
        }
    }
}