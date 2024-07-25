

using Calculator.Generator;
using Calculator.Validator;
using Logger;
using Calculator.Calculator;
using Calculator.Handler;
using ShoppingCart.Validator;
using UserInterfaces;
using Handler = ShoppingCart.Handler;

class Program
{
    public static void Main(string[] args)
    {
        UserInterfaceCalc calcInterface = new UserInterfaceCalc();
        IValidator validator = new Validator();
        UserInterfaceShoppingCart shopInterfaces = new UserInterfaceShoppingCart();
        do
        {
            Console.WriteLine("1 - Калькулятор\n2 - Корзина\ne - выход");
            string input = Console.ReadLine();
            if (input == "1")
            {
                calcInterface.ChooseMode();
            }

            if (input == "2")
            {
                shopInterfaces.ChooseMode();
            }

            if (input == "e")
            {
                break;
            }
        } while (true);
    }
}