

using Calculator.Generator;
using Calculator.Validator;
using Logger;
using Calculator;
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
                    Calculator.Calculator calc = new Calculator.Calculator(logger, valid);
                    calc.Start();
                    logger.Write("Then now it was method LogControl from class Calculator\n");
                }
                else
                {
                    Calculator.Calculator calc = new Calculator.Calculator(null, valid);
                    calc.Start();
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