

using Calculator.Generator;
using Calculator.Validator;
using Logger;
using Calculator.Calculator;
using Calculator.Handler;

class Program
{
    public static void Main(string[] args)
    {
        ILogger logger = new Logger.Logger();
        IValidator valid = new Validator(logger);
        string input;
        do
        {
            do
            {
                Console.WriteLine("1 - calculator\n2 - generator\ne - exit\n");
                input = Console.ReadLine();
            }while (!valid.ValidateStart(input));

            if (input == "1")
            {
                string answer;
                do
                {
                    Console.WriteLine("Track working methods?(y/n)\n");
                    answer = Console.ReadLine();
                } while (!valid.ValidateLogControl(answer));

                if (answer == "y")
                {
                   ICalculator calc = new Calculator.Calculator.Calculator(logger, valid);
                   logger.Write("Then now it was method LogControl from class Calculator\n");
                   Handler handler = new Handler(valid, calc);
                   handler.CalcOrArray();
                }
                else
                {
                    ICalculator calc = new Calculator.Calculator.Calculator(null, valid);
                    Handler handler = new Handler(valid, calc);
                    handler.CalcOrArray();
                }

            }
            else
            {
                if (input == "e")
                {
                    break;
                }
                else
                {
                    Generator.ChooseGenerate();
                }
            }
            
        } while (true);
    }
}