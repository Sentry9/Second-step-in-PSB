using Calculator.Validator;
using Logger;

namespace Calculator.Calculator
{
    public class Calculator : ICalculator
    {
        private readonly ILogger _logger;
        private readonly IValidator _validator;
        private double Result;
        private int Len;
        private double NumCheck;

        public Calculator(ILogger logger, IValidator validator)
        {
            _logger = logger;
            _validator = validator;
        }

        public string Array()
        {
            if (_logger != null)
            {
                _logger.Log("Now starting method Array from class Calculator");
            }
            string len;
            string[] parts;
            bool checker;
            do
            {
                Console.WriteLine("Input len of array\n");
                len = Console.ReadLine();
            } while (!_validator.ValidateInt(len, out Len));

            do
            {
                checker = true;
                Console.WriteLine("Input numbers splited by space\n");
                var input = Console.ReadLine();
                parts = input.Split(' ');
                if (parts.Length != Len)
                {
                    checker = false;
                }
                else
                {
                    for (int i = 0; i < Len; i++)
                    {
                        if (!_validator.ValidateDouble(parts[i], out NumCheck))
                        {
                            checker = false;
                        }
                    }
                }
            } while (!checker);

            double[] numbers = new double[Len];
            for (int i = 0; i < parts.Length; i++)
            {
                numbers[i] = Convert.ToDouble(parts[i]);
            }
            if (_logger != null)
            {
                _logger.Log("Method Array from class Calculator finished without problems");
            }
            return ($"Max : {numbers.Max()}\nMin : {numbers.Min()}");
        }

        public virtual double Calculate(double num1, double num2, string op)
        {
            if (_logger != null)
            {
                _logger.Log("Now starting method Calculate from class Calculator");
            }

            switch (op)
            {
                case "+":
                    Result = num1 + num2;
                    break;
                case "-":
                    Result = num1 - num2;
                    break;
                case "*":
                    Result = num1 * num2;
                    break;
                case "/":
                    Result = num1 / num2;
                    break;
                default:
                    Console.WriteLine("Unsupported operation");
                    break;
            }
            if (_logger != null)
            {
                _logger.Log("Method Calculate from class Calculator finished without problems");
            }
            return Math.Round(Result, 4);
        }
    }
}
