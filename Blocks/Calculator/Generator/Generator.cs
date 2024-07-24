using Calculator.Validator;
using Logger;

namespace Calculator.Generator;

public static class Generator
{
    private static Random random = new Random();

    public static void ChooseGenerate()
    {
        ILogger logger = new Logger.Logger();
        IValidatorCalc valid = new Validator.ValidatorCalc(logger);
        string choosen;
        do
        {
            Console.WriteLine(
                "Input '+' to generate possitive number\nInput '-' to generate negative number\nInput '1' to generate odd number\nInput '2' to generate even number");
            choosen = Console.ReadLine();
        } while (!valid.ValidateGenerator(choosen));

        switch (choosen)
        {
            case "+":
                Console.WriteLine(GeneratePositive());
                break;
            case "-":
                Console.WriteLine(GenerateNegative());
                break;
            case "2":
                Console.WriteLine(GenerateEven());
                break;
            case "1":
                Console.WriteLine(GenerateOdd());
                break;
        }
    }

    public static double GeneratePositive()
    {
        return random.NextDouble() + random.Next(0, int.MaxValue);
    }

    public static double GenerateNegative()
    {
        return random.NextDouble() + random.Next(int.MinValue, 0);
    }

    public static double GenerateEven()
    {
        int number = random.Next(int.MinValue / 2, int.MaxValue / 2) * 2;
        return number;
    }

    public static double GenerateOdd()
    {
        int number = random.Next(int.MinValue / 2, int.MaxValue / 2) * 2 + 1;
        return number;
    }
}