using Calculator.Validator;
using Logger;

namespace Calculator
{
    public class Calculator
    {
        private readonly ILogger _logger;
        private readonly IValidator _validator;
        private double Num1;
        private double Num2;
        private string Op;
        private double Result;
        private bool LogStatus = true;
        private int Len;
        private double NumCheck;

        public Calculator(ILogger logger, IValidator validator)
        {
            _logger = logger;
            _validator = validator;
        }

        public void Start()
        {
            LogControl();
            CalcOrArray();
        }

        public void CalcOrArray()
        {
            if (LogStatus)
            {
                _logger.Write("Now starting method CalcOrArray from class Calculator");
            }

            string mode;
            do
            {
                Console.WriteLine("Choose mode\n1 - Calculator\n2 - Arrays\n");
                mode = Console.ReadLine();
            } while (!_validator.ValidateMode(mode));

            if (mode == "1")
            {
                ModeInput();
                Calculate();
            }
            else
            {
                Array();
            }

            if (LogStatus)
            {
                _logger.Write("Method CalcOrArray from class Calculator finished without problems");
            }
        }

        public void Array()
        {
            if (LogStatus)
            {
                _logger.Write("Now starting method Array from class Calculator");
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
            Console.WriteLine($"Max : {numbers.Max()}\nMin : {numbers.Min()}");
            if (LogStatus)
            {
                _logger.Write("Method Array from class Calculator finished without problems");
            }
        }
        public void LogControl()
        {
            if (_logger == null)
            {
                LogStatus = false;
            }
        }

        public void ModeInput()
        {
            if (LogStatus)
            {
                _logger.Write("Now starting method ModeInput from class Calculator");
            }

            string mode;
            do
            {
                Console.WriteLine("Choose mode\n1 - input by number\n2 - input in a row\n");
                mode = Console.ReadLine();
            } while (!_validator.ValidateMode(mode));

            ChooseMode(mode);

            if (LogStatus)
            {
                _logger.Write("Method ModeInput from class Calculator finished without problems");
            }
        }

        public void ChooseMode(string mode)
        {
            if (LogStatus)
            {
                _logger.Write("Now starting method ChooseMode from class Calculator");
            }

            if (mode == "1")
            {
                ByNumberInput();
            }
            else
            {
                InARowInput();
            }

            if (LogStatus)
            {
                _logger.Write("Method ChooseMode from class Calculator finished without problems");
            }
        }

        public void ByNumberInput()
        {
            if (LogStatus)
            {
                _logger.Write("Now starting method ByNumberInput from class Calculator");
            }

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

            if (LogStatus)
            {
                _logger.Write("Method ByNumberInput from class Calculator finished without problems");
            }
        }

        public void InARowInput()
        {
            if (LogStatus)
            {
                _logger.Write("Now starting method InARowInput from class Calculator");
            }

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

            if (LogStatus)
            {
                _logger.Write("Method InARowInput from class Calculator finished without problems");
            }
        }

        public void Calculate()
        {
            if (LogStatus)
            {
                _logger.Write("Now starting method Calculate from class Calculator");
            }

            switch (Op)
            {
                case "+":
                    Result = Num1 + Num2;
                    break;
                case "-":
                    Result = Num1 - Num2;
                    break;
                case "*":
                    Result = Num1 * Num2;
                    break;
                case "/":
                    Result = Num1 / Num2;
                    break;
                default:
                    Console.WriteLine("Unsupported operation");
                    return;
            }

            Console.WriteLine($"Result : {Result}");

            if (LogStatus)
            {
                _logger.Write("Method Calculate from class Calculator finished without problems");
            }
        }
    }
}
