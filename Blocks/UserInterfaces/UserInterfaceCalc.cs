using Calculator.Calculator;
using Calculator.Generator;
using Calculator.Handler;
using Calculator.Validator;
using Logger;

namespace UserInterfaces;

public class UserInterfaceCalc : UserInterface
{
    public override void ChooseMode()
    {
        ILogger logger = new Logger.Logger();
        IValidatorCalc validCalc = new ValidatorCalc(logger);
        string input;
        do
        {
            do
            {
                Console.WriteLine("Выберите режим\n1 - калькулятор\n2 - генератор\ne - exit");
                input = Console.ReadLine();
            } while (!validCalc.ValidateStart(input));

            if (input == "1")
            {
                string answer;
                do
                {
                    Console.WriteLine("Отслеживать методы?");
                    answer = Console.ReadLine();
                } while (!validCalc.ValidateLogControl(answer));

                var loggerStatus = LoggerControl(answer);
                if (loggerStatus == LoggerStatus.On)
                {
                    ICalculator calc = new Calculator.Calculator.Calculator(logger, validCalc);
                    Handler handler = new Handler(validCalc, calc);
                    handler.CalcOrArray();
                }
                else
                {
                    ICalculator calc = new Calculator.Calculator.Calculator(null, validCalc);
                    Handler handler = new Handler(validCalc, calc);
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