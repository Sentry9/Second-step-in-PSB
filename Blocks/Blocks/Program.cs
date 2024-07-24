﻿

using Calculator.Generator;
using Calculator.Validator;
using Logger;
using Calculator.Calculator;
using Calculator.Handler;
using ShoppingCart.Validator;

class Program
{
    public static void Main(string[] args)
    {
        ILogger logger = new Logger.Logger();
        IValidatorCalc validCalc = new ValidatorCalc(logger);
        IValidator validCart = new Validator();
        
        string input;
        do
        {
            do
            {
                Console.WriteLine("1 - calculator\n2 - generator\ne - exit\n");
                input = Console.ReadLine();
            }while (!validCalc.ValidateStart(input));

            if (input == "1")
            {
                string answer;
                do
                {
                    Console.WriteLine("Track working methods?(y/n)\n");
                    answer = Console.ReadLine();
                } while (!validCalc.ValidateLogControl(answer));

                if (answer == "y")
                {
                   ICalculator calc = new Calculator.Calculator.Calculator(logger, validCalc);
                   logger.Log("Then now it was method LogControl from class Calculator\n");
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