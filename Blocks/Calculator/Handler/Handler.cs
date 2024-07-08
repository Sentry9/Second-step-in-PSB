using Calculator.Calculator;
using Calculator.Validator;

namespace Calculator.Handler;

public class Handler
{
    private readonly IValidator _validator;
    private readonly ICalculator _calculator;
    private double Num1;
    private double Num2;
    private string Op;
    public Handler(IValidator validator, ICalculator calculator)
    {
        _validator = validator;
        _calculator = calculator;
    }
    public void CalcOrArray()
    {
        string mode;
        do
        {
            Console.WriteLine("Choose mode\n1 - Calculator\n2 - Arrays\n");
            mode = Console.ReadLine();
        } while (!_validator.ValidateMode(mode));

        if (mode == "1")
        {
            ModeInput();
        }
        else
        {
            Console.WriteLine(_calculator.Array());
        }
    }
    public void ModeInput()
    {
        string mode;
        do
        {
            Console.WriteLine("Choose mode\n1 - input by number\n2 - input in a row\n");
            mode = Console.ReadLine();
        } while (!_validator.ValidateMode(mode));

        if (mode == "1")
        {
            ByNumberInput();
        }
        else
        {
            InARowInput();
        }
    }

    public void ByNumberInput()
    {

        string num1;
        do
        {
            Console.WriteLine("Input first number\n");
            num1 = Console.ReadLine();
        } while (!_validator.ValidateDouble(num1, out Num1));

        string num2;
        do
        {
            Console.WriteLine("Input second number\n");
            num2 = Console.ReadLine();
        } while (!_validator.ValidateDouble(num2, out Num2));

        string op;
        do
        {
            Console.WriteLine("Input operation");
            op = Console.ReadLine();
        } while (!_validator.ValidateOperation(op));

        Op = op;
        Console.WriteLine(_calculator.Calculate(Num1, Num2, Op));
    }

    public void InARowInput()
    {

        string input;
        string[] parts;
        do
        {
            Console.WriteLine("Input your sum");
            input = Console.ReadLine();
            parts = input.Split(' ');
        } while (!_validator.ValidateLen3(parts) || !_validator.ValidateDouble(parts[0], out Num1) ||
                 !_validator.ValidateDouble(parts[2], out Num2) || !_validator.ValidateOperation(parts[1]));

        Op = parts[1];
        Console.WriteLine(_calculator.Calculate(Num1, Num2, Op));
    }
}